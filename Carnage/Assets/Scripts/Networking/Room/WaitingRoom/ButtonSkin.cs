using CarCustomizations;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSkin : MonoBehaviour
{
    [SerializeField]
    Skins skin;

    [SerializeField]
    PlayerSkinManager playerSkinManager;

    public void ChangeSkin()
    {
        playerSkinManager.ChangeSkin(skin);
    }   
}