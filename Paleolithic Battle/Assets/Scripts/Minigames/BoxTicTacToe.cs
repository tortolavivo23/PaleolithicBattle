using UnityEngine;

public class BoxTicTacToe : MonoBehaviour
{
    public GameObject spriteX;
    public GameObject spriteO;

    public bool win = false;

    public void SetX()
    {
        spriteX.SetActive(true);
        spriteO.SetActive(false);
    }
    public void SetO()
    {
        spriteX.SetActive(false);
        spriteO.SetActive(true);
    }
    public void SetEmpty()
    {
        spriteX.SetActive(false);
        spriteO.SetActive(false);
    }


    
}
    