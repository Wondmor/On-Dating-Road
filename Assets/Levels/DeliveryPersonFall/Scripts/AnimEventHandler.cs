using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class AnimEventHandler : MonoBehaviour
{
    public event Action<string> OnEventTriggeredByString;

    public Subject<string> EventTriggeredStream { get; private set; } = new Subject<string>();

    private void Awake()
    {
        var animator = GetComponent<Animator>();
        if (animator == null)
            return;
        //var handlerArray = animator.GetBehaviours<AnimStateEventHandler>();
        //foreach (var animStateEvtHandler in handlerArray)
        //{
        //    animStateEvtHandler.OnStateEnterAction = OnAnimStateEnter;
        //    animStateEvtHandler.OnStateExitAction = OnAnimStateExit;
        //}
    }

    public void AnimEvt_OnEventTriggeredByString(string _eventName)
    {
        OnEventTriggeredByString?.Invoke(_eventName);
        EventTriggeredStream?.OnNext(_eventName);
    }

    //public void OnAnimStateEnter(Animator animator, AnimatorStateInfo stateInfo, string stateName, int layerIndex)
    //{
    //    OnEventTriggeredByString?.Invoke($"Enter_{stateName}");
    //}

    //public void OnAnimStateExit(Animator animator, AnimatorStateInfo stateInfo, string stateName, int layerIndex)
    //{
    //    OnEventTriggeredByString?.Invoke($"Exit_{stateName}");
    //}
}
