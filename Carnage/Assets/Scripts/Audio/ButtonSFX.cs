using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{

    public bool IsButton;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsButton)
        {
            MenuSFXManager.Instance.playButtonSelect();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuSFXManager.Instance.playButtonHover();
    }
}
