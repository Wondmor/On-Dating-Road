using System.Collections;
using System.Collections.Generic;
using Fungus;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour
{
    public RectTransform yesRectTransform;
    public RectTransform noRectTransform;

    public Camera mainCamera;

    public Image yesImage;
    public Image noImage;

    public Sprite yesBefore;
    public Sprite yesAfter;
    public Sprite noBefore;
    public Sprite noAfter;

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

                yesImage.sprite = yesAfter;
            }
            if (noRectTransform != null)
            {
                float newWidth = 75f;
                float newHeight = 175f;
                Vector2 sizeDelta = noRectTransform.sizeDelta;
                sizeDelta.x = newWidth;
                sizeDelta.y = newHeight;
                noRectTransform.sizeDelta = sizeDelta;

                noImage.sprite = noBefore;
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

                yesImage.sprite = yesBefore;
            }
            if (noRectTransform != null)
            {
                float newWidth = 150f;
                float newHeight = 350f;
                Vector2 sizeDelta = noRectTransform.sizeDelta;
                sizeDelta.x = newWidth;
                sizeDelta.y = newHeight;
                noRectTransform.sizeDelta = sizeDelta;

                noImage.sprite = noAfter;
            }
        }
        
    }



}