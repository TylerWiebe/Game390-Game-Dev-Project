using Photon.Realtime;
using Photon.Pun;
using System.Linq;

using UnityEngine;

public class ScoreParkingGame : MonoBehaviour
{
    public Player[] CurrentScores()
    {
        Player[] players = PhotonNetwork.PlayerList;
        return players.OrderByDescending(player => player.CustomProperties["playerScore"]).ToArray<Player>();
    }
}
