using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineEndPlayable : BasicPlayableBehaviour
{
    [Header("主控件")]
    public ExposedReference<TimelineLogic> timelineLogic;
    private TimelineLogic _timelineLogic;

    public override void OnGraphStart(Playable playable)
    {
        _timelineLogic = timelineLogic.Resolve(playable.GetGraph().GetResolver());
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        _timelineLogic.OnTimelineLastFrame();
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }
}