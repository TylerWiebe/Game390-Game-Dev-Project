using CarCustomizations;
using System;
using UnityEngine;
using Photon.Pun;

public class PlayerSkinManager : MonoBehaviour
{
    public GameObject[] selectors;
    public GameObject Axis;

    public void Start()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("skin"))
            ChangeSkin(PlayerProperties.Skin);
    }

    private void FixedUpdate()
    {
       Quaternion rot = Axis.transform.rotation;
       rot *= Quaternion.Euler(Vector3.up * 1);
       Axis.transform.rotation = rot;
    }


    public void ChangeSkin(Skins skin)
    {
        SetSelector((int)skin);
        SetPlayerSkin(skin);
    }

    private void SetSelector(int idx)
    {
        for (int i = 0; i < selectors.Length; i++)
        {
            if (i == idx)
                selectors[i].SetActive(true);
            else
                selectors[i].SetActive(false);
        }
        PlayerProperties.Skin = (Skins)Enum.Parse(typeof(Skins), idx.ToString());
    }

    private void SetPlayerSkin(Skins skin)
    {
        Destroy(Axis.transform.GetChild(0).gameObject);
        GameObject car = (GameObject) Instantiate(Resources.Load(skin.ToString("g")), Axis.transform.position, Axis.transform.rotation);
        car.transform.parent = Axis.transform;

        car.GetComponent<Rigidbody>().useGravity = false;
        car.GetComponent<Rigidbody>().isKinematic = true;
        Destroy(car.GetComponent<PlayerCarController>());
        Destroy(car.transform.GetChild(1).gameObject);
        Destroy(car.transform.GetChild(2).gameObject);
    }

    public void HideCar()
    {
        Axis.SetActive(false);
    }

    public void ShowCar()
    {
        Axis.SetActive(true);
    }
}
