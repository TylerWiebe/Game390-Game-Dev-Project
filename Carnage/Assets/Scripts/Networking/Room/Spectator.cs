using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectator : MonoBehaviour
{
    public GameObject lockedVCam;
    public GameObject simpleVCam;
    public GameObject noRollVCam;
    public GameObject mainCamera;

    void Start()
    {
        if (RoomSettings.IsGameInProgress)
            EnableSpectatorCamera();
        else
            DisableSpecatorCamera();
    }

    private void EnableSpectatorCamera()
    {
        gameObject.SetActive(true);
        lockedVCam.SetActive(false);
        simpleVCam.SetActive(false);
        noRollVCam.SetActive(false);
        mainCamera.SetActive(false);
    }

    private void DisableSpecatorCamera()
    {
        gameObject.SetActive(false);
    }
}
