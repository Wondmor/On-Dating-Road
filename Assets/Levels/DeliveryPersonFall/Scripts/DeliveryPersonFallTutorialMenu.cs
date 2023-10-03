using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeliveryPersonFallTutorialMenu : MonoBehaviour
{
    public InputActionAsset inputActionAsset;

    public IObservable<Unit> OnHide => onHide;

    Subject<Unit> onHide = new();

    private void OnEnable()
    {
        inputActionAsset.Enable();
        inputActionAsset.FindAction("Continue").performed += Continue;
    }

    private void OnDisable()
    {
        inputActionAsset.Disable();
        inputActionAsset.FindAction("Continue").performed -= Continue;
    }

    void Continue(InputAction.CallbackContext _)
    {
        onHide.OnNext(Unit.Default);
        gameObject.SetActive(false);
    }

    public void ShowMenu()
    {
        gameObject.SetActive(true);
    }
}
