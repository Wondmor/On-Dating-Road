using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class WaitForInputPlayable : BasicPlayableBehaviour
{
    public override double duration => 1 / 300.0f;

    [Header("等输入")]
    public CommonInputAction.EType eAction;


    [Header("主控件")]
    public ExposedReference<TimelineLogic> timelineLogic;

    private TimelineLogic _timelineLogic;


    private bool _bUsed = false;
    public override void OnGraphStart(Playable playable)
    {
        _timelineLogic = timelineLogic.Resolve(playable.GetGraph().GetResolver());

    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (!_bUsed)
        {
            _bUsed = true;
            _timelineLogic.OnTimelineWaitingInput(eAction);
        }
    }
}