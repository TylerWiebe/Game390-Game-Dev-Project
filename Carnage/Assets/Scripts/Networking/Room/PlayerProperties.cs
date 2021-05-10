using CarCustomizations;
using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine;


public class PlayerProperties
{
    public static Skins Skin
    {
        get
        {
            return GetPlayerProperty<Skins>("skin");
        }
        set
        {
            SetPlayerProperty("skin", value);
        }
    }
    
    public static float PlayerScore
    {
        get
        {
            return GetPlayerProperty<float>("playerScore");
        }
        set
        {
            SetPlayerProperty("playerScore", value);
        }
    }

    public static void SetPlayerProperty(string key, object value)
    {
        Hashtable prop = new Hashtable
        {
            {key, value }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(prop);
    }

    public static T GetPlayerProperty<T>(string key)
    {
        object value;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(key, out value))
        {
            return (T)value;
        } 
        return default(T);
    }
}
