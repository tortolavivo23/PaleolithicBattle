using System.Collections.Generic;
using UnityEngine;
using System.Linq;

class MoveNode : BTNode
{
    private LevelManager levelManager;
    private Dictionary<IUnit, bool> isMoving = new();

    public MoveNode(LevelManager manager) => levelManager = manager;

    public override BTState Tick(IUnit unit)
    {
        if (!isMoving.ContainsKey(unit))
        {
            Debug.Log("MoveNode");
            Dictionary<Cell, Cell> availableCells = levelManager.GetAvailableMoveCells(unit.currentCell);
            if (availableCells.Count == 0) return BTState.Failure;

            Cell best = availableCells.Keys
                .OrderByDescending(cell => EvaluateMoveCell(cell, unit))
                .FirstOrDefault();
            if (best == null || best.isOccupied) return BTState.Failure;

            isMoving[unit] = true;
            unit.OnMovementFinished = () => isMoving[unit] = false;
            levelManager.MoveUnit(unit, best, availableCells);
            return BTState.Running;
        }

        if (isMoving[unit]) return BTState.Running;

        isMoving.Remove(unit);
        return BTState.Success;
    }

    private float EvaluateMoveCell(Cell cell, IUnit unit)
    {
        float score = 0f;

        // Penalización si está ocupada
        if (cell.isOccupied)
            return -10000f; // early return, esta celda no sirve

        // Si es unidad Base, quiere capturar (situarse sobre la celda)
        if (unit.unitType == UnitType.Base && cell.capturable && !cell.enemy)
            score += 100f;

        // Si no es Base, prioriza estar cerca de celdas capturables pero sin ocuparlas
        if (unit.unitType != UnitType.Base)
        {
            var nearestCapturable = levelManager.GetAllCells()
                .Where(c => c.capturable && !c.enemy && !c.isOccupied)
                .OrderBy(c => levelManager.RealDistance(cell, c, unit))
                .FirstOrDefault();

            if (nearestCapturable != null)
            {
                int distToCapturable = levelManager.RealDistance(cell, nearestCapturable, unit);
                if (distToCapturable != int.MaxValue)
                    score += 30f / (distToCapturable + 1); // cuanto más cerca, mejor
            }
        }

        // Proximidad a la base enemiga
        int distToBase = levelManager.RealDistance(cell, levelManager.playerBase, unit);
        if (distToBase != int.MaxValue)
            score += 100f / (distToBase + 1);

        // Proximidad a enemigos
        IUnit closestEnemy = FindClosestEnemy(cell);
        if (closestEnemy != null)
        {
            int distToEnemy = levelManager.RealDistance(cell, closestEnemy.currentCell, unit);
            if (distToEnemy != int.MaxValue)
            {
                if (unit.unitType == UnitType.Range)
                    score += 0;//distToEnemy * 5f; // alejarse
                else
                    score += 50f / (distToEnemy + 1); // acercarse
            }
        }

        // Penalizar acumulación de aliados en zona cercana
        var nearbyAllies = levelManager.GetUnitsInRange(cell, 2)
            .Where(u => u.playerUnit == unit.playerUnit)
            .Count();

        score -= nearbyAllies * 2.5f;

        return score;
    }




    private IUnit FindClosestEnemy(Cell fromCell)
    {
        return levelManager.playerUnits
            .OrderBy(enemy => levelManager.GetDistance(fromCell, enemy.currentCell))
            .FirstOrDefault();
    }


}