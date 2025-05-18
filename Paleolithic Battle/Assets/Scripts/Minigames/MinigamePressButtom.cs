
using UnityEngine;


public class MinigamePressButtom : MonoBehaviour
{

    LevelManager levelManager;

    public float timeLimit = 5f; // Tiempo límite para completar el minijuego

    float timeRemaining; // Tiempo restante para completar el minijuego´

    public TimeBar timeBar; // Referencia al script TimeBar

    void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
        timeRemaining = timeLimit; // Inicializar el tiempo restante
        timeBar.SetMaxTime(timeLimit); // Establecer el tiempo máximo en la barra de tiempo
    }


    public void Win()
    {
        levelManager.minigameResult = 1; 
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime; // Reducir el tiempo restante
        timeBar.SetTime(timeRemaining); // Actualizar la barra de tiempo
        if (timeRemaining <= 0)
        {
            levelManager.minigameResult = -1; 
        }
    }


}
