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
    public ExposedReference<Image> subtitleBG;
    private Image _subtitleBG;
    private TextMeshProUGUI _subtitle;
    private EndingLogic _endingLogic;
    [Multiline(3)]
    public string text;

    public override void OnGraphStart(Playable playable)
    {
        _endingLogic = endingLogic.Resolve(playable.GetGraph().GetResolver());
        _subtitleBG = subtitleBG.Resolve(playable.GetGraph().GetResolver());
        _subtitle = _subtitleBG.transform.Find("Subtitle").GetComponent<TextMeshProUGUI>();
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        _subtitleBG.gameObject.SetActive(true);
        _subtitle.gameObject.SetActive(true);
        Sprite sprite = null;
        string giftname = "";
        _endingLogic.GetGiftInfo(false, ref sprite, ref giftname);
        _subtitle.text = String.Format(text, giftname);
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }
}
