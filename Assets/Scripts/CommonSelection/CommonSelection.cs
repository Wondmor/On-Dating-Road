using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CommonSelection : MonoBehaviour
{
    [SerializeField]
    string trueString, falseString, descriptionString;

    [SerializeField]
    TextMeshProUGUI descriptionTime, choice;

    [SerializeField]
    UnityEvent<bool> OnSelection;

    GameObject canvas;

    CommonInputAction myInput;

    bool currentSelection = true;

    // Start is called before the first frame update
    void Start()
    {
        canvas = transform.Find("Canvas").gameObject;
        canvas.SetActive(false);
        myInput = canvas.GetComponent<CommonInputAction>();
        ShowChoice();
    }

    public void ShowChoice()
    {
        canvas.SetActive(true);
        GameManager.Instance.CommonInputAction.enabled = false;

        GameObject commonInputActionPrefab = Resources.Load<GameObject>("Prefabs/CommonInputAction");

        myInput.directions.performed += OnActionDirection;
        myInput.enter.performed += OnActionEnter;

        descriptionTime.text = string.Format(descriptionString, string.Format("{0:D2}:{1:D2}", (int)GameLogicManager.Instance.gameData.countDown / 60, (int)GameLogicManager.Instance.gameData.countDown % 60));
    }

    void OnActionDirection(InputAction.CallbackContext context)
    {
        if(context.ReadValue<Vector2>().x < 0)
        {
            currentSelection = true;
            choice.text = trueString;
        }
        if(context.ReadValue<Vector2>().x > 0)
        {
            currentSelection = false;
            choice.text = falseString;
        }
    }


    void OnActionEnter(InputAction.CallbackContext context)
    {
        GameManager.Instance.CommonInputAction.enabled = true;
        OnSelection?.Invoke(currentSelection);
        canvas.SetActive(false);
        return;
    }


}
