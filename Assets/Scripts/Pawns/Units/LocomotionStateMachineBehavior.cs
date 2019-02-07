using UnityEngine;

public class LocomotionStateMachineBehavior : StateMachineBehaviour {
    private HashIDs hash;

    public float m_Damping = 0.15f;

    public float sensitivity = 1000f;

    private static int ignoreLayerMask = ~((1 << 2) + (1 << Tags.CameraLayer));
    private static int onlyLayerMask = (1 << Tags.TerrainLayer);

    // Use this for initialization
    private void Awake()
    {
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 input = new Vector2(horizontal, vertical).normalized;

        animator.SetFloat(hash.horizontalParameter, input.x, m_Damping, Time.deltaTime);
        animator.SetFloat(hash.verticalParameter, input.y, m_Damping, Time.deltaTime);

        /*
        var targetRotation = Quaternion.LookRotation(targetObj.transform.position - transform.position);
        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        */

        RaycastHit cursorRayHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out cursorRayHit))
        {
            var test = ray.GetPoint(cursorRayHit.distance);
            test.x = 0;
            test.z = 0;
            var targetRotation = Quaternion.LookRotation(test);

            animator.gameObject.transform.LookAt(ray.GetPoint(cursorRayHit.distance));

            //animator.gameObject.transform.LookAt(ray.GetPoint(cursorRayHit.distance));
        }

        //var targetRotation = Quaternion.LookRotation(new Vector3(-vertical, 0, horizontal));
        // Smoothly rotate towards the target point.
        //animator.gameObject.transform.rotation = Quaternion.Slerp(animator.gameObject.transform.rotation, targetRotation, 5 * Time.deltaTime);
        //animator.gameObject.transform.rotation = targetRotation;

        var currentPosition = animator.gameObject.transform.position;
        var lookingPosition = new Vector3(currentPosition.x + horizontal, currentPosition.y + vertical, 0);
        //animator.gameObject.transform.LookAt(lookingPosition, Vector3.up);


        /*Vector3 lookPos = cameraRef.transform.position - transform.position;
        float angle = Mathf.Atan2(lookPos.x, lookPos.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle + 180, Vector3.up), Time.deltaTime * 10);*/


        //Debug.Log("Looking at: " + lookingPosition);
    }
}
