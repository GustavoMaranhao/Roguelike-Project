using System.Collections.Generic;
using UnityEngine;

public class GlobalGameController : MonoBehaviour {

    [HideInInspector]
    public Camera playerCameraRef;

    [HideInInspector]
    public List<UnitsBase> availableUnits;

    [HideInInspector]
    public List<UnitsBase> selectedUnits;

    [HideInInspector]
    public List<UnitsBase> highlightedUnits;

    private void Awake()
    {
        playerCameraRef = Camera.main;
    }

    void Start()
    {
    }

    void Update()
    {
    }
}