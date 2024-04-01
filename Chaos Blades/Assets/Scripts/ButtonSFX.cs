using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameObject.name == "RestartButton")
        {
            AudioManager.instance.Play("ReplayClick");
        }

        if (gameObject.name == "StartButton")
        {
            AudioManager.instance.Play("StartButton");
        }

        else
        {
            AudioManager.instance.Play("ButtonClick");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.Play("ButtonHover");
    }
}
