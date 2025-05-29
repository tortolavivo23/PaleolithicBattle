using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTurnState : ILevelState
{
    private LevelManager levelManager;
    private BTNode behaviourTree;
    private CaptureNode captureNode;
    private AttackNode attackNode;

    private MoveNode moveNode;


    public EnemyTurnState(LevelManager levelManager)
    {
        this.levelManager = levelManager;
        captureNode = new CaptureNode(levelManager);
        attackNode = new AttackNode(levelManager);
        moveNode = new MoveNode(levelManager);
        behaviourTree = new Selector(
            captureNode,
            new Selector(
                new Sequence(
                    moveNode,
                    new Selector(
                        captureNode,
                        attackNode
                    )
                ),
                attackNode
            )
        );
    }

    public void EnterState()
    {
        Debug.Log("Enemy Turn State Entered");
        captureNode.Reset();
        attackNode.Reset();
        moveNode.Reset();
        levelManager.StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        foreach (var unit in levelManager.enemyUnits)
        {
            yield return ExecuteBehaviourTree(unit);
        }
        foreach (var camp in levelManager.enemyCamps)
        {
            if (!camp.isOccupied)
                TrainEnemyUnit(camp);
        }

        GoToPlayerTurnState();
    }

    private IEnumerator ExecuteBehaviourTree(IUnit unit)
    {
        Debug.Log($"Unidad {unit} ha empezado su turno");
        BTState state = BTState.Running;
        int safetyCounter = 0;

        while (state == BTState.Running)
        {
            safetyCounter++;
            if (safetyCounter > 1000)
            {
                Debug.LogError($"Bucle infinito detectado para unidad {unit}");
                break;
            }
            state = behaviourTree.Tick(unit);
            yield return null;
        }
        Debug.Log($"Unidad {unit} ha terminado su turno con estado: {state}");
    }
        

    private void TrainEnemyUnit(Cell camp)
    {

        List<UnitType> availableUnits = levelManager.unitTypes
            .Where(t => t != UnitType.None && levelManager.CanAffordUnit(t, false))
            .ToList();

        if (availableUnits.Count == 0) return;

        UnitType selected = availableUnits[Random.Range(0, availableUnits.Count)];
        levelManager.CreateUnit(selected, camp, false);
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
    }

    public void GoToAttackState()
    {
        throw new System.NotImplementedException();
    }
}