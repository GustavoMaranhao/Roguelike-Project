using UnityEngine;

public class UnitsBase : MonoBehaviour
{
    [Tooltip("Check if this unit shoould be allied to the player.")]
    public bool isPlayerTeam = false;

    [Tooltip("Speed with which the unit runs.")]
    public float unitSpeed = 10f;
    [Tooltip("Speed with which the unit turns.")]
    public float rotationSpeed = 1f;

    [HideInInspector]
    public bool isMoving = false;

    private ControllerBase unitController;
    private Animator animator;

    void Awake()
    {
        unitController = transform.parent.gameObject.GetComponent<ControllerBase>();

        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {       
        #region Unit Movement
        if (unitController.moveDirection.magnitude > 0)
        {
            isMoving = true;

            animator.SetBool("bShouldMove", true);
            animator.Play("Locomotion");

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion"))
            {
                Vector3 lookDirection = (unitController.moveDirection - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
        }
        else
        {
            isMoving = false;

            animator.SetBool("bShouldMove", false);
        }
        #endregion
    }
}
