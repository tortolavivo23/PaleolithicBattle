using UnityEngine;

[System.Serializable]
public class OptionsData
{
    [Range(-80, 0)]
    public float masterVolume = 0f;
    [Range(-80, 0)]
    public float musicVolume = 0f;
    [Range(-80, 0)]
    public float sfxVolume = 0f;

    public bool isFullScreen = true;
    public int qualityIndex = 3; // Index of the selected resolution in a predefined list

}