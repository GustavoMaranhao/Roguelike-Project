using UnityEngine;

public class UnitsBase : MonoBehaviour
{
    [Tooltip("Check if this unit shoould be allied to the player.")]
    protected bool isPlayerTeam = false;

    [HideInInspector]
    public bool isMoving = false;

    protected ControllerBase unitController;
    protected Animator animator;

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

    }
}
