using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

//using UnityEngine.UI;

public class MasterAudioManager : MonoBehaviour
{
    public AudioMixer MasterMixer;
    public MultiplayerGameManager GameManager;

    [SerializeField]
    public List<Slider> volumeSliders;

    public void Start()
    {
        loadAudioSettings();
    }

    public void FixedUpdate()
    {
        if (GameManager != null)
            GetAllEngineAccelerations();
    }



    private void loadAudioSettings()
    {
        volumeSliders[0].value = PlayerPrefs.GetFloat("MasterVolume", 1);
        volumeSliders[1].value = PlayerPrefs.GetFloat("SFXVolume", 1);
        volumeSliders[2].value = PlayerPrefs.GetFloat("MusicVolume",1);
        volumeSliders[3].value = PlayerPrefs.GetFloat("AmbienceVolume",1);
        SetMasterVolume(volumeSliders[0].value);
        SetSFXVolume(volumeSliders[1].value);
        SetMusicVolume(volumeSliders[2].value);
        SetAmbienceVolume(volumeSliders[3].value);
    }
    
    public void SetMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
        MasterMixer.SetFloat("MasterVolume", convertSliderToVolume(volume));
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
        MasterMixer.SetFloat("SFXVolume", convertSliderToVolume(volume));
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
        MasterMixer.SetFloat("MusicVolume", convertSliderToVolume(volume));
    }

    public void SetAmbienceVolume(float volume)
    {
        PlayerPrefs.SetFloat("AmbienceVolume", volume);
        PlayerPrefs.Save();
        MasterMixer.SetFloat("AmbienceVolume", convertSliderToVolume(volume));
    }
    
    private float convertSliderToVolume(float volume)
    {

        return Mathf.Log10(volume)*20;
    }
    
    public void SetEnginePitch(float PitchMultiplier)
    {
        if (PitchMultiplier <= 2f && PitchMultiplier >= 0.5f)
        {
            MasterMixer.SetFloat("EnginePitch", PitchMultiplier);
        }
    }

    public void GetAllEngineAccelerations()
    {
        try {
            foreach (GameObject car in GameManager.CarInstances)
            {
                Rigidbody RB = car.GetComponent<Rigidbody>();
                PlayerCarController PCC = car.GetComponent<PlayerCarController>();
                if (PCC != null)
                {
                    car.transform.GetChild(2).GetChild(1).GetComponent<AudioSource>().pitch = 1f + ((RB.velocity.magnitude / PCC.MaxSpeed));
                }
                else
                {
                    car.transform.GetChild(1).GetComponent<AudioSource>().pitch = 1f + ((RB.velocity.magnitude / car.GetComponent<NPCCarController>().MaxSpeed));
                }
            }
        }
        catch (MissingReferenceException) {
            ParkingGameManager.Instance.UpdatePlayerList();
            GetAllEngineAccelerations();
        }
    }

    //Example of volume set by a volume slider
    /*
    public void SetMasterVolume(Slider volume)
    {
        MasterMixer.SetFloat("Master", volume.value);
    }
    */
}
