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

    int pastSelection;

    int currentSelection = 2;

    void Start()
    {
        selectAudio.Stop();
        
    }

    void OnEnable()
    {
        currentSelection = 2;
    }

    public void Update()
    {
        //Debug.Log("currentSelection"+currentSelection);
        Vector3 mouseScreenPosition = Input.mousePosition;

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        if(Input.GetAxis("Horizontal") < 0)
        {
            currentSelection = 0;
            
        }
        if(Input.GetAxis("Horizontal") > 0)
        {
            currentSelection = 1;
            
        }

        if(currentSelection == 2)
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
                float newWidth = 75f;
                float newHeight = 175f;
                Vector2 sizeDelta = noRectTransform.sizeDelta;
                sizeDelta.x = newWidth;
                sizeDelta.y = newHeight;
                noRectTransform.sizeDelta = sizeDelta;

                noImage.sprite = noBefore;
            }
        }

        
        if(currentSelection == 1)
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

        if(currentSelection == 0)
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


        if(currentSelection == 1 && pastSelection == 0)
        {
            selectAudio.Play();
        }
        if(currentSelection == 0 && pastSelection == 1)
        {
            selectAudio.Play();
        }

        pastSelection = currentSelection;
    }



}