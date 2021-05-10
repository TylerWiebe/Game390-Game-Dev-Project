using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;

public class UpdateParkingSpotDuration : MonoBehaviourPunCallbacks
{
    public Text text;

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("timeUntilParkingSpotMoves"))
        {
            text.text = "Parking Spot Moves In: " + propertiesThatChanged["timeUntilParkingSpotMoves"].ToString() + "s";
        }
    }
}
