using UnityEngine;

public interface ILevelState
{
    void UpdateState();
    void GoToPlayerTurnState();
    void GoToPreviewMoveState();
    void GoToEnemyTurnState();
    void GoToMenuState();
    void GoToAttackState();
    void GoToTrainState();
}