public class CharacterUnit : UnitsBase {

    // Overriding the parent base Start
    override protected void Start()
    {
        base.Start();

        isPlayerTeam = true;
    }

    // Overriding the parent base Update
    override protected void Update()
    {
        base.Update();

        #region Unit Movement
        isMoving = unitController.moveDirection.magnitude > 0;
        if(isMoving) animator.Play("Player Locomotion");
        #endregion
    }
}
