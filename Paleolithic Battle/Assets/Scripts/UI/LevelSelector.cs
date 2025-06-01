using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public GameObject button;
    public GameObject blockedImage;
    public StarUI[] stars = new StarUI[3];

    public int levelNumber;
    public string sceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManager instance is not found.");
            return;
        }
        button.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + (levelNumber+1);
        button.GetComponent<Button>().onClick.AddListener(() =>
        {
            gameManager.SetLastLevel(levelNumber);
            AudioManager.Instance.Stop("MainMenuMusic");
            AudioManager.Instance.Play("Click");
            SceneManager.LoadScene(sceneName);
        }
        );
        button.GetComponent<Button>().interactable = gameManager.unlockedLevels[levelNumber];
        blockedImage.SetActive(!gameManager.unlockedLevels[levelNumber]);
        for (int i=0; i < gameManager.GetScore(levelNumber); i++)
        {
            if (i < stars.Length)
            {
                stars[i].EnableStar();
            }
            else
            {
                Debug.LogWarning("More stars than available StarUI components.");
            }
        }
    }
}
