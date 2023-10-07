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

    public AudioSource selectAudio;

    private bool pastSelection;

    bool currentSelection = true;

    void Start()
    {
        selectAudio.Stop();
        
    }

    public void Update()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        if(Input.GetAxis("Horizontal") < 0)
        {
            currentSelection = false;
        }
        if(Input.GetAxis("Horizontal") > 0)
        {
            currentSelection = true;
        }




        
        if(currentSelection == true)
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

        if(currentSelection == false)
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


        if(currentSelection == true && pastSelection == false)
        {
            selectAudio.Play();
        }
        if(currentSelection == false && pastSelection == true)
        {
            selectAudio.Play();
        }

        pastSelection = currentSelection;
    }



}