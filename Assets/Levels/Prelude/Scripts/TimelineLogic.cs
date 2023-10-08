using Fungus;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AntiSkipFrameTimelineControl
{
    static public void Update(PlayableDirector playableDirector)
    {
        if (playableDirector.timeUpdateMode != DirectorUpdateMode.Manual)
        {
            return;
        }
        if (playableDirector.state != PlayState.Playing)
            return;
        var deltaTime = Time.deltaTime;
        var targetTime = playableDirector.time + deltaTime;
        const double frameSpan = 1.0 / 60.0;
        for (double time = playableDirector.time; time < targetTime; time += frameSpan)
        {
            playableDirector.time = time;
            playableDirector.Evaluate();
            if (playableDirector.state != PlayState.Playing)
                return;
        }

        if (Math.Floor(playableDirector.time / frameSpan) < Math.Floor(targetTime / frameSpan))
        {
            playableDirector.time = targetTime;
            playableDirector.Evaluate();
            if (playableDirector.state != PlayState.Playing)
                return;
        }
        else
        {
            playableDirector.time = targetTime;
        }


        if (targetTime > playableDirector.duration)
        {
            playableDirector.time = playableDirector.duration;
            playableDirector.Stop();
        }
    }
}
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

    enum EWait
    {
        WaitingInput,
        WaitingWriterStop,
        NoWaiting
    }

    EWait eWaitingInput = EWait.NoWaiting;
    float timeDiff = 0.001f;
    CommonInputAction.EType eType = CommonInputAction.EType.None;
    internal void OnTimelineWaitingInput(CommonInputAction.EType eType, float timeDiff = 0.001f)
    {
        GetComponent<PlayableDirector>().Pause();
        this.eType = eType;
        eWaitingInput = EWait.WaitingInput;
        this.timeDiff = timeDiff;
    }

    //LinkedList<CutsceneBehaviour>
    Writer writer = null;
    // Start is called before the first frame update
    void Start()
    {
        Writer[] writers = GameObject.FindObjectsOfType<Writer>(true);
        if (writers.Length >= 1)
        {
            writer = writers[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        AntiSkipFrameTimelineControl.Update(GetComponent<PlayableDirector>());


        switch(eWaitingInput)
        {
            case EWait.WaitingInput:
                if (GameManager.Instance.CommonInputAction.GetPerformedTypeThisFrame() == eType)
                {
                    if (writer == null || !(writer.IsWriting || writer.IsWaitingForInput))
                    {
                        //no deal with writer
                        CleanWaitingAndResumeTimeline();
                    }
                    else
                    {

                        writer.OnNextLineEvent();
                        eWaitingInput = EWait.WaitingWriterStop;
                        eType = CommonInputAction.EType.None;
                    }
                }
                break;
            case EWait.WaitingWriterStop:
                if(writer == null || !(writer.IsWriting || writer.IsWaitingForInput))
                {
                    //no deal with writer
                    CleanWaitingAndResumeTimeline();
                }

                break;
            default: 
                break;
        }

    }

    void CleanWaitingAndResumeTimeline()
    {
        eWaitingInput = EWait.NoWaiting;
        eType = CommonInputAction.EType.None;

        var dir = GetComponent<PlayableDirector>();
        dir.time += timeDiff;
        dir.Resume();
    }
}
