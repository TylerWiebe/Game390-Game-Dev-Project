using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayersInRoom : MonoBehaviour
{
    public Text playersInRoomText;
    
    public void FixedUpdate()
    {
        if (playersInRoomText != null)
        {
            playersInRoomText.text = "Players in Room: " + PlayersCurrentlyInRoom();
        }
    }

    public string PlayersCurrentlyInRoom()
    {
        string temp = "";
        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length; i++)
        {
            if (i == players.Length - 1)
            {
                if (players[i] == PhotonNetwork.LocalPlayer)
                {
                    temp += players[i].NickName + " (You)";
                }
                else
                {
                    temp += players[i].NickName;
                }
            }
            else
            {
                if (players[i] == PhotonNetwork.LocalPlayer)
                {
                    temp += players[i].NickName + " (You), ";
                }
                else
                {
                    temp += players[i].NickName + ", ";
                }
            }
        }

        return temp;
    }
}
