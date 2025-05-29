using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameFPS : MonoBehaviour
{
    LevelManager levelManager;
    public TimeBar timeBar;

    public GameObject textInstructions;
    public float timeLimit = 5f;
    public float timeInstructions = 0.5f; // Tiempo para mostrar las instrucciones
    public Camera mainCamera;
    public Transform BottomLeft;
    public Transform TopRight;

    public GameObject targetPrefab;

    public int numberOfTargets = 5;

    private GameObject target;

    bool gameStarted = false; // Variable para controlar si el juego ha comenzado


    void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
        timeBar.SetMaxTime(timeLimit);
        mainCamera.orthographic = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Invoke("HideInstructions", timeInstructions);
    }

    void CreateTarget()
    {
        //Create a target at a random position within the bounds
        target = Instantiate(targetPrefab, new Vector3(Random.Range(BottomLeft.position.x, TopRight.position.x), Random.Range(BottomLeft.position.y, TopRight.position.y), 0), Quaternion.identity);
        // Random size between 0.5 and 1.5
        float randomSize = Random.Range(0.5f, 1.5f);
        target.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
    }

    void HideInstructions()
    {
        textInstructions.SetActive(false);
        gameStarted = true; // Cambiar el estado del juego a iniciado
        CreateTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted) return;
        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.Instance.Play("Shoot");
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Sphere"))
                {
                    AudioManager.Instance.Play("BreakSphere");
                    Destroy(hit.collider.gameObject);
                    numberOfTargets--;

                    if (numberOfTargets <= 0)
                    {
                        Win();
                    }
                    else
                    {
                        CreateTarget();
                    }
                }
            }
        }

        timeLimit -= Time.deltaTime;
        timeBar.SetTime(timeLimit);
        if (timeLimit <= 0)
        {
            Lose();
        }
    }

    void EndGame()
    {
        if (target != null)
        {
            Destroy(target);
        }   
        mainCamera.orthographic = true;
        Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
    }

    void Win()
    {
        Debug.Log("You win!");
        EndGame();
        levelManager.minigameResult = 1;
    }

    void Lose()
    {
        Debug.Log("You lose!");
        EndGame();
        levelManager.minigameResult = -1;
    }
}
