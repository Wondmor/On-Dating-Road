using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CommonInputAction : MonoBehaviour
{
    public InputAction enter;
    public InputAction cancel;
    public InputAction directions;

    public enum EType
    {
        Enter,
        Cancel,
        Directions,
        None
    }


    public void Awake()
    {
        // assign a callback for the "jump" action.
        //enter.performed += ctx => { Debug.LogFormat("enterPerformed"); };
        //cancel.performed += ctx => { Debug.LogFormat("cancelPerformed"); };
        //directions.performed += ctx => { Debug.LogFormat("directionsPerformed"); };
    }

    public void OnEnable()
    {
        enter.Enable();
        cancel.Enable();
        directions.Enable();
    }

    public void OnDisable()
    {
        enter.Disable();
        cancel.Disable();
        directions.Disable();
    }

    public EType GetPerformedTypeThisFrame()
    {
        if(enter.WasPerformedThisFrame())
            return EType.Enter;
        else if(cancel.WasPerformedThisFrame())
            return EType.Cancel;
        else if(directions.WasPerformedThisFrame())
            return EType.Directions;
        else return EType.None;
    }
}
