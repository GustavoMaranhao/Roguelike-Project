using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitsBase : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Check if this unit shoould be allied to the player.")]
    public bool isPlayerTeam = false;

    [Tooltip("Speed with which the unit runs.")]
    public float unitSpeed = 10f;

    [Tooltip("Distance from which the unit can start stopping when approaching its movement destination.")]
    public float walkStopDistance = 0.5f;

    private GlobalGameController globalGameController;
    private Projector selectionCircleProjector;

    private Camera cameraRef;
    private GUIMouseCursorController cursorController;

    private bool isSelected = false;
    private bool isHighlighted = false;

    private float rotationSpeed = 10f;
    private float walkTimer;
    private UnityEngine.AI.NavMeshAgent nav;
    private Vector3 destination;
    private Animator animator;

    void Awake()
    {
        cameraRef = Camera.main.GetComponent<Camera>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        destination = gameObject.transform.position;

        animator = GetComponent<Animator>();
        nav.speed = unitSpeed;

        cursorController = GameObject.FindWithTag(Tags.gameController).GetComponent<GUIMouseCursorController>();
    }

    protected virtual void Start()
    {
        selectionCircleProjector = gameObject.GetComponentInChildren<Projector>();
        globalGameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GlobalGameController>();

        globalGameController.availableUnits.AddUnitToUnitsArray(this, false);
    }

    protected virtual void Update()
    {
        #region Unit Selection
        if (globalGameController.selectedUnits.Contains(this))
            isSelected = true;
        else
            isSelected = false;

        if (globalGameController.highlightedUnits.Contains(this))
            isHighlighted = true;
        else
            isHighlighted = false;

        selectionCircleProjector.enabled = isSelected || isHighlighted;
        #endregion

        #region Unit Movement
        if (isSelected && isPlayerTeam)
        {
            if (Input.GetMouseButtonUp(1))
            {
                destination = cursorController.worldPoint;

                animator.SetBool("bShouldMove", true);
                animator.Play("Locomotion");
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion"))
            {
                if (Vector3.Distance(gameObject.transform.position, destination) > walkStopDistance)
                    nav.destination = this.destination;
                else
                {
                    animator.SetBool("bShouldMove", false);
                }

                Vector3 lookDirection = (destination - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
        }
        #endregion
    }

    private void OnDestroy()
    {
        //Arrays cleanup
        if (globalGameController.highlightedUnits.Contains(this)) globalGameController.highlightedUnits.Remove(this);
        if (globalGameController.selectedUnits.Contains(this)) globalGameController.selectedUnits.Remove(this);
        if (globalGameController.availableUnits.Contains(this)) globalGameController.availableUnits.Remove(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.clickCount)
        {
            case 1:
                globalGameController.selectedUnits.Clear();
                globalGameController.highlightedUnits.Clear();

                globalGameController.selectedUnits.AddUnitToUnitsArray(this);
                globalGameController.highlightedUnits.AddUnitToUnitsArray(this);
                break;
            case 2:
                foreach (UnitsBase selectableObject in globalGameController.availableUnits)
                {
                    if (!GUIUtils.IsWithinViewport(cameraRef, selectableObject.gameObject)) continue;

                    globalGameController.selectedUnits.AddUnitToUnitsArray(selectableObject);
                    globalGameController.highlightedUnits.AddUnitToUnitsArray(selectableObject);
                }
                break;
            default:
                break;
        }
    }
}
