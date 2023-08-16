using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnPointerDownHandler;
    public event Action OnPointerUpHandler;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownHandler?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpHandler?.Invoke();
    }
}
