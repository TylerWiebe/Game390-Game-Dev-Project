using Photon.Pun;
using UnityEngine;

public class OilProjectile : MonoBehaviour
{
    public int FuseLength = 200;
    private int count = 0;

    void Update()
    {
        count++;
        if (count == 50) {
            GetComponent<Collider>().enabled = true;
        }
        if (count == FuseLength) {
            GetComponent<Collider>().enabled = true;
            Explode();
        }
    }

    private void Explode() {
        Explosion explosion = PhotonNetwork.Instantiate("Oil", transform.position, Quaternion.identity, 0).GetComponent<Explosion>();
        Destroy(gameObject);
    }
}
