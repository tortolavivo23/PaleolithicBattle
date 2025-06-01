using UnityEngine;

public class StarUI : MonoBehaviour
{
    public GameObject starEnabled;
    public GameObject starDisabled;

    void Awake()
    {
        starDisabled.SetActive(true);
        starEnabled.SetActive(false);
    }

    public void EnableStar()
    {
        starDisabled.SetActive(false);
        starEnabled.SetActive(true);
    }
}
