using Photon.Pun;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PowerupUser : MonoBehaviour
{
    private Powerup powerup = null;
    private PhotonView photonView;

    public AudioClip powerupCollectionSFX;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (photonView.IsMine && Input.GetMouseButtonDown(0) && powerup != null)
        {
            powerup.Activate(this);
        }
    }
        
    public void ClearPowerup()
    {
        powerup = null;
        if (photonView.IsMine)
        {
            Color color = ParkingGameManager.Instance.PowerupDisplay.color;
            color.a = 0;
            ParkingGameManager.Instance.PowerupDisplay.color = color;
            ParkingGameManager.Instance.PowerupDisplay.sprite = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            if (powerup == null)
            {
                if (powerupCollectionSFX != null)
                    AudioSource.PlayClipAtPoint(powerupCollectionSFX, gameObject.transform.position, 1);
                
                PowerupPickup powerupPickup = other.GetComponent<PowerupPickup>();
                Debug.Log(powerupPickup);
                powerup = powerupPickup.GetPowerup();
                powerup.OnPickup(this);
                powerupPickup.PowerupWasPickedUp();
                
                if (photonView.IsMine)
                {
                    Color color = ParkingGameManager.Instance.PowerupDisplay.color;
                    color.a = 1;
                    ParkingGameManager.Instance.PowerupDisplay.color = color;
                    ParkingGameManager.Instance.PowerupDisplay.sprite = powerup.PowerUpIcon;

                }
            }
        }
    }
}
