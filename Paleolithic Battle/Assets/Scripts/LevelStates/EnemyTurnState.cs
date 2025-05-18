using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnState : ILevelState
{
    private LevelManager levelManager;


    public EnemyTurnState(LevelManager levelManager){
        this.levelManager = levelManager;
    }

    public void EnterState()
    {
        // Move all enemy units
        foreach (var unit in levelManager.enemyUnits)
        {
            MoveEnemyUnit(unit);
        }

        // Capture the cell it the unit can or attack the player if it is in range
        foreach (var unit in levelManager.enemyUnits)
        {
            if (unit.currentCell.capturable && !unit.currentCell.enemy){
                levelManager.CaptureCell(unit.currentCell, false);
            }
            else{
                AttackEnemyUnit(unit);
            }
        }

        // Train the units in the camps if they are not occupied
        foreach (var camp in levelManager.enemyCamps)
        {
            if (camp.isOccupied) continue;
            TrainEnemyUnit(camp);
        }

        GoToPlayerTurnState();
    }

    private void MoveEnemyUnit(IUnit unit){
        // Get the available cells for the unit
        List<Cell> availableCells = levelManager.GetAvailableMoveCells(unit.currentCell);

        // Move the unit to a random available cell
        if (availableCells.Count > 0)
        {
            int randomIndex = Random.Range(-1, availableCells.Count);
            if(randomIndex < 0)
                return;
            Cell targetCell = availableCells[randomIndex];
            // Move the unit to the target cell
            levelManager.MoveUnit(unit, targetCell);
        }
    }

    private void TrainEnemyUnit(Cell camp){
        Debug.Log("Train unit in camp: " + camp.x + "," + camp.y);
        List<UnitType> availableUnits = new List<UnitType>();
        foreach (UnitType unitType in levelManager.unitTypes)
        {
            if (unitType == UnitType.None || !levelManager.CanAffordUnit(unitType, false)) continue;
            Debug.Log("Available unit: " + unitType);
            availableUnits.Add(unitType);
        }
        if (availableUnits.Count == 0) return;
        int randomIndex = Random.Range(0, availableUnits.Count);
        levelManager.CreateUnit(availableUnits[randomIndex], camp, false);
    }

    private void AttackEnemyUnit(IUnit unit){
        // Get the available cells for the unit
        List<Cell> availableCells = levelManager.GetAvailableAttackCells(unit.currentCell);

        // Attack the unit to a random available cell
        if (availableCells.Count > 0)
        {
            int randomIndex = Random.Range(-1, availableCells.Count);
            if(randomIndex < 0)
                return;
            Cell targetCell = availableCells[randomIndex];
            // Move the unit to the target cell
            Debug.Log("Attack unit: " + unit.unitType + " to cell: " + targetCell.x + "," + targetCell.y);
            levelManager.AttackUnit(unit, targetCell.unit);
        }
    }

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