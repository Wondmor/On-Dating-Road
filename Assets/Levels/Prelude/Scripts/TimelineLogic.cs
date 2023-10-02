using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimelineLogic : MonoBehaviour
{
    public delegate void OnTimelineFinishCallback();
    public OnTimelineFinishCallback OnTimelineFinish = ()=>{ };

    [Header("结束后跳转到")]
    [SerializeField] public string sceneName = "";

    internal void OnTimelineLastFrame()
    {
        GetComponent<PlayableDirector>().Pause();
        if (transform.Find("CanvasFadeOut") && transform.Find("CanvasFadeOut").GetComponent<FadeInOutScene>())
        {
            var fadeOut = transform.Find("CanvasFadeOut").GetComponent<FadeInOutScene>();
            fadeOut.gameObject.SetActive(true);
            fadeOut.FadeOut(gameObject, "InvokeOnTimelineFinish");
        }
        else
        {
            InvokeOnTimelineFinish();
            //SceneManager.LoadScene(sceneName);

        }
    }

    void InvokeOnTimelineFinish()
    {
        OnTimelineFinish();
    }

    //internal void OnFadeOutComplete()
    //{
    //    var eState = MainMgr.GetInstance().GetCurState();
    //    switch(eState)
    //    {
    //        case GameStateMachine.EState.Prelude:
    //            MainMgr.GetInstance().OnPreludeFinished();
    //            break;
    //        case GameStateMachine.EState.OnDatingRoad:
    //            MainMgr.GetInstance().OnBridgeFinished();
    //            break;
    //        default:
    //            throw new Exception("wrong state");
    //    }
    //}

    bool bWaitingInput = false;
    CommonInputAction.EType eType = CommonInputAction.EType.None;
    internal void OnTimelineWaitingInput(CommonInputAction.EType eType)
    {
        GetComponent<PlayableDirector>().Pause();
        this.eType = eType;
        bWaitingInput = true;
    }

    //LinkedList<CutsceneBehaviour>

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(bWaitingInput)
        {
            if (GameManager.Instance.CommonInputAction.GetPerformedTypeThisFrame() == eType)
            {
                bWaitingInput = false;
                eType = CommonInputAction.EType.None;

                var dir = GetComponent<PlayableDirector>();
                dir.time += 0.001;
                dir.Play();
                //GetComponent<PlayableDirector>().Play();
            }
        }

    }
}
