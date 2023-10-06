using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class NextTimelinePlayable : BasicPlayableBehaviour
{
    public override double duration => 1 / 300.0f;
    [SerializeField] UnityEvent<string> OnTimelineFinish;
    [SerializeField] string timelineName="";
    public override void OnGraphStart(Playable playable)
    {

    }
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        OnTimelineFinish?.Invoke(timelineName);
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }
}
