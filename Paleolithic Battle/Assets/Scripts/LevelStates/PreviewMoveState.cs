using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PreviewMoveState : ILevelState
{
    private LevelManager levelManager;

    private Cell selectedCell; // La celda seleccionada por el jugador
    private List<Cell> availableCells; // Las celdas disponibles para moverse
    private IUnit selectedUnit; // La unidad seleccionada por el jugador


    public PreviewMoveState(LevelManager levelManager, Cell selectedCell)
    {
        this.levelManager = levelManager;
        this.selectedCell = selectedCell;
        Debug.Log("PreviewMoveState: " + selectedCell.name);
        selectedUnit = selectedCell.unit; // Obtener la unidad de la celda seleccionada
        availableCells = GetAvailableCells(selectedCell, selectedUnit); // Obtener las celdas disponibles para moverse

        foreach (var cell in availableCells)
        {
            cell.GetComponent<SpriteRenderer>().color = Color.green; // Cambiar el color de las celdas disponibles a verde
        }
        selectedCell.GetComponent<SpriteRenderer>().color = Color.red; // Cambiar el color de la celda seleccionada a rojo


    }

    private List<Cell> GetAvailableCells(Cell selectedCell, IUnit selectedUnit)
    {
        List<Cell> availableCells = new List<Cell>();
        bool[,] visited = new bool[levelManager.map.height, levelManager.map.width];
        Queue<Cell> queue = new Queue<Cell>();
        queue.Enqueue(selectedCell);
        queue.Enqueue(null);
        int range = selectedUnit.movementRange; // Obtener el rango de movimiento de la unidad
        int steps = 0; // Contador de pasos

        while (queue.Count > 0)
        {
            Cell currentCell = queue.Dequeue();
            if (currentCell == null)
            {
                steps++;
                if (steps > range) break; // Si hemos alcanzado el rango de movimiento, salimos del bucle
                queue.Enqueue(null); // Añadimos un marcador para el siguiente nivel
                continue;
            }

            if (visited[currentCell.y, currentCell.x]) continue; // Si ya hemos visitado esta celda, la ignoramos
            visited[currentCell.y, currentCell.x] = true; // Marcamos la celda como visitada

            if (selectedUnit.unitType == UnitType.Heavy && currentCell.cellType == CellType.Mountain) continue; // Si la unidad es pesada y la celda es montaña, la ignoramos
            if(steps!=0) availableCells.Add(currentCell); // Añadimos la celda a las celdas disponibles
            if(currentCell.cellType == CellType.Mountain && steps!=0) continue; // Si la celda es montaña, no añadimos las celdas adyacentes

            // Añadimos las celdas adyacentes a la cola
            foreach (var adjacentCell in levelManager.GetAdjacentCells(currentCell))
            {
                if (!visited[adjacentCell.y, adjacentCell.x] && !adjacentCell.isOccupied &&
                 (selectedUnit.unitType == UnitType.Water && adjacentCell.cellType == CellType.Water || 
                 (selectedUnit.unitType != UnitType.Water && adjacentCell.cellType != CellType.Water))) // Solo añadimos celdas no ocupadas y que no sean agua si la unidad no es de tipo acuática
                {
                    queue.Enqueue(adjacentCell);
                }
            }
        }
        return availableCells;
    }

    public void GoToEnemyTurnState()
    {
        throw new System.NotImplementedException();
    }

    public void GoToPlayerTurnState()
    {
        foreach (var cell in availableCells)
        {
            cell.GetComponent<SpriteRenderer>().color = Color.white; // Restaurar el color original de las celdas disponibles
        }
        selectedCell.GetComponent<SpriteRenderer>().color = Color.white; // Restaurar el color original de la celda seleccionada
        levelManager.currentState = levelManager.playerTurnState; // Cambiamos al estado de turno del jugador
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
                if (clickedCell != null && availableCells.Contains(clickedCell))
                {
                    // Aquí puedes manejar la lógica de movimiento de la unidad
                    levelManager.MoveUnit(selectedUnit, clickedCell); // Mover la unidad a la celda seleccionada
                    selectedUnit.Move(clickedCell.x, clickedCell.y); // Mover la unidad a la celda seleccionada
                }
            }
            GoToPlayerTurnState(); // Volver al estado de turno del jugador
        }
    }

    public void GoToMenuState()
    {
        throw new System.NotImplementedException();
    }

    public void GoToAttackState()
    {
        throw new System.NotImplementedException();
    }

    public void GoToTrainState()
    {
        throw new System.NotImplementedException();
    }
}
