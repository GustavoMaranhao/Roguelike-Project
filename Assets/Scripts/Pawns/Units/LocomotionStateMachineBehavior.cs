using UnityEngine;

public class LocomotionStateMachineBehavior : StateMachineBehaviour {
    private HashIDs hash;

    public float m_Damping = 0.15f;

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

        var targetRotation = Quaternion.LookRotation(new Vector3(-vertical, 0, horizontal));
        // Smoothly rotate towards the target point.
        animator.gameObject.transform.rotation = Quaternion.Slerp(animator.gameObject.transform.rotation, targetRotation, 5 * Time.deltaTime);

        var currentPosition = animator.gameObject.transform.position;
        var lookingPosition = new Vector3(currentPosition.x + horizontal, currentPosition.y + vertical, 0);
        //animator.gameObject.transform.LookAt(lookingPosition, Vector3.up);
        //Debug.Log("Looking at: " + lookingPosition);
    }
}
