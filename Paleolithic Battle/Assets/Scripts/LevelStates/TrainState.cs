using TMPro;
using UnityEngine;

public class TrainState : ILevelState
{
    private LevelManager levelManager;
    private GameObject menuUI; // UI del menú de entrenamiento

    private GameObject buttonPrefab; // Prefab del botón de entrenamiento

    private int buttonCount = 0; // Contador de botones creados

    public TrainState(LevelManager levelManager)
    {
        this.levelManager = levelManager;
    }

    public void EnterState()
    {
        buttonCount = 0;
        menuUI = levelManager.menu; // Obtenemos la UI del menú de entrenamiento
        foreach (Transform child in menuUI.transform) // Limpiar el menú de botones anteriores
        {
            Object.Destroy(child.gameObject);
        }
        menuUI.SetActive(true); // Activamos el menú de entrenamiento
        buttonPrefab = levelManager.buttonPrefab; // Obtenemos el prefab del botón de entrenamiento
        foreach (UnitType unitType in levelManager.unitTypes)
        {
            if (unitType == UnitType.None || !levelManager.CanAffordUnit(unitType, true)) continue;
            CreateButton(
                GetNameUnitType(unitType) + " " + levelManager.GetUnitCost(unitType), () => levelManager.CreateUnit(unitType, levelManager.selectedCell, true)
                ); // Creamos un botón para cada tipo de unidad
        }
        CreateButton("Close", () => GoToPlayerTurnState());
    }

    public void ExitState()
    {
        foreach (Transform child in menuUI.transform) // Limpiar el menú de botones anteriores
        {
            Object.Destroy(child.gameObject);
        }
        menuUI.SetActive(false); // Ocultar el menú
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
        levelManager.ChangeState(levelManager.playerTurnState); // Cambiamos al estado de turno del jugador
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

    private string GetNameUnitType(UnitType unitType)
    {
        switch (unitType)
        {
            case UnitType.Base:
                return "Base";
            case UnitType.Range:
                return "Range";
            case UnitType.Heavy:
                return "Heavy";
            case UnitType.Water:
                return "Water";
            default:
                return "Unknown";
        }
    }

    private void CreateButton(string buttonText, System.Action onClickAction)
    {
        buttonCount++;
        float height = menuUI.GetComponent<RectTransform>().rect.height; // Obtener la altura del menú
        GameObject button = Object.Instantiate(buttonPrefab, menuUI.transform.position - new Vector3(0, -height / 2 + buttonCount * 50, 0), Quaternion.identity); // Crear el botón en la posición del menú
        button.transform.SetParent(menuUI.transform); // Establecer el padre del botón como el menú
        button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText; // Cambiar el texto del botón
        button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => onClickAction.Invoke()); // Asignar la acción al botón
        button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => menuUI.SetActive(false)); // Cerrar el menú al hacer clic en el botón
        button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => GoToPlayerTurnState());
    }

    public void GoToAttackState()
    {
        throw new System.NotImplementedException();
    }
}
