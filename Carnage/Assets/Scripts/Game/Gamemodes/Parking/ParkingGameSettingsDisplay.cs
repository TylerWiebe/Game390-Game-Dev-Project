using UnityEngine.UI;
using Photon.Pun;
using UnityEngine;

public class ParkingGameSettingsDisplay : RoomSettingsDisplay
{
    public InputField VictoryCountdownMasterInput;
    public Text VictoryCountdownClientText;

    public InputField NumberOfTimesParkingSpotMovesMasterInput;
    public Text NumberOfTimesParkingSpotMovesClientText;


    private void Start()
    {
        UpdateDisplay();
    }

    protected override void UpdateDisplay()
    {
        base.UpdateDisplay();
        if (!PhotonNetwork.IsMasterClient)
        {
            VictoryCountdownClientText.text = ParkingGameSettings.VictoryCountdown.ToString() + "s";
            NumberOfTimesParkingSpotMovesClientText.text = ParkingGameSettings.NumberOfTimesParkingSpotMoves.ToString();
        }        
    }
}
