using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class CollisionOwnership : MonoBehaviour
{
    private PhotonView pView;
    void Start()
    {
        pView = GetComponent<PhotonView>();
        pView.SetOwnerInternal(PhotonNetwork.MasterClient, PhotonNetwork.MasterClient.ActorNumber);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.root.gameObject == MultiplayerPlayer.LocalPlayer && !pView.AmOwner)
            pView.RequestOwnership();
    }
}
