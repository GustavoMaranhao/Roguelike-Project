using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ArrayUtils
{
    public static void AddUnitToUnitsArray(this List<UnitsBase> arrayToAdd, UnitsBase toAdd, bool checkIfOtherTypes = true)
    {
        if (arrayToAdd.Count > 0)
        {
            if (checkIfOtherTypes && toAdd.GetType() != arrayToAdd.First().GetType()) return;

            if (arrayToAdd.Contains(toAdd)) return;
        }

        arrayToAdd.Add(toAdd);
    }

    public static void AddUnitToUnitsArray(this List<UnitsBase> arrayToAdd, List<UnitsBase> toAddList, bool checkIfOtherTypes = true)
    {
        foreach (UnitsBase toAdd in toAddList)
        {
            if (arrayToAdd.Count > 0)
            {
                if (!checkIfOtherTypes && toAdd.GetType() != arrayToAdd.First().GetType()) return;

                if (arrayToAdd.Contains(toAdd)) return;
            }

            arrayToAdd.Add(toAdd);
        }
    }

    public static void RemoveUnitFromUnitsArray(this List<UnitsBase> arrayToRemove, UnitsBase toRemove)
    {
        if (!arrayToRemove.Contains(toRemove)) return;

        arrayToRemove.Remove(toRemove);
    }

    public static void RemoveUnitFromUnitsArray(this List<UnitsBase> arrayToRemove, List<UnitsBase> toRemoveList)
    {
        foreach (UnitsBase toRemove in toRemoveList)
        {
            if (!arrayToRemove.Contains(toRemove)) continue;

            arrayToRemove.Remove(toRemove);
        }
    }
}
