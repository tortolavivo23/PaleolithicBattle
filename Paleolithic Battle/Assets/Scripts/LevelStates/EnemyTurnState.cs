using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTurnState : ILevelState
{
    private LevelManager levelManager;


    public EnemyTurnState(LevelManager levelManager){
        this.levelManager = levelManager;
    }

    public void EnterState()
    {
        foreach (var unit in levelManager.enemyUnits)
        {
            levelManager.StartCoroutine(UnitTurn(unit));
        }

        foreach (var camp in levelManager.enemyCamps)
        {
            if (!camp.isOccupied)
                TrainEnemyUnit(camp);
        }

        GoToPlayerTurnState();
    }

    IEnumerator UnitTurn(IUnit unit)
    {
        MoveEnemyUnit(unit);
        yield return new WaitForSeconds(0.5f); // Esperar un poco antes de la siguiente acción

        if (unit.currentCell.capturable && !unit.currentCell.enemy)
        {
            levelManager.CaptureCell(unit.currentCell, false);
        }
        else
        {
            AttackEnemyUnit(unit);
        }
    }

    private void MoveEnemyUnit(IUnit unit)
    {
        Dictionary<Cell, Cell> availableCells = levelManager.GetAvailableMoveCells(unit.currentCell);
        if (availableCells.Count == 0) return;

        Cell bestCell = availableCells.Keys
            .OrderByDescending(cell => EvaluateMoveCell(unit, cell))
            .FirstOrDefault();

        if (bestCell != null)
            levelManager.MoveUnit(unit, bestCell, availableCells);
    }

    private void AttackEnemyUnit(IUnit unit)
    {
        List<Cell> attackCells = levelManager.GetAvailableAttackCells(unit.currentCell);
        if (attackCells.Count == 0) return;

        Cell bestTargetCell = attackCells
            .OrderByDescending(cell => EvaluateAttack(unit, cell.unit))
            .FirstOrDefault();

        if (bestTargetCell != null)
        {
            Debug.Log($"Attack unit: {unit.unitType} → {bestTargetCell.unit.unitType} at ({bestTargetCell.x},{bestTargetCell.y})");
            levelManager.AttackUnit(unit, bestTargetCell.unit);
        }
    }

    private void TrainEnemyUnit(Cell camp)
    {
        Debug.Log("Train unit in camp: " + camp.x + "," + camp.y);

        List<UnitType> availableUnits = levelManager.unitTypes
            .Where(t => t != UnitType.None && levelManager.CanAffordUnit(t, false))
            .ToList();

        if (availableUnits.Count == 0) return;

        UnitType selected = availableUnits[Random.Range(0, availableUnits.Count)];
        levelManager.CreateUnit(selected, camp, false);
    }

    // ─────────────────────────────────────────────────────────────
    // Heurísticas auxiliares

    private float EvaluateMoveCell(IUnit unit, Cell cell)
    {
        float score = 0f;

        if (cell.capturable && !cell.enemy)
            score += 100f;

        if (cell.unit != null)
            score -= 1000f;

        IUnit closestEnemy = FindClosestEnemy(unit, cell);
        if (closestEnemy != null)
        {
            float distance = levelManager.GetDistance(cell, closestEnemy.currentCell);
            score += 50f / (distance + 1);
        }

        return score;
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

    private IUnit FindClosestEnemy(IUnit fromUnit, Cell fromCell)
    {
        return levelManager.playerUnits
            .OrderBy(enemy => levelManager.GetDistance(fromCell, enemy.currentCell))
            .FirstOrDefault();
    }

    // ─────────────────────────────────────────────────────────────

    public void ExitState()
    {
        
    }

    public void GoToPreviewAttackState()
    {
        throw new System.NotImplementedException();
    }

    public void GoToEnemyTurnState()
    {
        throw new System.NotImplementedException();
    }

    public void GoToMenuState()
    {
        throw new System.NotImplementedException();
    }

    public void GoToPlayerTurnState()
    {
        levelManager.currentTurn++; // Aumentar el turno actual
        levelManager.AddMoney(true); // Añadir dinero al jugador
        levelManager.ChangeState(levelManager.playerTurnState); // Cambiamos el estado actual a PlayerTurnState
    }

    public void GoToPreviewMoveState()
    {
        throw new System.NotImplementedException();
    }

    public void GoToTrainState()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    public void GoToAttackState()
    {
        throw new System.NotImplementedException();
    }
}