using UnityEngine;

public class EnemyTurnState : ILevelState
{
    private LevelManager levelManager;

    public EnemyTurnState(LevelManager levelManager){
        this.levelManager = levelManager;
    }

    public void GoToAttackState()
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
        throw new System.NotImplementedException();
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
}