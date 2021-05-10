using Photon.Pun;
using UnityEngine;

public class MultiplayerPlayer : MonoBehaviourPunCallbacks
{
    public static GameObject LocalPlayer { get; private set; }

    private void Awake()
    {
        if (photonView.IsMine)
        {
            // Register local player instance
            LocalPlayer = gameObject;

            // Attach cameras
            foreach (GameObject carCam in GameObject.FindGameObjectsWithTag(Tags.CarCamera))
            {
                carCam.GetComponent<CameraSync>().Sync();
            }
        }

        // Don't destroy when level syncs (Scene reload)
        DontDestroyOnLoad(gameObject);
    }

    public void OnDestroy()
    {
        if (photonView.IsMine)
        {
            LocalPlayer = null;
        }
    }

    public void DisableOwnership() {
        if (photonView.IsMine)
        {
            LocalPlayer = null;
        }
    }
}
