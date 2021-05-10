using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class RoomSetupManager : RoomManager
{
    public float DefaultRebound = 0.5f;
    public InputField BumperReboundMasterInput;
    public Text BumperReboundClientText;

    public byte DefaultMaxPlayers = 6;
    public InputField MaxPlayersMasterInput;
    public Text MaxPlayersClientText;

    public Text Seed;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            SetupRoomSettings();
        else
            FetchRoomSettings();

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("seed"))
            Random.InitState(RoomSettings.Seed);
    }

    #region Button Triggered
    public virtual void StartGame()
    {
        LoadScene(RoomSettings.Scene);
        RoomSettings.IsGameInProgress = true;
    }

    public void DisconnectFromRoom()
    {
        PhotonNetwork.Disconnect();
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks
    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene(3);
    }

    #endregion

    protected virtual void SetupRoomSettings()
    {
        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("rebound"))
        {
            RoomSettings.BumperCarRebound = DefaultRebound;
            BumperReboundMasterInput.text = DefaultRebound.ToString();
        } else
            BumperReboundMasterInput.text = RoomSettings.BumperCarRebound.ToString();

        MaxPlayersMasterInput.text = RoomSettings.MaxPlayers.ToString();
        RoomSettings.Seed = Random.Range(0, 1000000);
    }

    protected virtual void FetchRoomSettings()
    {
        BumperReboundClientText.text = RoomSettings.BumperCarRebound.ToString();
        MaxPlayersClientText.text = RoomSettings.MaxPlayers.ToString();
        Seed.text = "Seed: " + RoomSettings.Seed.ToString();
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("seed"))
        {
         Random.InitState(RoomSettings.Seed);
        }
    }
}
