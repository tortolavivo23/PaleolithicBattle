
using TMPro;
using UnityEngine;


public class MinigamePressButtom : MonoBehaviour
{

    LevelManager levelManager;

    public int pressesToWin = 8;

    public GameObject instructionsText; // Texto de instrucciones

    public float timeLimit = 3f; // Tiempo límite para completar el minijuego

    float timeRemaining; // Tiempo restante para completar el minijuego´

    public TimeBar timeBar; // Referencia al script TimeBar

    private bool gameStarted = false; // Variable para controlar si el juego ha comenzado

    public float timeInstructions = 1f; // Tiempo para mostrar las instrucciones

    void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
        timeRemaining = timeLimit; // Inicializar el tiempo restante
        timeBar.SetMaxTime(timeLimit); // Establecer el tiempo máximo en la barra de tiempo
        instructionsText.SetActive(true); // Mostrar el texto de instrucciones
        Invoke("HideInstructions", timeInstructions); // Ocultar el texto de instrucciones después de 1 segundo
    }

    void HideInstructions()
    {
        instructionsText.SetActive(false); // Ocultar el texto de instrucciones
        gameStarted = true; // Cambiar el estado del juego a iniciado
    }


    public void Win()
    {
        Debug.Log("Win");
        levelManager.minigameResult = 1;
    }

    public void PressButton()
    {
        AudioManager.Instance.Play("Click");
        if (gameStarted) pressesToWin--;
        if (pressesToWin <= 0)
        {
            Win();
        }
    }

    void Update()
    {
        if (gameStarted)
        {
            timeRemaining -= Time.deltaTime; // Reducir el tiempo restante
            timeBar.SetTime(timeRemaining); // Actualizar la barra de tiempo
        }
        
        if (timeRemaining <= 0)
        {
            Debug.Log("Lose");
            levelManager.minigameResult = -1; 
        }
    }


}
