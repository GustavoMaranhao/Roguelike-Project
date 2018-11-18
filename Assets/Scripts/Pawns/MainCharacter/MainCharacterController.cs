using UnityEngine;

public class MainCharacterController : ControllerBase {
	[Tooltip("Zoom step increment")]
	public float zoomIncrement = 20.0f;
	[Tooltip("Zoom maximum step")]
	public int maxZoom = 80;
	[Tooltip("Zoom minimum step")]
	public int minZoom = 30;

	[Tooltip("Mouse sensitivity for rotating the camera around.")]
	public float sensitivity = 1000f;

	private Camera cameraRef;

    private float rot_x, rot_y = 0.0f;
    private float distance = 0f;

    // Overriding the parent base start
    override protected void Start()
    {
        base.Start();

        cameraRef = globalGameController.playerCameraRef;

        FreeCamStartSetup();
    }

    // Overriding the parent base start
    override protected void Update()
    {
        base.Update();
    }

	override protected void FixedUpdate () {
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
        inputSprint = Input.GetButton("Sprint");
        inputJump = Input.GetButton("Jump");

        base.FixedUpdate();

		//Camera Zoom
		float mouseWheel = -Input.GetAxis ("Mouse ScrollWheel");
		if (mouseWheel != 0)
			cameraRef.fieldOfView = Mathf.Clamp (cameraRef.fieldOfView + mouseWheel * zoomIncrement, minZoom, maxZoom);
    }

	void FreeCamStartSetup(){
		distance = Vector3.Distance (cameraRef.transform.position, transform.position);

		rot_x = -cameraRef.transform.rotation.eulerAngles.x;
		rot_y = cameraRef.transform.rotation.eulerAngles.y + 180;

		cameraRef.transform.position = transform.position + Quaternion.Euler (rot_x, rot_y, 0f) * (distance * -Vector3.back);
		cameraRef.transform.LookAt (transform.position, Vector3.up);
	}
}