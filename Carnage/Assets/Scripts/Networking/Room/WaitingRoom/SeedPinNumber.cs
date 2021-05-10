using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedPinNumber : MonoBehaviour
{
    public Text text;
    public RoomSetupManager roomSetupManager;
    
    private void Awake()
    {
        text.text = "Pin: " + PhotonNetwork.CurrentRoom.Name;
    }
}
