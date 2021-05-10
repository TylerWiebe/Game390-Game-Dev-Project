using Photon.Pun;

public class Connecting : MonoBehaviourPunCallbacks
{
    public int NextWindow = 1;
    public override void OnConnectedToMaster()
    {
        GetComponent<WindowManager>().SwitchToWindow(NextWindow);
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
            GetComponent<WindowManager>().SwitchToWindow(NextWindow);
    }
}
