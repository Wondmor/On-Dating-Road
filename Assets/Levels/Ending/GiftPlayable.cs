using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GiftPlayable : BasicPlayableBehaviour
{
    public override double duration => 1/300.0f;

    [SerializeField] bool bGet = true;
    [SerializeField] EndingLogic endingLogic = null;
    [SerializeField] EndingGift endingGift = null;

    public override void OnGraphStart(Playable playable)
    {
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        Sprite sprite = null;

        string text = "得到礼物 ";
        if(!bGet)
            text = "送出 ";
        string giftName = string.Empty;
        endingLogic.GetGiftInfo(bGet, ref sprite, ref giftName);
        text += giftName;
        endingGift.SetGift(sprite, text);
        endingGift.gameObject.SetActive(true);
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }

}
