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
    }
}
