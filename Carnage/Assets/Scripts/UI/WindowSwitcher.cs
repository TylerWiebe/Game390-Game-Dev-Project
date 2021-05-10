using UnityEngine;

public class WindowSwitcher : MonoBehaviour
{
    [SerializeField]
    private WindowManager windowManager;
    [SerializeField]
    private int windowIdx = 0;

    public void SwitchToWindow()
    {
        windowManager.SwitchToWindow(windowIdx);
    }
}
