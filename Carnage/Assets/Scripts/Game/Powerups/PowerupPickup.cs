using Photon.Pun;
using System.Threading.Tasks;
using UnityEngine;

public class PowerupPickup : MonoBehaviour
{
    public int SpawnFrequency = 5000;
    public Powerup[] PowerUps;

    private Powerup powerup;
    private PhotonView photonView;
    private MeshRenderer meshRenderer;
    private Collider collider;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        meshRenderer = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
        
        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("SelectPowerUp", RpcTarget.All, RandomPowerUp());
    }

    [PunRPC]
    public void SelectPowerUp(int idx)
    {
        if (idx < 0)
        {
            meshRenderer.enabled = false;
            collider.enabled = false;
            if (transform.childCount > 0)
                Destroy(transform.GetChild(0).gameObject);
            powerup = null;
            if (PhotonNetwork.IsMasterClient)
                StartSpawn();
        } else
        {
            meshRenderer.enabled = false;
            collider.enabled = true;
            if (transform.childCount > 0)
                Destroy(transform.GetChild(0).gameObject);
            powerup = PowerUps[idx];
            Instantiate(powerup, transform.position, transform.rotation, transform);
        }
    }

    private async void StartSpawn()
    {
        await Task.Delay(SpawnFrequency);
        photonView.RPC("SelectPowerUp", RpcTarget.All, RandomPowerUp());
    }

    private int RandomPowerUp()
    {
        return Random.Range(0, PowerUps.Length);
    }

    public Powerup GetPowerup()
    {
        return powerup;
    }

    public void PowerupWasPickedUp()
    {
        photonView.RPC("SelectPowerUp", RpcTarget.All, -1);
    }
}
