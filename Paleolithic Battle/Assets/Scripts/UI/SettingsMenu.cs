using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    public GameObject mainMenu;
    public TMP_Dropdown qualityDropdown;

    public Toggle toggleFullscreen;

    public Slider volumeSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        toggleFullscreen.isOn = Screen.fullScreen;
        volumeSlider.value = AudioManager.Instance.GetMasterVolume();
        musicSlider.value = AudioManager.Instance.GetMusicVolume();
        sfxSlider.value = AudioManager.Instance.GetSFXVolume();
    }
    public void SetVolume(float volume)
    {
        AudioManager.Instance.SetMasterVolume(volume);
    }

    public void SetMusic(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }

    public void SetSFX(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void ResetGame()
    {
        AudioManager.Instance.Play("Click");
        GameManager.Instance.ResetGame();
    }

    public void ApplySettings()
    {
        AudioManager.Instance.Play("Click");
        GameManager.Instance.SaveOptions();
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void DiscardSettings()
    {
        AudioManager.Instance.Play("Click");
        GameManager.Instance.SetOptions();
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

}
