using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Photon.Pun;


public class ParkingLotManager : MonoBehaviour
{
    private List<GameObject> parkingLots = new List<GameObject>();
    public GameObject Beacon;
    public GameObject EmptyParkingSpot;

    public static ParkingLotManager Instance;

     //ID of the parking lot that currently contains an empty parking spot
     private int currentLotID = -1;

     private void Start()
     {
        Instance = this;
        foreach (Transform child in transform)
        {
            GameObject g = child.gameObject;
            if (g != EmptyParkingSpot)
                parkingLots.Add(g);
        }
        if (PhotonNetwork.IsMasterClient) 
            GenerateNewParkingSpot();
     }

     //This update function is used to generate a new parking spot for testing purposes
     //Use this instead of having to go to each parking spot
     private void Update()
     {
        if (Input.GetKeyDown(KeyCode.K) && Debug.isDebugBuild)
        {
            GenerateNewParkingSpot();
        }
     }
        
     public void GenerateNewParkingSpot()
     {
        int nextLotID = Random.Range(0, parkingLots.Count);
        if (parkingLots.Count > 1)
        {
            while (nextLotID == currentLotID)
            {
                nextLotID = Random.Range(0, parkingLots.Count);
            }
        }
        
        currentLotID = nextLotID;
        if (parkingLots.Count == 1)
        {
            currentLotID = 0;
        }

        parkingLots[currentLotID].GetComponent<ParkingSpotManager>().ChooseEmptyParkingSpotLocation();
        Beacon.transform.transform.position = parkingLots[currentLotID].transform.position;
     }
}
