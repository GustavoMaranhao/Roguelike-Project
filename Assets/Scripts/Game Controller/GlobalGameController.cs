using System.Collections.Generic;
using UnityEngine;

public class GlobalGameController : MonoBehaviour {

    [HideInInspector]
    public List<UnitsBase> availableUnits;

    [HideInInspector]
    public List<UnitsBase> selectedUnits;

    [HideInInspector]
    public List<UnitsBase> highlightedUnits;

    void Start()
    {
        selectedUnits = new List<UnitsBase>();
        highlightedUnits = new List<UnitsBase>();

        availableUnits = new List<UnitsBase>();
        foreach (UnitsBase selectableObject in FindObjectsOfType(typeof(UnitsBase)))
        {
            availableUnits.AddUnitToUnitsArray(selectableObject, false);
        }
    }

    void Update()
    {

    }
}