using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSettings : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle fullScreenToggle;
    public TMP_Dropdown resolutionDropdown;

    private void Start()
    {
        LoadSettings();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SfxVolume", sfxSlider.value);
        PlayerPrefs.SetInt("FullScreen", fullScreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
    }

    public void LoadSettings()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 8f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 8f);
        sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", 8f);
        fullScreenToggle.isOn = PlayerPrefs.GetInt("FullScreen", 1) == 1 ? true : false;
        if (fullScreenToggle.isOn) Screen.fullScreen = true; else Screen.fullScreen = false;
        resolutionDropdown.value = PlayerPrefs.GetInt("Resolution", resolutionDropdown.value);
    }
}
