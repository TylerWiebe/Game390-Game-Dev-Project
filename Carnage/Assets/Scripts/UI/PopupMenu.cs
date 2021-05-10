using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject popupMenu;

    [SerializeField]
    private GameObject endGameMenu;

    [SerializeField]
    private Text victoryTime;

    [SerializeField]
    private TimeTrial myTimeTrial;

    private void Awake()
    {
        endGameMenu.SetActive(false);
        popupMenu.SetActive(false);
    }

    private void Start()
    {
        myTimeTrial.OnFinish.AddListener(OpenEndGameMenu);
    }

    public void OpenMenu()
    {
        popupMenu.SetActive(true);
    }

    public void CloseMenu()
    {
        popupMenu.SetActive(false);
    }

    public void OpenEndGameMenu(long time)
    {
        
        if (victoryTime != null)
        {
            victoryTime.text = "Finished with a time of " + time + "ms.";
        }
        endGameMenu.SetActive(true);    
    }
}
