using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine;

public class RoomSettingsDisplay : MonoBehaviourPunCallbacks
{
    public Text Seed;
    public Text Scene;

    public InputField BumperReboundMasterInput;
    public Text BumperReboundClientText;

    public InputField MaxPlayersMasterInput;
    public Text MaxPlayersClientText;

    protected virtual void UpdateDisplay()
    {
        Seed.text = "Seed: " + RoomSettings.Seed;
        Scene.text = "Scene: " + RoomSettings.Scene;

        if (!PhotonNetwork.IsMasterClient)
        {
            BumperReboundClientText.text = RoomSettings.BumperCarRebound.ToString();
            MaxPlayersClientText.text = RoomSettings.MaxPlayers.ToString();
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        UpdateDisplay();
    }
}
