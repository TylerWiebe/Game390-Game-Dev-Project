using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;

public class UpdatePoints : MonoBehaviourPunCallbacks
{
    public Text text;
    public ScoreParkingGame ScoreParkingGame;

    public override void OnPlayerPropertiesUpdate(Player player, Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("playerScore") && player == PhotonNetwork.LocalPlayer)
        {
            text.text = "Points: " + propertiesThatChanged["playerScore"].ToString();
            GameState.ScoreOfCurrentLeader = (float)ScoreParkingGame.CurrentScores()[0].CustomProperties["playerScore"];
        }
    }
}
