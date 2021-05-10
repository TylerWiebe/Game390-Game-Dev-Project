using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SelectScene : MonoBehaviour
{
    Dropdown dropdown;

    void Start()
    {
        dropdown = GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener(delegate
        {
            UpdateSceneSelection(dropdown);
        });
        UpdateSceneSelection(dropdown);
    }

    void UpdateSceneSelection(Dropdown change)
    {
        if (PhotonNetwork.IsMasterClient)
            RoomSettings.Scene = change.captionText.text;
    }
}
