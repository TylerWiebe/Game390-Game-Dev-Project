using UnityEngine;
using UnityEngine.UI;

public class JoinRoom : MonoBehaviour
{
    public InputField Text;

    public void Join()
    {
        LobbyManager.JoinRoom(int.Parse(Text.text));
    }
}
