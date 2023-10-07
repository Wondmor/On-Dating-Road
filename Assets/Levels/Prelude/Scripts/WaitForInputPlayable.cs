using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class WaitForInputPlayable : BasicPlayableBehaviour
{
    public override double duration => 1 / 300.0f;

    [Header("等输入")]
    //public ExposedReference<CommonInputAction> input;
    public CommonInputAction.EType eAction;
    //public double StartTime;


    [Header("主控件")]
    public ExposedReference<TimelineLogic> timelineLogic;

    //private CommonInputAction _input;
    private bool _bContinue = false;
    private TimelineLogic _timelineLogic;
    //private TimelineClip clip;
    //public ExposedReference<Text> dialog;
    //private Text _dialog;
    //[Multiline(3)]
    //public string dialogStr;


    private bool _bUsed = false;
    public override void OnGraphStart(Playable playable)
    {
        //_input = input.Resolve(playable.GetGraph().GetResolver());
        _timelineLogic = timelineLogic.Resolve(playable.GetGraph().GetResolver());
        //sprite = target.Resolve(playable.GetGraph().GetResolver());

    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (!_bUsed)
        {
            _bUsed = true;
            _timelineLogic.OnTimelineWaitingInput(eAction);
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        //if (!_bContinue)
        //{
        //    var director = (playable.GetGraph().GetResolver() as PlayableDirector);
        //    director.time = StartTime+0.001f;
        //}

        //if (_input.GetPerformedTypeThisFrame() == eAction)
        //{
        //    _bContinue = true;

        //}
    }
}