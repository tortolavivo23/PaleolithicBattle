using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    private LevelManager levelManager;

    void Start()
    {
        // Se obtiene el LevelManager para comprobar el resultado del minijuego
        levelManager = FindFirstObjectByType<LevelManager>();
        if (levelManager == null)
        {
            Debug.LogError("LevelManager not found in the scene.");
        }

        // Se asegura de que el menú de pausa esté oculto al iniciar
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        // Si se pulsa escape se activa el menú de pausa
        if (Input.GetKeyDown(KeyCode.Escape) && !levelManager.playingMinigame)
        {
            if (GameIsPaused)
            {
                Resume(false);
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume(bool playSound = true)
    {
        if (playSound)
        {
            AudioManager.Instance.Play("Click");
        }
       // Se desactiva el menú de pausa y se reanuda el tiempo
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        // Se activa el menú de pausa y se pausa el tiempo
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void QuitGame()
    {
        AudioManager.Instance.Play("Click");
        Debug.Log("Quitting game...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        
        Application.Quit();
#endif
    }

    public void RestartGame()
    {
        AudioManager.Instance.Play("Click");
        // Se reinicia el nivel
        Debug.Log("Restarting game...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        AudioManager.Instance.StopAll();
        AudioManager.Instance.Play("Click");
        // Se carga el menú principal
        Debug.Log("Loading menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
