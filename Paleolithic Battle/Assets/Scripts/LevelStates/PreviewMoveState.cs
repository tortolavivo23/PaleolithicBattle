using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PreviewMoveState : ILevelState
{
    private LevelManager levelManager;

    private Cell selectedCell; // La celda seleccionada por el jugador
    private Dictionary<Cell, Cell> availableCells; // Las celdas disponibles para moverse
    private IUnit selectedUnit; // La unidad seleccionada por el jugador


    public PreviewMoveState(LevelManager levelManager)
    {
        this.levelManager = levelManager;
        
    }

    public void EnterState()
    {
        selectedCell = levelManager.selectedCell; // Guardamos la celda seleccionada
        selectedUnit = selectedCell.unit; // Obtener la unidad de la celda seleccionada
        availableCells = levelManager.menuState.availableCells; // Obtener las celdas disponibles para moverse

        foreach (var cell in availableCells.Keys)
        {
            if(!cell.isOccupied) cell.GetComponent<SpriteRenderer>().color = Color.green; // Cambiar el color de las celdas disponibles a verde
        }
        selectedCell.GetComponent<SpriteRenderer>().color = Color.red; // Cambiar el color de la celda seleccionada a rojo
    }

    public void ExitState()
    {
        foreach (var cell in availableCells.Keys)
        {
            cell.GetComponent<SpriteRenderer>().color = Color.white; // Restaurar el color original de las celdas disponibles
        }
        selectedCell.GetComponent<SpriteRenderer>().color = Color.white; // Restaurar el color original de la celda seleccionada
    }

    

    public void GoToEnemyTurnState()
    {
        throw new System.NotImplementedException();
    }

    public void GoToPlayerTurnState()
    {
        levelManager.ChangeState(levelManager.playerTurnState); // Cambiamos el estado actual a PlayerTurnState
    }

    public void GoToPreviewMoveState()
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
                if (clickedCell != null && availableCells.ContainsKey(clickedCell) && !clickedCell.isOccupied)
                {
                    // Aquí puedes manejar la lógica de movimiento de la unidad
                    levelManager.MoveUnit(selectedUnit, clickedCell, availableCells); // Mover la unidad a la celda seleccionada
                    selectedUnit.Move(clickedCell.x, clickedCell.y); // Mover la unidad a la celda seleccionada
                    selectedUnit.lastMoveTurn = levelManager.currentTurn; // Actualizar el turno del ultimo movimiento
                }
            }
            GoToPlayerTurnState(); // Volver al estado de turno del jugador
        }
    }

    public void GoToMenuState()
    {
        throw new System.NotImplementedException();
    }

    public void GoToPreviewAttackState()
    {
        throw new System.NotImplementedException();
    }

    public void GoToTrainState()
    {
        throw new System.NotImplementedException();
    }

    public void GoToAttackState()
    {
        throw new System.NotImplementedException();
    }
}
