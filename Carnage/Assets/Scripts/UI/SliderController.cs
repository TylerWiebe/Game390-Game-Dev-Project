using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField]
    private MasterAudioManager AudioMngr;

    [SerializeField] 
    private Slider mySlider;
    
    public void setAudioVolume(String group)
    {
        switch (group)
        {
            case "Master":
                AudioMngr.SetMasterVolume(mySlider.value);
                break;
            case "SFX":
                AudioMngr.SetSFXVolume(mySlider.value);
                break;
            case "Music":
                AudioMngr.SetMusicVolume(mySlider.value);
                break;
            case "Ambience":
                AudioMngr.SetAmbienceVolume(mySlider.value);
                break;
        }
    }
}
