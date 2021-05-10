using Cinemachine;
using UnityEngine;

public class CameraSync : MonoBehaviour
{
    private void Start()
    {
        Sync();
    }

    public void Sync()
    {
        if (MultiplayerPlayer.LocalPlayer == null)
            return;
        CinemachineVirtualCamera vcam = GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = MultiplayerPlayer.LocalPlayer.transform;
        vcam.LookAt = MultiplayerPlayer.LocalPlayer.transform;
    }

    public void SpectatorCamera()
    {
        CinemachineVirtualCamera vcam = GetComponent<CinemachineVirtualCamera>();
        Debug.Log("Use spectator camera");
    }
}
