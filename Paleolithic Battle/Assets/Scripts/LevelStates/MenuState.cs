using System.Collections;
using TMPro;
using UnityEngine;

public class MenuState : ILevelState
{
    private LevelManager levelManager;
    private GameObject menuUI;
    private GameObject buttonPrefab;
    private Cell selectedCell; // Celda seleccionada por el jugador

    private int buttonCount = 0; // Contador de botones creados



    public MenuState(LevelManager levelManager, Cell selectedCell)
    {
        this.levelManager = levelManager;
        menuUI = levelManager.menu;
        foreach (Transform child in menuUI.transform) // Limpiar el menú de botones anteriores
        {
            Object.Destroy(child.gameObject);
        }
        menuUI.SetActive(true);
        buttonPrefab = levelManager.buttonPrefab;
        this.selectedCell = selectedCell; // Guardamos la celda seleccionada

        if(selectedCell.isOccupied) // Si la celda está ocupada, mostramos las opciones de ataque
        {
            CreateButton("Attack", () => GoToAttackState());
            CreateButton("Move", () => GoToPreviewMoveState()); // Opción de mover a la celda seleccionada
            if(selectedCell.capturable && !selectedCell.player){
                CreateButton("Capture", () => CaptureCell()); // Opción de capturar la celda
            }
        }
        else if (selectedCell.cellType == CellType.Camp && selectedCell.player)
        {
            CreateButton("Train", () => GoToTrainState());
        }
        CreateButton("Close", () => GoToPlayerTurnState()); // Opción de cerrar el menú y volver al turno del jugador
    }

    public void GoToTrainState()
    {
        levelManager.trainState = new TrainState(levelManager, selectedCell); // Cambiamos al estado de entrenamiento
        levelManager.currentState = levelManager.trainState; // Cambiamos el estado actual a TrainState
    }
    public void GoToAttackState()
    {
        levelManager.attackState = new AttackState(levelManager, selectedCell); // Cambiamos al estado de ataque
        levelManager.currentState = levelManager.attackState; // Cambiamos el estado actual a AttackState
        menuUI.SetActive(false); // Cerrar el menú
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
        levelManager.currentState = levelManager.playerTurnState;
        menuUI.SetActive(false); // Cerrar el menú
    }

    public void GoToPreviewMoveState()
    {
        levelManager.previewMoveState = new PreviewMoveState(levelManager, selectedCell); // Cambiamos al estado de previsualización de movimiento
        levelManager.currentState = levelManager.previewMoveState; // Cambiamos al estado de previsualización de movimiento
        menuUI.SetActive(false); // Cerrar el menú
    }

    public void UpdateState()
    {

    }

    private void CaptureCell()
    {
        levelManager.CaptureCell(selectedCell, true); // Capturar la celda seleccionada
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
}
