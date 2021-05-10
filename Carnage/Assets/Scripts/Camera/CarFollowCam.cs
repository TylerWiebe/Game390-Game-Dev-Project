using Cinemachine;
using UnityEngine;

public class CarFollowCam : MonoBehaviour
{
    public float Threshold = 2;
    public CinemachineVirtualCamera LockVCam;
    public CinemachineVirtualCamera SimpleVCam;
    public CinemachineVirtualCamera NoRollCam;
    public float RollFixRange = 80;

    private Rigidbody targetRb;

    private void Start()
    {
        Sync();
    }

    private void Sync()
    {
        if (MultiplayerPlayer.LocalPlayer == null)
            return;
        targetRb = MultiplayerPlayer.LocalPlayer.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (targetRb == null)
        {
            Sync();
        }
        else
        {
            if (targetRb.angularVelocity.magnitude > Threshold)
            {
                SetModeSimple();
            }
            else if (targetRb.transform.localEulerAngles.z > 180 - RollFixRange && targetRb.transform.localEulerAngles.z < 180 + RollFixRange)
            {
                SetModeNoRoll();
            }
            else
            {
                SetModeLock();
            }
        }
    }

    private void SetModeLock()
    {
        NoRollCam.Priority = 8;
        SimpleVCam.Priority = 9;
        LockVCam.Priority = 10;
    }

    private void SetModeSimple()
    {
        NoRollCam.Priority = 8;
        LockVCam.Priority = 9;
        SimpleVCam.Priority = 10;
    }

    private void SetModeNoRoll()
    {
        LockVCam.Priority = 8;
        SimpleVCam.Priority = 9;
        NoRollCam.Priority = 10;
    }
}
