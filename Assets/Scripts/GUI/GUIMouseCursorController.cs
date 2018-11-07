using UnityEngine;

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

    private Camera cameraRef;
	private Vector3 cameraPos;
	private float camDistance;

	private Ray cursorRay;

    private GlobalGameController globalGameController;

    // RayCasting ignoring default layer (number 2), "Terrain"(number 11) "Camera" (number 8) and "BuildTemplate" (number 10) layers, 
    // it will collide with 1s and ignore 0s
    // Bitshifting the layers 2, 8, 9 and 11 and inverting the integer in binary
    private static int ignoreLayerMask = ~((1 << 2) + (1 << Tags.TerrainLayer) + (1 << Tags.CameraLayer));

	void Start () {
        globalGameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GlobalGameController>();
        cameraRef = globalGameController.playerCameraRef;
        cameraPos = cameraRef.transform.position;
	}

    void Update()
    {
        #region Mouse Raycasting
        cursorRay = cameraRef.ScreenPointToRay(Input.mousePosition);
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
            if (!isMouseDragging)
            {
                if (mouseDragStart.Equals(Vector2.zero))
                {
                    mouseDragStart = Input.mousePosition;
                }
                isMouseDragging = true;
            }
        }

        //Left Mouse Up
        if (Input.GetMouseButtonUp(0))
        {
            //Mouse drag just ended
            if (isMouseDragging)
            {
                mouseDragStart = Vector2.zero;
                isMouseDragging = false;
            }
        }
        #endregion
    }
}
