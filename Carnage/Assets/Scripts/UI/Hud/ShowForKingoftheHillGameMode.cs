using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowForKingoftheHillGameMode : MonoBehaviour
{
    void Start()
    {
        if (ParkingGameSettings.GameMode == ParkingGameManager.GameMode.KingOfTheHill)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
}
