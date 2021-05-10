using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine;

public class ParkingGameSetupManager : RoomSetupManager
{
    public float DefaultVictoryCountdown = 5f;
    public int DefaultNumberOfTimesParkingSpotMoves = 0;

    public InputField VictoryCountdownMasterInput;
    public Text VictoryCountdownClientText;

    public InputField NumberOfTimesParkingSpotMovesMasterInput;
    public Text NumberOfTimesParkingSpotMovesClientText;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            SetupRoomSettings();
        else
            FetchRoomSettings();
    }

    protected override void SetupRoomSettings()
    {
        base.SetupRoomSettings();

        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("victory_time"))
        {
            ParkingGameSettings.VictoryCountdown = DefaultVictoryCountdown;
            VictoryCountdownMasterInput.text = DefaultVictoryCountdown.ToString();
        } else 
            VictoryCountdownMasterInput.text = ParkingGameSettings.VictoryCountdown.ToString();

        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("times_parking_spot_moves"))
        {
            ParkingGameSettings.NumberOfTimesParkingSpotMoves = DefaultNumberOfTimesParkingSpotMoves;
            GameState.ParkingSpotsLeft = DefaultNumberOfTimesParkingSpotMoves;
            NumberOfTimesParkingSpotMovesMasterInput.text = DefaultNumberOfTimesParkingSpotMoves.ToString();
        }
        else 
            NumberOfTimesParkingSpotMovesMasterInput.text = ParkingGameSettings.NumberOfTimesParkingSpotMoves.ToString();

        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("gamemode"))
        {
            ParkingGameSettings.GameMode = ParkingGameManager.GameMode.KingOfTheHill;
        }

    }

    protected override void FetchRoomSettings()
    {
        base.FetchRoomSettings();
        VictoryCountdownClientText.text = ParkingGameSettings.VictoryCountdown.ToString();
        NumberOfTimesParkingSpotMovesClientText.text = ParkingGameSettings.NumberOfTimesParkingSpotMoves.ToString();
    }

    public override void StartGame()
    {
        SetupRoomSettings();
        base.StartGame();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
