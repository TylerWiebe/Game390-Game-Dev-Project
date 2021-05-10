using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private int nextSceneID = 0;

    public void ChangeScene()
    {
        SceneManager.LoadScene(nextSceneID);
    }

    public void Exit()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }

    public void BackToMenu()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }
}
