using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using static Shopping;

public class EndingLogic : MonoBehaviour
{
    [Header("Timelines")]
    [SerializeField] PlayableDirector BeLate = null;
    [SerializeField] PlayableDirector JustArrived = null;
    [SerializeField] PlayableDirector FreeGift = null;
    [SerializeField] PlayableDirector BadCheapGift = null;
    [SerializeField] PlayableDirector NiceCheapGift = null;
    [SerializeField] PlayableDirector BadNormalGift = null;
    [SerializeField] PlayableDirector NiceNormalGift = null;
    [SerializeField] PlayableDirector BadGoodGift = null;
    [SerializeField] PlayableDirector NiceGoodGift = null;

    public struct GiftInfo
    {
        public string name;
        public Sprite sprite;
    }

    [Header("礼物")]
    [SerializeField] GiftInfo[] gifts = null;

    List<PlayableDirector> toPlay = new List<PlayableDirector>();
    GiftInfo getGift = new GiftInfo();
    GiftInfo giveGift = new GiftInfo();

    int playingTimelineIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        var endingData = GameLogicManager.Instance.endingData;

        var dummyEndingData = new EndingData();
        dummyEndingData.eEnding = EEnding.BadCharacter;
        dummyEndingData.eGift = EGift.Free;
        var dummyGiftInfo = new ShopItem();
        dummyGiftInfo.sprite = "19";
        dummyGiftInfo.giftname = "传单折的千纸鹤";
        dummyEndingData.giftInfo = dummyGiftInfo;

        endingData = endingData;


        if (endingData.eEnding == EEnding.BeLate)
        {
            toPlay.Add(BeLate);
            BeLate.gameObject.SetActive(true);
        }
        else
        {
            giveGift.sprite = Resources.Load<Sprite>(String.Format("Shopping/Sprites/{0}", endingData.giftInfo.sprite));
            giveGift.name = endingData.giftInfo.giftname;

            toPlay.Add(JustArrived);

            switch(endingData.eGift)
            {
                case EGift.Free:
                    toPlay.Add(FreeGift);
                    getGift.sprite = Resources.Load<Sprite>("Assets/Levels/Ending/Textures/礼物-限量版桌面mini街机.png");
                    getGift.name = "Taito Egret mini桌面游戏机套装";
                    break;
                case EGift.Bad:
                    toPlay.Add(endingData.eEnding== EEnding.GoodCharacter ? 
                        NiceCheapGift:BadCheapGift);
                    if(endingData.eEnding == EEnding.GoodCharacter)
                    {
                        getGift.sprite = Resources.Load<Sprite>("Assets/Levels/Ending/Textures/礼物-高级香薰蜡烛.png");
                        getGift.name = "高级香薰蜡烛";
                    }
                    else
                    {
                        getGift.sprite = Resources.Load<Sprite>("Assets/Levels/Ending/Textures/礼物-铁皮小公鸡.png");
                        getGift.name = "铁皮小公鸡";
                    }
                    break;
                case EGift.Normal:
                    toPlay.Add(endingData.eEnding == EEnding.GoodCharacter ?
                        NiceNormalGift : BadNormalGift);
                    getGift.sprite = giveGift.sprite;
                    getGift.name = giveGift.name;
                    break;
                case EGift.Good:
                    toPlay.Add(endingData.eEnding == EEnding.GoodCharacter ?
                        NiceGoodGift : BadGoodGift);
                    getGift.sprite = Resources.Load<Sprite>("Assets/Levels/Ending/Textures/礼物-限量版桌面mini街机.png");
                    getGift.name = "Taito Egret mini桌面游戏机套装";
                    break;
            }
        }


        toPlay[0].Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (playingTimelineIdx < toPlay.Count) 
        {
            if (!toPlay[playingTimelineIdx].playableGraph.IsValid())
            {
                ++playingTimelineIdx;
                if (playingTimelineIdx < toPlay.Count)
                    toPlay[playingTimelineIdx].Play();
                else
                    OnFinished();
            }
        }
    }


    public void GetGiftInfo(bool bGet, ref Sprite sprite, ref string giftName)
    {
        if(bGet)
        {
            sprite = getGift.sprite;
            giftName = getGift.name;
        }
        else
        {
            sprite = giveGift.sprite;
            giftName = giveGift.name;
        }
    }

    public void OnFinished()
    {
        if (GameManager.Instance.CommonInputAction.enter.WasPerformedThisFrame())
            GameLogicManager.Instance.OnEndingFinished();
    }
}
