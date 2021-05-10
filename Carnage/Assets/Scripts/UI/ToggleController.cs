using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    [SerializeField]
    private Toggle myToggle;
    private void Start()
    {
        
        bool temp = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        Debug.LogError(temp);
        Screen.fullScreen = temp;
        myToggle.isOn = temp;
        myToggle.onValueChanged.AddListener(ToggleFullscreen);
    }

    

    public void ToggleFullscreen(bool Value)
    {
        Debug.LogError("TogglingFullscreen");
        Screen.fullScreen = myToggle.isOn;
        if (Screen.fullScreen)
        {
            PlayerPrefs.SetInt("Fullscreen", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
        }

        PlayerPrefs.Save();
    }
}
