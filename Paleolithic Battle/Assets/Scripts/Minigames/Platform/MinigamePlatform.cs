using UnityEngine;

public class MinigamePlatform : MonoBehaviour
{
    LevelManager levelManager;
    public GameObject instructionsText; // Texto de instrucciones
    public float timeInstructions = 1f; // Tiempo para mostrar las instrucciones
    private bool gameStarted = false; // Variable para controlar si el juego ha comenzado

    public TimeBar timeBar; // Referencia al script TimeBar
    public float timeLimit = 3f; // Tiempo límite para completar el minijuego
    private float timeRemaining; // Tiempo restante para completar el minijuego

    public EnemyPatrol enemyPatrol; // Referencia al script EnemyPatrol
    public PlayerMovement playerMovement; // Referencia al script PlayerMovement

    private void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
        instructionsText.SetActive(true); // Mostrar el texto de instrucciones
        timeRemaining = timeLimit; // Inicializar el tiempo restante
        timeBar.SetMaxTime(timeLimit); // Configurar la barra de tiempo con el tiempo inicial
        enemyPatrol.enabled = false; // Desactivar el movimiento del enemigo al inicio
        playerMovement.enabled = false; // Desactivar el movimiento del jugador al inicio
        Invoke("HideInstructions", timeInstructions); // Ocultar el texto de instrucciones después de 1 segundo
    }

    private void HideInstructions()
    {
        instructionsText.SetActive(false); // Ocultar el texto de instrucciones
        gameStarted = true; // Cambiar el estado del juego a iniciado
        enemyPatrol.enabled = true; // Activar el movimiento del enemigo
        playerMovement.enabled = true; // Activar el movimiento del jugador
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            timeRemaining -= Time.deltaTime; // Reducir el tiempo restante
            timeBar.SetTime(timeRemaining); // Actualizar la barra de tiempo
        }

        if (timeRemaining <= 0)
        {
            Lose(); // Llamar al método Lose si el tiempo se agota
        }

    }

    public void Win()
    {
        levelManager.minigameResult = 1; // Indicar que se ha ganado el minijuego
    }

    public void Lose()
    {
        levelManager.minigameResult = -1; // Indicar que se ha perdido el minijuego
    }
}
