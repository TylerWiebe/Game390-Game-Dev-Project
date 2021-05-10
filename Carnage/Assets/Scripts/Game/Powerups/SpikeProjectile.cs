using Photon.Pun;
using UnityEngine;

public class SpikeProjectile : MonoBehaviour
{
    public string SpikeTrap = "Spike Trap";
    private void OnCollisionEnter(Collision collision) {
        if (PhotonNetwork.IsMasterClient) {
            GameObject spikeTrap = PhotonNetwork.Instantiate(SpikeTrap, transform.position, transform.rotation, 0);
            //spikeTrap.transform.up = collision.contacts[0].normal;
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
