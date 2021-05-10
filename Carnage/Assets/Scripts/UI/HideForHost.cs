using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
public class HideForHost : MonoBehaviour
{

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (gameObject.GetComponent<Text>() != null)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
