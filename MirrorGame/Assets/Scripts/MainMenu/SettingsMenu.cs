﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer AudioMixerMusic;
    public AudioMixer AudioMixerSFX;
    private Resolution[] _resolutions;
    public Dropdown resolutionDropDown;

    void Start()
    {
        _resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);
            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }


        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject); //this is to keep settings of the game for the next scene
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = _resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetMusicVolume(float volume)
    {
        //gets current volume
        AudioMixerMusic.SetFloat("MainVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        //gets current volume
        AudioMixerSFX.SetFloat("SFXVolume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}