using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;

public class DeliveryPersonFallGameResultMenu : MonoBehaviour
{
    public InputActionAsset inputActionAsset;

    private InputAction continueAction;
    Subject<Unit> onHideFinished = new();

    public IObservable<Unit> OnHideFinished => onHideFinished;

    private void Awake()
    {
        continueAction = inputActionAsset.FindAction("Continue");
    }

    private void HideAndContinue(InputAction.CallbackContext _)
    {
        gameObject.SetActive(false);
        onHideFinished.OnNext(Unit.Default);
    }

    public void OnEnable()
    {
        continueAction.Enable();
        continueAction.performed += HideAndContinue;
    }

    public void OnDisable()
    {
        continueAction.Disable();
        continueAction.performed -= HideAndContinue;
    }
}
