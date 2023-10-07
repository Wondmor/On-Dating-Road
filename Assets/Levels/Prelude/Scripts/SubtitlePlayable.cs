using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class SubtitlePlayable : BasicPlayableBehaviour
{
    public override double duration => 1 / 300.0f;
    [Header("字幕")]
    //public ExposedReference<Image> subtitleBG;
    public ExposedReference<SaySubtitle> subtitleBG;
    private SaySubtitle _subtitleBG;
    //private TextMeshProUGUI _subtitle;
    [Multiline(3)]
    public string text;

    public override void OnGraphStart(Playable playable)
    {
        _subtitleBG = subtitleBG.Resolve(playable.GetGraph().GetResolver());
        //_subtitle = _subtitleBG.transform.Find("Subtitle").GetComponent<TextMeshProUGUI>();
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        _subtitleBG.gameObject.SetActive(true);
        _subtitleBG.Say(text);
        //_subtitle.gameObject.SetActive(true);
        //_subtitle.text = text;
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }
}
