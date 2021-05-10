using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSFXManager : MonoBehaviour
{
    public AudioSource ButtonAudioSrc;

    public AudioClip ButtonHover;
    public AudioClip ButtonSelect;

    public static MenuSFXManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        int numMusicPlayers = FindObjectsOfType<MenuSFXManager>().Length;
        if (numMusicPlayers > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void playButtonHover()
    {
        ButtonAudioSrc.PlayOneShot(ButtonHover);
    }

    public void playButtonSelect()
    {
        ButtonAudioSrc.PlayOneShot(ButtonSelect);
    }
}
