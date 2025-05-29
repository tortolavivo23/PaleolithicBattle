

using System.Collections.Generic;
using UnityEngine;

class CaptureNode : BTNode
{
    private LevelManager levelManager;
    private HashSet<IUnit> actedUnits = new();

    public CaptureNode(LevelManager manager) => levelManager = manager;

    public void Reset()
    {
        actedUnits.Clear();
    } 

    public override BTState Tick(IUnit unit)
    {
        if (actedUnits.Contains(unit))
            return BTState.Failure;
        if (unit.unitType == UnitType.Base && unit.currentCell.capturable && !unit.currentCell.enemy)
        {
            levelManager.CaptureCell(unit.currentCell, false);
            Debug.Log("Captured cell: " + unit.currentCell);
            actedUnits.Add(unit);
            return BTState.Success;
        }
        return BTState.Failure;
    }
}