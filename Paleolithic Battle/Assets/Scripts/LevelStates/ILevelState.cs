using UnityEngine;

public interface ILevelState
{
    void UpdateState();
    void GoToPlayerTurnState();
    void GoToPreviewMoveState();
    void GoToEnemyTurnState();
    void GoToMenuState();
    void GoToPreviewAttackState();
    void GoToTrainState();

    void ExitState();

    void EnterState();

    void GoToAttackState();
}