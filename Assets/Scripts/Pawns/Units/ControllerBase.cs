using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBase : MonoBehaviour {
    [Range(0, 1)][Tooltip("Percentage of the Pawn's movement speed")]
    public float slowedSpeedModier = 0.2f;

    [Tooltip("Pawn's walk speed in Km/h")]
    public float pawnWalkSpeed = 2.5f;
    [Tooltip("Pawn's run speed in Km/h")]
    public float pawnRunSpeed = 3.5f;
    [Tooltip("Pawn's jump speed in number of Gs upward")]
    public float jumpSpeed = 13.0f;
    [Tooltip("Pawn's rotation speed in no specific unit")]
    public int rotateSpeed = 100;

    [Tooltip("Distance from the ground to consider jumping.")]
    public float groundedThreshold = 0.3f;

    [HideInInspector]
    public Vector3 moveDirection = Vector3.zero;

    [HideInInspector]
    public bool isEncumbered { get; private set; }

    protected GlobalGameController globalGameController;
    protected UnitsBase controlledUnit;

    protected bool isGrounded { get { return Physics.CheckCapsule(unitCollider.bounds.center, new Vector3(unitCollider.bounds.center.x, unitCollider.bounds.min.y - 1f, unitCollider.bounds.center.z), groundedThreshold) ? true : false; }}
    private bool _isGrounded = true;

    protected float inputHorizontal = 0.0f;
    protected float inputVertical = 0.0f;

    protected bool inputSprint = false;
    protected bool inputJump = false;

    private CharacterController pawnController;
    private float vertical_force;
    private Collider unitCollider;

    public GameObject test;

    // Use this for initialization
    protected virtual void Start () {
        globalGameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GlobalGameController>();
        controlledUnit = gameObject.GetComponentInChildren<UnitsBase>();
        unitCollider = controlledUnit.GetComponent<Collider>();

        pawnController = GetComponent<CharacterController>();
        isEncumbered = false;
    }
	
	// Update is called once per frame
	protected virtual void Update () {

	}

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        float z = pawnWalkSpeed * inputHorizontal * Time.deltaTime * (-Vector3.left.x);
        float x = pawnWalkSpeed * inputVertical * Time.deltaTime * (-Vector3.forward.z);

        // Recalculate movedirection directly from axes
        moveDirection = new Vector3(x, 0, z); //Determine the player's forward speed based upon the input.

        //Encumbered movement
        float modifier = 1f;
        if (isEncumbered)
            modifier *= slowedSpeedModier;

        //Sprint movement
        if (inputSprint)
        {
            moveDirection *= pawnRunSpeed * modifier;
        }
        else
        {
            moveDirection *= pawnWalkSpeed * modifier;
        }

        
        // Apply jump and gravity
        if (!_isGrounded)
        {
            if (vertical_force != 0)
                moveDirection.y -= (globalGameController.gravity * Time.fixedDeltaTime) - (vertical_force * Time.fixedDeltaTime);

            if (vertical_force > 0)
            {
                vertical_force -= globalGameController.gravity * Time.fixedDeltaTime;
            }
            if (vertical_force < 0)
            {
                vertical_force = 0;
            }
        }
        if (inputJump && transform.position.y < groundedThreshold)
        {
            vertical_force = jumpSpeed;
        }

        // Move the controller
        CollisionFlags flags = pawnController.Move(moveDirection);

        _isGrounded = (flags == CollisionFlags.Below);
    }
}
