using UnityEngine.UI;
using UnityEngine;

public class MakeTransparent : MonoBehaviour
{
    void Start()
    {
        Color temp = gameObject.GetComponent<Image>().color;
        temp.a = 0f;
        gameObject.GetComponent<Image>().color = temp;
    }
}
