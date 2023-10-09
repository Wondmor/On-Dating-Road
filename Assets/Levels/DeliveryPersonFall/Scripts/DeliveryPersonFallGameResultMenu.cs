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
using TMPro;

public class DeliveryPersonFallGameResultMenu : MonoBehaviour
{
    public InputActionAsset inputActionAsset;
    public TextMeshProUGUI transactionPriceTxt;

    private InputAction continueAction;
    Subject<Unit> onHideFinished = new();

    public IObservable<Unit> OnHideFinished => onHideFinished;

    private void Awake()
    {
        continueAction = inputActionAsset.FindAction("Continue");
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

    private void HideAndContinue(InputAction.CallbackContext _)
    {
        gameObject.SetActive(false);
        onHideFinished.OnNext(Unit.Default);
    }

    public void Show(float _value)
    {
        transactionPriceTxt.text = $"+{_value}元";
        gameObject.SetActive(true);
    }
}
