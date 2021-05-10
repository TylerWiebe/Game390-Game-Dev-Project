using UnityEngine;

public class Billboard : MonoBehaviour
{
    public bool stayUpright = false;
    void Update() 
    {
        transform.LookAt(Camera.main.transform.position, -Vector3.up);
        if (stayUpright) {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
    }
}