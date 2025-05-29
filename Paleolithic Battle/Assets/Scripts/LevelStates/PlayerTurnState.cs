using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnState : ILevelState
{
    private LevelManager levelManager;

    public PlayerTurnState(LevelManager levelManager)
    {
        this.levelManager = levelManager;
        levelManager.endTurnButton.onClick.AddListener(() => AudioManager.Instance.Play("Click"));
        levelManager.endTurnButton.onClick.AddListener(EndTurn); // Añadir el listener al botón de fin de turno
    }

    public void EndTurn(){
        GoToEnemyTurnState(); // Cambiar al estado de turno del enemigo
    }

    public void GoToPreviewAttackState()
    {
        throw new System.NotImplementedException();
    }

    public void GoToEnemyTurnState()
    {
        Debug.Log("Cambiando al estado de turno del enemigo");
        levelManager.AddMoney(false); // Añadir dinero al enemigo
        levelManager.ChangeState(levelManager.enemyTurnState); // Cambiamos el estado actual a EnemyTurnState
    }

    public void GoToMenuState()
    {
        levelManager.ChangeState(levelManager.menuState); // Cambiamos al estado de menú
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
        if (Input.GetMouseButtonDown(0)) // Botón izquierdo del ratón
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider != null)
            {
                Cell clickedCell = hit.collider.GetComponent<Cell>();
                if (clickedCell != null && (clickedCell.isOccupied && clickedCell.unit.playerUnit || clickedCell.cellType == CellType.Camp && clickedCell.player))
                {
                    // Aquí puedes manejar la lógica de selección de celdas
                    levelManager.selectedCell = clickedCell; // Guardar la celda seleccionada
                    GoToMenuState();
                }
            }
        }
    }

    public void EnterState(){
        Debug.Log("Entrando al estado de turno del jugador");
        levelManager.endTurnButton.gameObject.SetActive(true); // Mostrar el botón de fin de turno
    }

    public void ExitState()
    {
        levelManager.endTurnButton.gameObject.SetActive(false); // Ocultar el botón de fin de turno
    }

    public void GoToAttackState()
    {
        throw new System.NotImplementedException();
    }
}