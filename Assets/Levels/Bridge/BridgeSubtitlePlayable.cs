using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class BridgeSubtitlePlayable : BasicPlayableBehaviour
{
    public override double duration => 1 / 300.0f;
[Header("字幕")]
public ExposedReference<SaySubtitle> subtitleBG;
private SaySubtitle _subtitleBG;
[Multiline(3)]
public string text;

public override void OnGraphStart(Playable playable)
{
    _subtitleBG = subtitleBG.Resolve(playable.GetGraph().GetResolver());
}

public override void OnBehaviourPlay(Playable playable, FrameData info)
{
    _subtitleBG.gameObject.SetActive(true);
    string line = GameLogicManager.Instance.bridgeData.subtitle;
    _subtitleBG.Say(String.Format(text, line));
}

public override void OnBehaviourPause(Playable playable, FrameData info)
{
}
}
