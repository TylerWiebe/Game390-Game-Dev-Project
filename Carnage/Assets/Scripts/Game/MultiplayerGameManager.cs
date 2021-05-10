using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using UnityEngine.Assertions;

public class MultiplayerGameManager : RoomManager, IOnEventCallback
{
    public static MultiplayerGameManager Instance { get; private set; }

    public GameObject[] CarInstances
    {
        get
        {
            return (GameObject[])playerInstances.Clone();
        }
    }
    private GameObject[] playerInstances = new GameObject[0];


    public void Awake()
    {
        Assert.IsNull(Instance);
        Instance = this;
    }

    public GameObject InstantiatePlayer(Transform spawnPoint)
    {
        return InstantiatePlayer(spawnPoint.position, spawnPoint.rotation);
    }

    public GameObject InstantiatePlayer(Vector3 position, Quaternion rotation)
    {
        if (MultiplayerPlayer.LocalPlayer == null)
        {
            GameObject player = PhotonNetwork.Instantiate(PlayerProperties.Skin.ToString("g"), position, rotation, 0);
            PhotonNetwork.RaiseEvent(2, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
            return player;
        }
        else
        {
            throw new Exception("Player instance already exists.");
        }
    }

    public GameObject InstantiatePlayer(String skin, Vector3 position, Quaternion rotation)
    {
        if (MultiplayerPlayer.LocalPlayer == null)
        {
            GameObject player = PhotonNetwork.Instantiate(skin, position, rotation, 0);
            PhotonNetwork.RaiseEvent(2, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
            return player;
        }
        else
        {
            throw new Exception("Player instance already exists.");
        }
    }

    public virtual void OnEvent(EventData eventData)
    {
        switch(eventData.Code)
        {
            case 2:
            UpdatePlayerList(); 
            break;
        }
    }
    
    public void UpdatePlayerList()
    {
        playerInstances = GameObject.FindGameObjectsWithTag(Tags.Player);
    }
    
    public new void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
        base.OnEnable();
    }

    public new void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        base.OnDisable();
    }

    public void OnDestroy()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag(Tags.Player))
        {
            Destroy(player);
        } 
    }
}
