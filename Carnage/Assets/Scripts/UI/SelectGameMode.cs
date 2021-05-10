using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;

public class SelectGameMode : MonoBehaviour
{
    Dropdown dropdown;

    void Start()
    {
        dropdown = GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener(delegate
        {
            UpdateGameModeSelection(dropdown);
        });

        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("gamemode"))
            UpdateGameModeSelection(dropdown);
        else
            dropdown.value = (int)ParkingGameSettings.GameMode;
    }

    void UpdateGameModeSelection(Dropdown change)
    {
        if (change.captionText.text == "First to Park")
            ParkingGameSettings.GameMode = ParkingGameManager.GameMode.FirstToPark;
        else if (change.captionText.text == "King of the Hill")
            ParkingGameSettings.GameMode = ParkingGameManager.GameMode.KingOfTheHill;
    }
}
