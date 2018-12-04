using UnityEngine;

public class EnemyUnit : UnitsBase
{
    [Tooltip("Speed with which the unit turns.")]
    public float rotationSpeed = 1f;

    // Overriding the parent base Start
    override protected void Start()
    {
        base.Start();

        isPlayerTeam = false;
    }

    // Overriding the parent base Update
    override protected void Update()
    {
        base.Update();

        #region Unit Movement
        if (unitController.moveDirection.magnitude > 0)
        {
            animator.SetBool("bNpcShouldMove", true);
            animator.Play("NPC Locomotion");

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("NPC Locomotion"))
            {
                Vector3 lookDirection = (unitController.moveDirection - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
        }
        else
        {
            animator.SetBool("bNpcShouldMove", false);
        }
        #endregion
    }
}
