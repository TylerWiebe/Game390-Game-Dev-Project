using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine;

public class GameState
{
    public static float TimeInParkingSpot
    {
        get
        {
            return GetPlayerProperty<float>("timeInParkingSpot");
        }
        set
        {
            SetPlayerProperty("timeInParkingSpot", value);
        }
    }

    public static string PlayerInParkingSpot
    {
        get
        {
            return GetPlayerProperty<string>("playerInParkingSpot");
        }
        set
        {
            SetPlayerProperty("playerInParkingSpot", value);
        }
    }

    public static int ParkingSpotsLeft
    {
        get
        {
            return GetPlayerProperty<int>("parkingSpotsLeft");
        }
        set
        {
            SetPlayerProperty("parkingSpotsLeft", value);
        }
    }

    public static int TimeUntilParkingSpotMoves
    {
        get
        {
            return GetPlayerProperty<int>("timeUntilParkingSpotMoves");
        }
        set
        {
            SetPlayerProperty("timeUntilParkingSpotMoves", value);
        }
    }

    public static float ScoreOfCurrentLeader
    {
        get
        {
            return GetPlayerProperty<float>("scoreOfCurrentLeader");
        }
        set
        {
            SetPlayerProperty("scoreOfCurrentLeader", value);
        }
    }

    public static void SetPlayerProperty(string key, object value)
    {
        Hashtable prop = new Hashtable
        {
            {key, value }
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(prop);
    }

    public static T GetPlayerProperty<T>(string key)
    {
        object value;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(key, out value))
        {
            return (T)value;
        }
        return default(T);
    }
}
