using UnityEngine;

public abstract class SpawningSystem : MonoBehaviour
{
    public abstract void SpawnPlayer(MultiplayerGameManager gm);
    public abstract Transform GetAvailableSpawnpoint();
}
