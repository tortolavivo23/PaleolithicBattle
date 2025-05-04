using UnityEngine;

public class PlayerTurnState : ILevelState
{
    private LevelManager levelManager;

    public PlayerTurnState(LevelManager levelManager)
    {
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
        levelManager.currentState = levelManager.menuState;
    }

    public void GoToPlayerTurnState()
    {
        throw new System.NotImplementedException();
    }

    public void GoToPreviewMoveState()
    {
        levelManager.currentState = levelManager.previewMoveState;
    }

    public void GoToTrainState()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState()
    {
        if (Input.GetMouseButtonDown(0)) // Botón izquierdo del ratón
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider != null)
            {
                Cell clickedCell = hit.collider.GetComponent<Cell>();
                if (clickedCell != null && !(clickedCell.isOccupied && !clickedCell.unit.playerUnit) && (clickedCell.isOccupied || clickedCell.cellType == CellType.Camp && clickedCell.player))
                {
                    // Aquí puedes manejar la lógica de selección de celda
                    levelManager.menuState = new MenuState(levelManager, clickedCell); // Cambiamos al estado de menú
                    GoToMenuState();
                }
            }
        }
    }
}