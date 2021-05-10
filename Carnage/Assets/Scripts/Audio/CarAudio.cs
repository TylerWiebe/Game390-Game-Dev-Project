using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class CarAudio : MonoBehaviour
{
    // Unity Audio Sources
    public AudioSource HornAS;
    
    [PunRPC]
    public void HonkHorn() 
    {
        if (!HornAS.isPlaying)
        {
            HornAS.Play();
        }
    } 
}
