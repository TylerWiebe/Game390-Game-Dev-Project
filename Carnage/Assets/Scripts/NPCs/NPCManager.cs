using System.Collections;
using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour
{
    public int MinNPCs = 20;
    public float SpawnDelay = 5;
    private int spawnCount;

    public static List<GameObject> cars;

    void Start()
    {
        cars = new List<GameObject>();
        if (!PhotonNetwork.IsMasterClient)
            return;
        spawnCount = MinNPCs;
        StartCoroutine(SpawnNPCs());
    }

    IEnumerator SpawnNPCs() {
        while (spawnCount > 0) {
            yield return new WaitForSeconds(SpawnDelay);
            SpawnNPC();
            spawnCount--;
        }
    }

    private void SpawnNPC() {
        int idx = Random.Range(0, transform.childCount);
        Transform spawnpoint = transform.GetChild(idx);
        Navigator nav = spawnpoint.GetComponent<Navigator>();
        GameObject npc = PhotonNetwork.Instantiate("NPC", spawnpoint.transform.position, spawnpoint.transform.rotation, 0);
        cars.Add(npc);
        npc.GetComponent<NPCCarController>().Manager = this;
        Navigator npcnav = npc.GetComponent<Navigator>();
        npcnav.TargetPoint = nav.TargetPoint;
        npcnav.PreviousPoint = nav.PreviousPoint;
    }

    public void CarDestroyed(GameObject npc) {
        if (!PhotonNetwork.IsMasterClient)
            return;
        cars.Remove(npc);
        bool startSpawnRoutine = spawnCount == 0;
        spawnCount++;
        if (startSpawnRoutine) {
            StartCoroutine(SpawnNPCs());
        }
    }
}
