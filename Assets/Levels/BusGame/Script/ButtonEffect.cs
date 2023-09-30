using System.Collections;
using System.Collections.Generic;
using Fungus;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ButtonEffect : MonoBehaviour
{
    public RectTransform yesRectTransform;
    public RectTransform noRectTransform;

    public Camera mainCamera;

    public void Update()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        if(mouseWorldPosition.x > 1)
        {
            
            if (yesRectTransform != null)
            {
                float newWidth = 150f;
                float newHeight = 270f;
                Vector2 sizeDelta = yesRectTransform.sizeDelta;
                sizeDelta.x = newWidth;
                sizeDelta.y = newHeight;
                yesRectTransform.sizeDelta = sizeDelta;
            }
            if (noRectTransform != null)
            {
                float newWidth = 75f;
                float newHeight = 175f;
                Vector2 sizeDelta = noRectTransform.sizeDelta;
                sizeDelta.x = newWidth;
                sizeDelta.y = newHeight;
                noRectTransform.sizeDelta = sizeDelta;
            }
        }

        if(mouseWorldPosition.x < -1)
        {
            
            if (yesRectTransform != null)
            {
                float newWidth = 75f;
                float newHeight = 135f;
                Vector2 sizeDelta = yesRectTransform.sizeDelta;
                sizeDelta.x = newWidth;
                sizeDelta.y = newHeight;
                yesRectTransform.sizeDelta = sizeDelta;
            }
            if (noRectTransform != null)
            {
                float newWidth = 150f;
                float newHeight = 350f;
                Vector2 sizeDelta = noRectTransform.sizeDelta;
                sizeDelta.x = newWidth;
                sizeDelta.y = newHeight;
                noRectTransform.sizeDelta = sizeDelta;
            }
        }
        
    }



}