using ExitGames.Client.Photon;
using Photon.Pun;

public class RoomSettings
{
    public static int Seed {
        get
        {
            return GetRoomProperty<int>("seed");
        }
        set
        {
            SetRoomProperty("seed", value);
        }
    }

    public static string Scene
    {
        get
        {
            return GetRoomProperty<string>("scene");
        }
        set
        {
            SetRoomProperty("scene", value);
        }
    }

    public static int MaxPlayers
    {
        get
        {
            return PhotonNetwork.CurrentRoom.MaxPlayers;
        }
        set
        {
            PhotonNetwork.CurrentRoom.MaxPlayers = (byte)value;
        }
    }

    public static float BumperCarRebound
    {
        get
        {
            return GetRoomProperty<float>("rebound");
        }
        set
        {
            SetRoomProperty("rebound", value);
        }
    }

    public static bool IsGameInProgress
    {
        get
        {
            return GetRoomProperty<bool>("IsGameInProgress");
        }
        set
        {
            SetRoomProperty("IsGameInProgress", value);
        }

    }

    public static void SetRoomProperty(string key, object value)
    {
        Hashtable prop = new Hashtable
        {
            { key, value }
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(prop);
    }

    public static T GetRoomProperty<T>(string key)
    {
        object value;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(key, out value))
        {
            return (T)value;
        }
        return default(T);
    }
}
