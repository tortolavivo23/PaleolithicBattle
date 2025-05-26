using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackState : ILevelState
{
    private LevelManager levelManager;
    private GameObject menuUI;
    private GameObject buttonPrefab;

    private IUnit selectedUnit; // La unidad seleccionada por el jugador
    public IUnit targetUnit; // La unidad objetivo seleccionada por el jugadorç
    private string minigameName; // El nombre del minijuego seleccionado

    int buttonCount = 0; // Contador de botones creados

    bool minigame = false; // Indica si se ha seleccionado un minijuego
    public AttackState(LevelManager levelManager)
    {
        this.levelManager = levelManager;
        menuUI = levelManager.menu;
        buttonPrefab = levelManager.buttonPrefab;
    }

    public void EnterState()
    {
        minigame = true;
        buttonCount = 0; // Reiniciar el contador de botones
        menuUI.SetActive(true); // Activar el menú de ataque
        selectedUnit = levelManager.selectedCell.unit; // Obtener la unidad seleccionada
        levelManager.minigameResult = 0;
        CreateButton("Play Minigame", () => PlayMinigame()); // Botón para iniciar el minijuego
        CreateButton("Don't play minigame", () => DontPlay()); // Botón para atacar sin minijuego
    }

    void DontPlay()
    {
        levelManager.AttackUnit(selectedUnit, targetUnit, 1.0f);
        GoToPlayerTurnState();
    }

    void PlayMinigame()
    {
        levelManager.moneyText.enabled = false;
        minigame = true; // Indicar que se ha jugado un minijuego
        minigameName = levelManager.minigames.RandomMinigame(); // Obtener un minijuego aleatorio
        SceneManager.LoadScene(minigameName, LoadSceneMode.Additive); // Cargar la escena de ataque
    }

    public void ExitState()
    {
        levelManager.moneyText.enabled = true; // Activar el texto de dinero
       
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
        levelManager.ChangeState(levelManager.playerTurnState); // Cambiar al estado de turno del jugador
    }

    public void GoToPreviewAttackState()
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
        if (Input.GetKeyDown(KeyCode.Escape)) // Si el jugador presiona Escape
        {
            levelManager.ChangeState(levelManager.playerTurnState); // Cambiar al estado de turno del jugador
            SceneManager.UnloadSceneAsync(minigameName); // Descargar la escena del minijuego
        }

        if (minigame && levelManager.minigameResult > 0)
        {
            Debug.Log("Ganaste el minijuego");
            levelManager.AttackUnit(selectedUnit, targetUnit, 1.5f);
            GoToPlayerTurnState();
        }
        else if (minigame && levelManager.minigameResult < 0)
        {
            Debug.Log("Perdiste el minijuego");
            levelManager.AttackUnit(selectedUnit, targetUnit, 0.5f);
            GoToPlayerTurnState();
        }

        if (minigame && levelManager.minigameResult != 0)
        {
             SceneManager.UnloadSceneAsync(minigameName); // Descargar la escena del minijuego
        }
    }

    public void GoToAttackState()
    {
        throw new System.NotImplementedException();
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