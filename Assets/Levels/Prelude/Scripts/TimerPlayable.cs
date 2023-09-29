using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimerPlayable : BasicPlayableBehaviour
{
    [Header("设置倒计时")]
    public ExposedReference<PhoneTimer> phoneTimer;
    public uint secondCount;

    private PhoneTimer _phoneTimer;

    public override void OnGraphStart(Playable playable)
    {
        _phoneTimer = phoneTimer.Resolve(playable.GetGraph().GetResolver()).GetComponent<PhoneTimer>();
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        _phoneTimer.totalTime = secondCount;
        _phoneTimer.bStart = true;
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }
}
