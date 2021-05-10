using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject InGameMenu;
    private WindowManager windowManager;
    
    void Start()
    {
        InGameMenu.SetActive(false);
        windowManager = InGameMenu.GetComponent<WindowManager>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            InGameMenu.SetActive(!InGameMenu.activeSelf);
            windowManager.SwitchToWindow(0);
        }
    }

    public void Resume()
    {
        InGameMenu.SetActive(!InGameMenu.activeSelf);
        windowManager.SwitchToWindow(0);
    }
}
