using System.Collections.Generic;
using UnityEngine;

public class GlobalGameController : MonoBehaviour {
    [Tooltip("Global gravity")]
    public float gravity = GlobalConstants.Gravity;

    [HideInInspector]
    public Camera playerCameraRef;

    [HideInInspector]
    public Transform playerRef;

    [HideInInspector]
    public ControllerBase playerController;

    [HideInInspector]
    public UnitsBase playerUnit;

    private void Awake()
    {
        playerCameraRef = Camera.main;
        playerRef = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Transform>();
        playerController = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<ControllerBase>();
        playerUnit = GameObject.FindGameObjectWithTag(Tags.player).GetComponentInChildren<UnitsBase>();
    }

    void Start()
    {
    }

    void Update()
    {
    }
}