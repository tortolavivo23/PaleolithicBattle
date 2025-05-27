using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class AttackNode : BTNode
{
    private LevelManager levelManager;
    private HashSet<IUnit> actedUnits = new();

    public AttackNode(LevelManager manager) => levelManager = manager;

    public void Reset()
    {
        actedUnits.Clear();
    }

    public override BTState Tick(IUnit unit)
    {
        Debug.Log("AttackNode");
        if (actedUnits.Contains(unit))
            return BTState.Failure;
        actedUnits.Add(unit);
        List<Cell> targets = levelManager.GetAvailableAttackCells(unit.currentCell);
        if (targets.Count == 0) return BTState.Failure;

        var best = targets.OrderByDescending(t => EvaluateAttack(unit, t.unit)).FirstOrDefault();
        if (best == null) return BTState.Failure;

        levelManager.AttackUnit(unit, best.unit);
        return BTState.Success;
    }

    private float EvaluateAttack(IUnit attacker, IUnit target)
    {
        float score = 0f;
        score += 100f * (1f - (float)target.health / target.maxHealth);
        switch (target.unitType)
        {
            case UnitType.Heavy: score += 50f; break;
            case UnitType.Water: score += 30f; break;
            case UnitType.Range: score += 20f; break;
            case UnitType.Base: score += 10f; break;
        }
        if (attacker.CanKill(target))
            score += 200f;
        return score;
    }
}