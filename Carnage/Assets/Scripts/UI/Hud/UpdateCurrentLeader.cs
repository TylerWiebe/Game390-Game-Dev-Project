using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;

public class UpdateCurrentLeader : MonoBehaviourPunCallbacks
{
    public Text text;

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("scoreOfCurrentLeader"))
        {
            text.text = "First Place: " + propertiesThatChanged["scoreOfCurrentLeader"].ToString();
        }
    }
}
