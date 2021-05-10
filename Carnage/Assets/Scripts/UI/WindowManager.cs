using System;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> windows;

    private int openWindow = 0;

    private void Start()
    {
        foreach(GameObject window in windows)
        {
            window.SetActive(false);
        }
        windows[openWindow].SetActive(true);
    }

    public void SwitchToWindow(int idx)
    {
        if (idx >= windows.Count)
            throw new IndexOutOfRangeException();
        windows[openWindow].SetActive(false);
        openWindow = idx;
        windows[openWindow].SetActive(true);
    }
}
