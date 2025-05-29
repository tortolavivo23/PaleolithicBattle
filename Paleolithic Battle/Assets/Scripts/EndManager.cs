using TMPro;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    public StarUI[] stars = new StarUI[3];
    public TextMeshProUGUI resultText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager gameManager = GameManager.Instance;
        int lastScore = gameManager.GetLastScore();
        Debug.Log("Last score: " + lastScore);
        for (int i = 0; i < lastScore; i++)
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
        if (lastScore > 0)
        {
            resultText.text = "You win!";
            resultText.color = Color.green;
        }
        else
        {
            resultText.text = "You lose!";
            resultText.color = Color.red;
        }
    }
}
