using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Nametag : MonoBehaviour
{
    [SerializeField]
    private Text nametag;
    private GameObject mainCamera;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag(Tags.MainCamera);
        nametag.text = transform.root.GetComponent<PhotonView>().Owner.NickName;
        if (transform.root.GetComponent<PhotonView>().IsMine)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }
}
