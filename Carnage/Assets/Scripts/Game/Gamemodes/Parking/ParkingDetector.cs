using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class ParkingDetector : MonoBehaviour
{

    /// <summary>
    /// OnPlayerParkingEnter is called when the player fully enters the parking boundaries.
    /// </summary>
    public UnityEvent OnPlayerParkingEnter = new UnityEvent();

    /// <summary>
    /// OnPlayerParkingExit is called when the player is no longer fully within the parking boundaries.
    /// </summary>
    public UnityEvent OnPlayerParkingExit = new UnityEvent();

    /// <summary>
    /// OnPlayerParkingStay is called on each frame that the player remains fully within the parking boundaries.
    /// </summary>
    public UnityEvent OnPlayerParkingStay = new UnityEvent();
    private BoxCollider boxCollider;
    private bool parked = false;


    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        DetectParking(otherCollider.gameObject);
    }

    private void OnTriggerExit(Collider otherCollider)
    {
        DetectParking(otherCollider.gameObject);
    }

    private void OnTriggerStay(Collider otherCollider)
    {
        DetectParking(otherCollider.gameObject);
    }

    private void DetectParking(GameObject player)
    {
        if (player != MultiplayerPlayer.LocalPlayer || player.name.Contains("TaxiCar"))
            return;

        if (ColliderUtilities.Encompasses(boxCollider, player.GetComponent<BoxCollider>()))
        {
            if (parked)
            {
                OnPlayerParkingStay.Invoke();
            }
            else
            {
                parked = true;
                OnPlayerParkingEnter.Invoke();
            }
        }
        else
        {
            parked = false;
            OnPlayerParkingExit.Invoke();
            GameState.TimeInParkingSpot = 0f;
            GameState.PlayerInParkingSpot = "";
        }
    }
}
