using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject mainMenu;
    public void ChangeScene(string sceneName)
    {
        AudioManager.Instance.Play("Click");
        SceneManager.LoadScene(sceneName);
    }

    public void OpenSettings()
    {
        AudioManager.Instance.Play("Click");
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
        settingsMenu.GetComponent<SettingsMenu>().Init();
    }

    public void QuitGame()
    {
        AudioManager.Instance.Play("Click");
        Debug.Log("Quit Game");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop playing in the editor
#else
        Application.Quit();
#endif
    }
}
