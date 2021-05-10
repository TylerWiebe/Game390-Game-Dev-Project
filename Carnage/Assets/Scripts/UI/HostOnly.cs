using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class HostOnly : MonoBehaviour
{
    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            if (gameObject.GetComponent<Button>() != null)
                gameObject.GetComponent<Button>().interactable = false;
            else if (gameObject.GetComponent<InputField>() != null)
            {
                gameObject.GetComponent<InputField>().interactable = false;
                gameObject.SetActive(false);
            }
            else if (gameObject.GetComponent<Dropdown>() != null)
            {
                gameObject.GetComponent<Dropdown>().interactable = false;
                gameObject.SetActive(false);
            }
        }
    }
}
