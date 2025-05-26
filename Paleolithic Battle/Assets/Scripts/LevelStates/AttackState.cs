using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackState : ILevelState
{
    private LevelManager levelManager;

    private IUnit selectedUnit; // La unidad seleccionada por el jugador
    public IUnit targetUnit; // La unidad objetivo seleccionada por el jugadorç
    private string minigameName; // El nombre del minijuego seleccionado
    public AttackState(LevelManager levelManager)
    {
        this.levelManager = levelManager;
        
    }

    public void EnterState()
    {
        selectedUnit = levelManager.selectedCell.unit; // Obtener la unidad seleccionada
        levelManager.minigameResult = 0;
        levelManager.moneyText.enabled = false; // Desactivar el texto de dinero
        minigameName = levelManager.minigames.RandomMinigame(); // Obtener un minijuego aleatorio
        SceneManager.LoadScene(minigameName, LoadSceneMode.Additive); // Cargar la escena de ataque
    }

    public void ExitState()
    {
        levelManager.moneyText.enabled = true; // Activar el texto de dinero
        SceneManager.UnloadSceneAsync(minigameName); // Descargar la escena del minijuego
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
        if(Input.GetKeyDown(KeyCode.Escape)) // Si el jugador presiona Escape
        {
            levelManager.ChangeState(levelManager.playerTurnState); // Cambiar al estado de turno del jugador
            SceneManager.UnloadSceneAsync(minigameName); // Descargar la escena del minijuego
        }

        if(levelManager.minigameResult > 0)
        {
            Debug.Log("Ganaste el minijuego");
            levelManager.AttackUnit(selectedUnit, targetUnit, 1.5f);
            GoToPlayerTurnState();
        }
        else if(levelManager.minigameResult < 0)
        {
            Debug.Log("Perdiste el minijuego");
            levelManager.AttackUnit(selectedUnit, targetUnit, 0.5f); 
            GoToPlayerTurnState();
        }
    }

    public void GoToAttackState()
    {
        throw new System.NotImplementedException();
    }
}