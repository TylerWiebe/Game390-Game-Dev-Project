
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.
using Photon.Pun;

public class SaveInputField : MonoBehaviour, IDeselectHandler
{
    public GameObject ParkingSpotMovesValueToHighError;

    public void OnDeselect(BaseEventData data)
    {
        float number;
        if (gameObject.name == "VictoryCountdownMasterInput" && float.TryParse(gameObject.GetComponent<InputField>().text, out number))
            ParkingGameSettings.VictoryCountdown = number;
        else if (gameObject.name == "NumberOfTimesParkingSpotMovesMasterInput" && float.TryParse(gameObject.GetComponent<InputField>().text, out number))
        {
            int playersInRoom = PhotonNetwork.CurrentRoom.PlayerCount;
            if (ParkingGameSettings.GameMode == ParkingGameManager.GameMode.FirstToPark && number > playersInRoom - 1)
            {
                ParkingGameSettings.NumberOfTimesParkingSpotMoves = playersInRoom - 1;
                GameState.ParkingSpotsLeft = (int)playersInRoom - 1;
                ParkingSpotMovesValueToHighError.SetActive(true);
            }
            else
            {
                ParkingGameSettings.NumberOfTimesParkingSpotMoves = number;
                GameState.ParkingSpotsLeft = (int)number;
                ParkingSpotMovesValueToHighError.SetActive(false);
            }
        }
        else if (gameObject.name == "BumperReboundMasterInput" && float.TryParse(gameObject.GetComponent<InputField>().text, out number))
            RoomSettings.BumperCarRebound = number;
        else if (gameObject.name == "MaxPlayersMasterInput" && float.TryParse(gameObject.GetComponent<InputField>().text, out number))
            RoomSettings.MaxPlayers = (byte)number;
    }
}

