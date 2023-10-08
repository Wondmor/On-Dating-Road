using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class GiftNameSubtitlePlayable : BasicPlayableBehaviour
{
    public override double duration => 1 / 300.0f;
    public ExposedReference<EndingLogic> endingLogic;
    [Header("字幕")]
    public ExposedReference<SaySubtitle> subtitleBG;
    private SaySubtitle _subtitleBG;
    private EndingLogic _endingLogic;
    [Multiline(3)]
    public string text;

    public override void OnGraphStart(Playable playable)
    {
        _endingLogic = endingLogic.Resolve(playable.GetGraph().GetResolver());
        _subtitleBG = subtitleBG.Resolve(playable.GetGraph().GetResolver());
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        _subtitleBG.gameObject.SetActive(true);
        Sprite sprite = null;
        string giftname = "";
        _endingLogic.GetGiftInfo(false, ref sprite, ref giftname);
        _subtitleBG.Say( String.Format(text, giftname));
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }
}
