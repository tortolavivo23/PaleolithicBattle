using System.Collections.Generic;
using UnityEngine;

public class AttackState : ILevelState
{
    private LevelManager levelManager;
    private Cell cellSelected;

    private List<Cell> availableCells; // Las celdas disponibles para atacar

    public AttackState(LevelManager levelManager, Cell cellSelected)
    {
        this.levelManager = levelManager;
        this.cellSelected = cellSelected;

        availableCells = GetAvailableCells(cellSelected, cellSelected.unit); // Obtener las celdas disponibles para atacar

        foreach (var cell in availableCells)
        {
            cell.GetComponent<SpriteRenderer>().color = Color.red; // Cambiar el color de las celdas disponibles a rojo
        }
    }

    public List<Cell> GetAvailableCells(Cell selectedCell, IUnit selectedUnit)
    {
        List<Cell> availableCells = new List<Cell>();
        bool[,] visited = new bool[levelManager.map.height, levelManager.map.width];
        Queue<Cell> queue = new Queue<Cell>();
        queue.Enqueue(selectedCell);
        queue.Enqueue(null);
        int rangeMin = selectedUnit.minAttackRange; // Obtener el rango de ataque de la unidad
        int rangeMax = selectedUnit.maxAttackRange; // Obtener el rango de ataque de la unidad
        int steps = 0; // Contador de pasos

        while (queue.Count > 0)
        {
            Cell currentCell = queue.Dequeue();
            if (currentCell == null)
            {
                steps++;
                if (steps > rangeMax) break; // Si hemos alcanzado el rango de ataque, salimos del bucle
                queue.Enqueue(null); // Añadimos un marcador para el siguiente nivel
                continue;
            }

            if (visited[currentCell.y, currentCell.x]) continue; // Si ya hemos visitado esta celda, la ignoramos
            visited[currentCell.y, currentCell.x] = true; // Marcamos la celda como visitada

            if(steps>=rangeMin && currentCell.isOccupied && !currentCell.unit.playerUnit) availableCells.Add(currentCell); // Añadimos la celda a las celdas disponibles

            // Añadimos las celdas adyacentes a la cola
            foreach (var adjacentCell in levelManager.GetAdjacentCells(currentCell))
            {
                queue.Enqueue(adjacentCell);
            }
        }
        return availableCells;
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
        levelManager.currentState = levelManager.playerTurnState; // Cambiamos el estado actual a PlayerTurnState
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
                if (clickedCell != null && availableCells.Contains(clickedCell)) // Si la celda está en las celdas disponibles
                {
                    // Aquí puedes manejar la lógica de ataque
                    levelManager.AttackUnit(cellSelected.unit, clickedCell.unit); // Atacar a la unidad en la celda seleccionada
                }
            }
            foreach (var cell in availableCells)
            {
                cell.GetComponent<SpriteRenderer>().color = Color.white; // Restaurar el color original de las celdas disponibles
            }
            GoToPlayerTurnState(); // Volver al estado de turno del jugador
        }
    }
}
