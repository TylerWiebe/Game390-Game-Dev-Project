using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UpdatePlayerName : MonoBehaviour
{
    public InputField nameInputField;

    public void ChooseNameButtonClicked()
    {
        PhotonNetwork.NickName = nameInputField.text;
    }
}
