using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject CouldNotFindRoomError;
    public Toggle PinToggle;


    #region Private Serializable Fields

    [SerializeField]
    private string gameVersion = "1";
    [SerializeField]
    private string defaultPlayerName = "Player";
    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    #endregion

    #region MonoBehaviour CallBacks

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        Connect();
        if (PhotonNetwork.NickName == "")
        {
            PhotonNetwork.NickName = defaultPlayerName;
        }
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene(0);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        CouldNotFindRoomError.SetActive(true);
        PinToggle.isOn = false;
        Debug.Log("OnJoinRoomFailed() callback \n Code: " + returnCode + "\n message: " + message);
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene(2);
    }

    #endregion

    #region Connection Management

    public static void JoinRoom(int pin)
    {
        PhotonNetwork.JoinRoom(pin.ToString());
    }

    public void CreateRoom()
    {
        int pin = GenereateRoomPin();
        PhotonNetwork.CreateRoom(pin.ToString(), new RoomOptions { MaxPlayers = maxPlayersPerRoom, PublishUserId = true });
    }

    #endregion

    #region Utility

    public static int GenereateRoomPin()
    {
        float min = 0.1f;
        float max = 0.9999f;
        return (int)(Random.Range(min, max) * 10000);
    }

    public static void SetNickName(string nickName)
    {
        PhotonNetwork.NickName = nickName;
    }

    #endregion

    #region Button Triggered

    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "usw";
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    #endregion
}