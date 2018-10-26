using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUIMouseCursorController : MonoBehaviour
{
	[HideInInspector]
	public Vector3 worldPoint;
	[HideInInspector]
	public RaycastHit cursorRayHit;
    [HideInInspector]
    public Transform ObjectUnderMouse;

    [HideInInspector]
    public bool isMouseDragging;
    [HideInInspector]
    public Vector2 mouseDragStart = Vector2.zero;

    [HideInInspector]
    public Vector2 mousePosition;

    public ParticleSystem movementClickParticle;

    private Camera mainCamera;
	private Vector3 cameraPos;
	private float camDistance;

	private Ray cursorRay;

    private GlobalGameController globalGameController;

    private List<UnitsBase> addToUnitSelection = new List<UnitsBase>();
    private List<UnitsBase> removeFromUnitSelection = new List<UnitsBase>();

    // RayCasting ignoring default layer (number 2), "Terrain"(number 11) "Camera" (number 8) and "BuildTemplate" (number 10) layers, 
    // it will collide with 1s and ignore 0s
    // Bitshifting the layers 2, 8, 9 and 11 and inverting the integer in binary
    private static int ignoreLayerMask = ~((1 << 2) + (1 << Tags.TerrainLayer) + (1 << Tags.CameraLayer));

	void Start () {
        globalGameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GlobalGameController>();
        mainCamera = GameObject.FindWithTag(Tags.gameCamera).GetComponent<Camera>();
		cameraPos = mainCamera.transform.position;
	}

    void Update()
    {
         #region Mouse Raycasting
        cursorRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        camDistance = Mathf.Sqrt(Mathf.Pow(cameraPos.x, 2) + Mathf.Pow(cameraPos.y, 2));

        if (Physics.Raycast(cursorRay, out cursorRayHit, (int)camDistance, ignoreLayerMask))
        {
            ObjectUnderMouse = cursorRayHit.collider.transform;
            worldPoint = cursorRay.GetPoint(cursorRayHit.distance);
        }
        #endregion

        #region Mouse HUD Events
        mousePosition = Input.mousePosition;

        //Left Mouse Down
        if (Input.GetMouseButtonDown(0))
        {
            globalGameController.highlightedUnits.Clear();
            if (!isMouseDragging)
            {
                if (mouseDragStart.Equals(Vector2.zero))
                {
                    mouseDragStart = Input.mousePosition;
                }

                globalGameController.highlightedUnits.Clear();
                globalGameController.selectedUnits.Clear();

                isMouseDragging = true;
            }
        }

        //Left Mouse Up
        if (Input.GetMouseButtonUp(0))
        {
            //Mouse drag just ended
            if (isMouseDragging)
            {
                //Check if anything was selected
                if (addToUnitSelection.Count > 0)
                {
                    globalGameController.selectedUnits.Clear();
                    globalGameController.selectedUnits.AddUnitToUnitsArray(addToUnitSelection);
                    addToUnitSelection.Clear();
                }

                mouseDragStart = Vector2.zero;
                isMouseDragging = false;
            }

            if (removeFromUnitSelection.Count > 0)
            {
                removeFromUnitSelection.ForEach(unitToRemove => 
                {
                    globalGameController.selectedUnits.Remove(unitToRemove);
                });
                removeFromUnitSelection.Clear();
            }
        }

        //Mouse drag button held
        if (isMouseDragging)
        {
            foreach (var selectableObject in globalGameController.availableUnits)
            {
                if (IsWithinSelectionBounds(selectableObject.gameObject))
                {
                    //Add unit to the temporary array
                    addToUnitSelection.AddUnitToUnitsArray(selectableObject);

                    //Highlight the selected unit
                    globalGameController.highlightedUnits.AddUnitToUnitsArray(selectableObject);
                }
                else
                {
                    //Remove unit from the temporary array
                    addToUnitSelection.RemoveUnitFromUnitsArray(selectableObject);

                    //Remove unit from the highlights array
                    globalGameController.highlightedUnits.RemoveUnitFromUnitsArray(selectableObject); 

                }
            }
        }

        //Right Mouse Up
        if (Input.GetMouseButtonUp(1))
        {
            //Activate a movement particle
            if(movementClickParticle != null)
            {
                movementClickParticle.transform.position = worldPoint;
                movementClickParticle.Play();
            }
        }
        #endregion
    }

    bool IsWithinSelectionBounds(GameObject gameObject)
    {
        var viewportBounds = GUIUtils.GetViewportBoundsFromWorld(mainCamera, mouseDragStart, Input.mousePosition);
        return viewportBounds.Contains(mainCamera.WorldToViewportPoint(gameObject.transform.position));
    }
}
