using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [Header("AudioMixer")]
    public AudioMixer MasterMixer;
    public AudioMixer MusicMixer;
    public AudioMixer SFXMixer;

    [Header("Resolution")]
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    void Start()
    {
        getScreenResolutions();
    }

    public void SetMasterVolume(float volume)
    {
        MasterMixer.SetFloat("MasterVolume", changeToDb(volume));
    }

    public void SetMusicVolume(float volume)
    {
        MusicMixer.SetFloat("MusicVolume", changeToDb(volume));
    }

    public void SetSFXVolume(float volume)
    {
        SFXMixer.SetFloat("SfxVolume", changeToDb(volume));
    }

    private float changeToDb(float volume)
    {
        return (((volume - 0) * (20 - -80)) / (10 - 0)) + -80;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void getScreenResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height+" @ "+resolutions[i].refreshRate+"Hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
}
