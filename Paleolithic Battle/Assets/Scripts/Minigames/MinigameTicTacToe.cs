using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameTicTacToe : MonoBehaviour
{
    public Camera cam;
    LevelManager levelManager;
    public GameObject textInstructions;
    public float timeInstructions = 0.5f; // Tiempo para mostrar las instrucciones
    public float timeLimit = 3f; // Tiempo límite para completar el minijuego
    public TimeBar timeBar; // Referencia al script TimeBar
    float timeRemaining; // Tiempo restante para completar el minijuego
    public BoxTicTacToe[] boxes = new BoxTicTacToe[9]; // Referencia a los objetos de las casillas

    // 0 = empty, 1 = X, 2 = O, 3 = win
    int[,,] boards = {
        {
            { 2, 0, 3 },
            { 1, 1, 2 },
            { 1, 2, 0 }
        },
        {
            { 3, 0, 1 },
            { 0, 1, 2 },
            { 2, 2, 1 }

        },
        {
            { 0, 0, 1 },
            { 0, 0, 1 },
            { 2, 2, 3 }
        },
        {
            { 2, 2, 1 },
            { 0, 1, 3 },
            { 3, 2, 1 }
        }
    };

    bool gameStarted = false; // Variable para controlar si el juego ha comenzado

    void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
        textInstructions.gameObject.SetActive(true);
        Invoke("HideInstructions", timeInstructions);
        timeBar.SetMaxTime(timeLimit); // Establecer el tiempo máximo en la barra de tiempo
        timeRemaining = timeLimit; // Inicializar el tiempo restante

        
    }

    void HideInstructions()
    {
        textInstructions.gameObject.SetActive(false);
        gameStarted = true; // Cambiar el estado del juego a iniciado
        int randomBoard = Random.Range(0, 4); // Seleccionar un tablero aleatorio
        Debug.Log("Tablero aleatorio: " + randomBoard);
        for (int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                Debug.Log("i: " + i + ", j: " + j);
                Debug.Log("Tablero: " + boards[randomBoard, i, j]);
                Debug.Log("Casilla: " + boxes[i*3+j]);
                boxes[i*3+j].SetEmpty(); // Inicializar todas las casillas como vacías
                if (boards[randomBoard, i, j] == 1)
                {
                    boxes[i*3+j].SetX(); // Establecer la casilla como X
                }
                else if (boards[randomBoard, i, j] == 2)
                {
                    boxes[i*3+j].SetO(); // Establecer la casilla como O
                }
                else if (boards[randomBoard, i, j] == 3)
                {
                    boxes[i*3+j].win = true; // Establecer la casilla como ganadora
                }
                
            }
            
        }
    }

    void Update()
    {
        if (!gameStarted) return; // Si el juego no ha comenzado, no hacer nada
        timeRemaining -= Time.deltaTime; // Reducir el tiempo restante
        timeBar.SetTime(timeRemaining); // Actualizar la barra de tiempo
        if (timeRemaining <= 0)
            Lose();

        if (Input.GetMouseButtonDown(0)) // Botón izquierdo del ratón
        {
            Debug.Log("Mouse clicked");
            Vector2 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Mouse world position: " + mouseWorldPos);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log("Hit object: " + hit.collider.gameObject.name);
                BoxTicTacToe clickedBox = hit.collider.GetComponent<BoxTicTacToe>();
                Debug.Log("Clicked box: " + clickedBox);
                if (clickedBox != null)
                    if (clickedBox.win)
                        Win();
                    else
                        Lose();
            }
        }
    }

    void Win()
    {
        levelManager.minigameResult = 1;
    }

    void Lose()
    {
        levelManager.minigameResult = -1;
    }

}
