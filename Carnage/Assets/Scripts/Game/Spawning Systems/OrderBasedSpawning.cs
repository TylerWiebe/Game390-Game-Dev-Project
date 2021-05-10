using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class OrderBasedSpawning : SpawningSystem
{
    [SerializeField]
    private Transform[] spawnpoints;

    public override void SpawnPlayer(MultiplayerGameManager gm)
    {
        if (spawnpoints.Length < PhotonNetwork.PlayerList.Length)
            Debug.LogError("Not enough spawnpoints for every player in the room.");

        int idx = 0;
        foreach(Player p in PhotonNetwork.PlayerList)
        {
            if (p == PhotonNetwork.LocalPlayer)
            {
                gm.InstantiatePlayer(spawnpoints[idx]);
                return;
            }
            idx++;
        }

        //Fallback
        Debug.LogError("Faling back to spawn player at the first spawnpoint.");
        gm.InstantiatePlayer(spawnpoints[0]);
    }

    public override Transform GetAvailableSpawnpoint() {
        return spawnpoints[Random.Range(0,spawnpoints.Length)];
    }


}
