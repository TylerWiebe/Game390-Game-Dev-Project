using Photon.Pun;
using UnityEngine;

public class SyncSceneObject : MonoBehaviour
{
    public int StartId = 0;
    void Start()
    {
        int i = 0;
        foreach (Transform t in transform)
        {
            PhotonView p = t.GetComponent<PhotonView>();
            if (p != null)
            {
                p.ViewID = StartId + i;
                i++;
            }
        }
    }
}
