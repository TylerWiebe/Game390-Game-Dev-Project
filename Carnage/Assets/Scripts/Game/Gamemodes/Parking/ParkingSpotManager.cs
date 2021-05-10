using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ParkingSpotManager : MonoBehaviour
{
    private List<Transform> parkingSpots = new List<Transform>();

    [SerializeField]
    private GameObject[] parkedCars;

    private List<int> EmptySpotsID = new List<int>();

    //private Transform[] ParkingSpots;

    [SerializeField]
    private GameObject emptyParkingStall;

    private ParkingGameManager PGM;
    public ParkingDetector ParkingDetector { get; private set; }
    public ParkingLotManager ParkingLotManager;

    // Start is called before the first frame update
    //This function gathers a list of all parking spot locations and instantiates a parked car there, and one random stall is set to be the parking spot.
    //This script requires all parking locations to be a child of the object this script is attached too.
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            GetComponent<PhotonView>().RPC("AddCarsToParkingSpots", RpcTarget.All, (int)System.DateTime.Now.Ticks);
        PGM = GameObject.Find("ParkingGameManager").GetComponent<ParkingGameManager>();
    }

    [PunRPC]
    public void AddCarsToParkingSpots(int seed)
    {
        Random.InitState(seed);
        foreach (Transform Child in transform)
        {
            parkingSpots.Add(Child);
            GameObject parkedCar = Instantiate(parkedCars[Random.Range(0, parkedCars.Length)], new Vector3(Child.position.x, Child.position.y - 1.5f, Child.position.z), Child.rotation);
            parkedCar.transform.localEulerAngles += new Vector3(0, Random.Range(0, 2) * 180, 0);
            parkedCar.transform.parent = Child.transform;
            Child.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void ChooseEmptyParkingSpotLocation()
    {
        int emptyParkingStallID = GetRandomParkedCarID();
        GetComponent<PhotonView>().RPC("MoveEmptyParkingSpot", RpcTarget.All, emptyParkingStallID);
    }

    [PunRPC]
    public void MoveEmptyParkingSpot(int emptyParkingStallID)
    {
        EmptySpotsID.Add(emptyParkingStallID);
        Transform newParkingSpot = parkingSpots[emptyParkingStallID];
        Destroy(newParkingSpot.GetChild(0).gameObject);

        ParkingLotManager.EmptyParkingSpot.transform.position = newParkingSpot.position;
        ParkingLotManager.EmptyParkingSpot.transform.rotation = newParkingSpot.rotation;
        ParkingLotManager.EmptyParkingSpot.transform.parent = newParkingSpot.transform;
        ParkingLotManager.EmptyParkingSpot.name = "Empty Parking Spot";
        ParkingDetector = ParkingLotManager.EmptyParkingSpot.GetComponent<ParkingDetector>();
        PGM.AddParkingDetector(ParkingDetector);
        ParkingLotManager.Instance.Beacon.transform.position = transform.position;
    }

    private int GetRandomParkedCarID()
    {
        int temp = 0;
        if (EmptySpotsID.Count<parkingSpots.Count)
        {
            temp = Random.Range(0, parkingSpots.Count );
            while (EmptySpotsID.Contains(temp))
            {
                temp = Random.Range(0, parkingSpots.Count);
            }
        }
        
        return temp;
    }
}
