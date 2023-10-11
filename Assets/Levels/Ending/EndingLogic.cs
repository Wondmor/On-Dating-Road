using Fungus;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Video;
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
    [SerializeField] PlayableDirector NicePart = null;
    [SerializeField] AudioClip BeLateBGM = null;
    [SerializeField] AudioClip NormalBGM = null;
    [SerializeField] AudioMixerGroup audioMixerGroupBGM = null;
    [SerializeField] AudioSource BlendBGM = null;
    [SerializeField] ToBeContinueLogic toBeContinue = null;

    public struct GiftInfo
    {
        public string name;
        public Sprite sprite;
    }


    [SerializeField] Sprite arcade = null;
    [SerializeField] Sprite candle = null;
    [SerializeField] Sprite rooster = null;

    [SerializeField] VideoPlayer player;
    [SerializeField] Flowchart flowchart;

    List<PlayableDirector> toPlay = new List<PlayableDirector>();
    GiftInfo getGift = new GiftInfo();
    GiftInfo giveGift = new GiftInfo();

    int playingTimelineIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        BlendBGM.outputAudioMixerGroup = audioMixerGroupBGM;
        BlendBGM.loop = true;

        var endingData = GameLogicManager.Instance.endingData;

        //var dummyEndingData = new EndingData();
        //dummyEndingData.eEnding = EEnding.BadCharacter;
        //dummyEndingData.eGift = EGift.Free;
        //var dummyGiftInfo = new ShopItem();
        //dummyGiftInfo.sprite = "19";
        //dummyGiftInfo.giftname = "传单折的千纸鹤";
        //dummyEndingData.giftInfo = dummyGiftInfo;

        //endingData = dummyEndingData;


        if (endingData.eEnding == EEnding.BeLate)
        {
            BlendBGM.clip = BeLateBGM;

            toPlay.Add(BeLate);
            BeLate.gameObject.SetActive(true);
        }
        else
        {
            BlendBGM.clip = NormalBGM;

            giveGift.sprite = Resources.Load<Sprite>(String.Format("Shopping/Sprites/{0}", endingData.giftInfo.sprite));
            giveGift.name = endingData.giftInfo.giftname;

            toPlay.Add(JustArrived);

            switch(endingData.eGift)
            {
                case EGift.Free:
                    toPlay.Add(FreeGift);
                    getGift.sprite = arcade;
                    getGift.name = "Taito Egret mini桌面游戏机套装";
                    break;
                case EGift.Bad:
                    toPlay.Add(endingData.eEnding== EEnding.GoodCharacter ? 
                        NiceCheapGift:BadCheapGift);
                    if(endingData.eEnding == EEnding.GoodCharacter)
                    {
                        toPlay.Add(NicePart);
                        getGift.sprite = candle;
                        getGift.name = "高级香薰蜡烛";
                    }
                    else
                    {
                        getGift.sprite = rooster;
                        getGift.name = "铁皮小公鸡";
                    }
                    break;
                case EGift.Normal:
                    toPlay.Add(endingData.eEnding == EEnding.GoodCharacter ?
                        NiceNormalGift : BadNormalGift);
                    if (endingData.eEnding == EEnding.GoodCharacter)
                    {
                        toPlay.Add(NicePart);
                    }
                    getGift.sprite = giveGift.sprite;
                    getGift.name = giveGift.name;
                    break;
                case EGift.Good:
                    toPlay.Add(endingData.eEnding == EEnding.GoodCharacter ?
                        NiceGoodGift : BadGoodGift);
                    if (endingData.eEnding == EEnding.GoodCharacter)
                    {
                        toPlay.Add(NicePart);
                    }
                    getGift.sprite = arcade;
                    getGift.name = "Taito Egret mini桌面游戏机套装";
                    break;
            }
        }

        toPlay[playingTimelineIdx].stopped += OnTimelineStop;

        BlendBGM.Play();
        toPlay[playingTimelineIdx].Play();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnTimelineStop(PlayableDirector _dir)
    {
        if (playingTimelineIdx < toPlay.Count)
        {
            ++playingTimelineIdx;
            if (playingTimelineIdx < toPlay.Count)
            {
                toPlay[playingTimelineIdx].stopped += OnTimelineStop;
                toPlay[playingTimelineIdx].Play();
            }
            else
                OnFinished();
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
        flowchart.ExecuteIfHasBlock("Video");
    }

    public void VideoPlay()
    {
        player.url = System.IO.Path.Combine(Application.streamingAssetsPath, "ending.mp4");
        player.Play();
        player.loopPointReached += (vp) =>
        {
            Debug.Log("Play stop");
            FungusManager.Instance.MusicManager.SetAudioVolume(0, 2f, () =>
            {
                FungusManager.Instance.MusicManager.StopMusic();
                gameObject.SetActive(true);
                toBeContinue.gameObject.SetActive(true);
            });
        };
        gameObject.SetActive(false);
    }
}
