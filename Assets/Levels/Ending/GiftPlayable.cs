using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GiftPlayable : BasicPlayableBehaviour
{
    public override double duration => 1/300.0f;

    [SerializeField] bool bGet = true;
    public ExposedReference<EndingLogic> endingLogic;
    public ExposedReference<EndingGift> endingGift;


    EndingLogic _endingLogic;
    EndingGift _endingGift;

    public override void OnGraphStart(Playable playable)
    {
        _endingLogic = endingLogic.Resolve(playable.GetGraph().GetResolver());
        _endingGift = endingGift.Resolve(playable.GetGraph().GetResolver());
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        Sprite sprite = null;

        string text = "得到礼物 ";
        if(!bGet)
            text = "送出 ";
        string giftName = string.Empty;
        _endingLogic.GetGiftInfo(bGet, ref sprite, ref giftName);
        text += giftName;
        _endingGift.gameObject.SetActive(true);
        _endingGift.SetGift(sprite, text);
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }

}
