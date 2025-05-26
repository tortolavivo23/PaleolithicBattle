using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuState : ILevelState
{
    private LevelManager levelManager;
    private GameObject menuUI;
    private GameObject buttonPrefab;
    private Cell selectedCell; // Celda seleccionada por el jugador

    private int buttonCount = 0; // Contador de botones creados
    
    public Dictionary<Cell, Cell> availableCells = new Dictionary<Cell, Cell>(); // Diccionario de celdas disponibles para moverse
    public List<Cell> attackCells = new List<Cell>(); // Lista de celdas disponibles para atacar


    public MenuState(LevelManager levelManager)
    {
        this.levelManager = levelManager;
        menuUI = levelManager.menu;
        buttonPrefab = levelManager.buttonPrefab;

    }

    public void EnterState()
    {
        buttonCount = 0; // Reiniciar el contador de botones
        foreach (Transform child in menuUI.transform) // Limpiar el menú de botones anteriores
        {
            Object.Destroy(child.gameObject);
        }
        menuUI.SetActive(true);
        
        selectedCell = levelManager.selectedCell; // Guardamos la celda seleccionada

        if(selectedCell.isOccupied) // Si la celda está ocupada, mostramos las opciones de ataque
        {
            IUnit unit = selectedCell.unit; // Obtenemos la unidad de la celda seleccionada
            if(unit.lastActionTurn < levelManager.currentTurn) // Si la unidad no ha actuado en este turno
            {
                unit.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.green;
                availableCells = levelManager.GetAvailableMoveCells(selectedCell); // Obtenemos las celdas disponibles para moverse
                attackCells = levelManager.GetAvailableAttackCells(selectedCell); // Obtenemos las celdas disponibles para atacar
                if(attackCells.Count > 0) CreateButton("Attack", () => GoToPreviewAttackState()); // Opción de atacar
                if(selectedCell.capturable && unit.unitType==UnitType.Base && !selectedCell.player)
                    CreateButton("Capture", () => CaptureCell()); // Opción de capturar la celda
                if (unit.lastMoveTurn < levelManager.currentTurn && availableCells.Count > 0)
                    CreateButton("Move", () => GoToPreviewMoveState()); // Opción de mover a la celda seleccionada
            }
        }
        else if (selectedCell.cellType == CellType.Camp && selectedCell.player)
        {
            CreateButton("Train", () => GoToTrainState());
        }
        if (buttonCount == 0) GoToPlayerTurnState(); // Si no hay botones, volver al turno del jugador
        else CreateButton("Close", () => GoToPlayerTurnState()); // Opción de cerrar el menú y volver al turno del jugador
    }

    public void ExitState()
    {
        foreach (Transform child in menuUI.transform) // Limpiar el menú de botones anteriores
        {
            Object.Destroy(child.gameObject);
        }
        if (selectedCell.isOccupied) // Si la celda está ocupada, restaurar el color de la unidad
        {
            IUnit unit = selectedCell.unit; // Obtenemos la unidad de la celda seleccionada
            unit.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
        menuUI.SetActive(false); // Ocultar el menú
    }

    public void GoToTrainState()
    {
        levelManager.ChangeState(levelManager.trainState); // Cambiamos al estado de entrenamiento
    }
    public void GoToPreviewAttackState()
    {
        levelManager.ChangeState(levelManager.previewAttackState); // Cambiamos el estado actual a AttackState

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
        levelManager.ChangeState(levelManager.playerTurnState); // Cambiamos el estado actual a PlayerTurnState
    }

    public void GoToPreviewMoveState()
    {
        levelManager.ChangeState(levelManager.previewMoveState); // Cambiamos el estado actual a PreviewMoveState
    }

    public void UpdateState()
    {

    }

    private void CaptureCell()
    {
        levelManager.CaptureCell(selectedCell, true); // Capturar la celda seleccionada
        selectedCell.unit.lastActionTurn = levelManager.currentTurn; // Actualizar el turno de la unidad
        GoToPlayerTurnState(); // Volver al estado de turno del jugador
    }


    private void CreateButton(string buttonText, System.Action onClickAction)
    {
        buttonCount++;
        float height = menuUI.GetComponent<RectTransform>().rect.height; // Obtener la altura del menú
        GameObject button = Object.Instantiate(buttonPrefab, menuUI.transform.position - new Vector3(0, -height/2 + buttonCount*50, 0), Quaternion.identity); // Crear el botón en la posición del menú
        button.transform.SetParent(menuUI.transform); // Establecer el padre del botón como el menú
        button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText; // Cambiar el texto del botón
        button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => menuUI.SetActive(false)); // Cerrar el menú al hacer clic en el botón
        button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => onClickAction.Invoke()); // Asignar la acción al botón
    }

    public void GoToAttackState()
    {
        throw new System.NotImplementedException();
    }
}
