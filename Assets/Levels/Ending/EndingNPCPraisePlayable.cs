using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Timeline;



public class EndingNPCPraisePlayable : BasicPlayableBehaviour
{
    public override double duration => 1 / 300.0f;
    [Header("NPCInfo")]
    public ExposedReference<EndingPraiseNPC> endingPraiseNPC;
    [Header("字幕")]
    public ExposedReference<SaySubtitle> subtitleBG;
    [Header("等输入")]
    public CommonInputAction.EType eAction;
    [Header("主控件")]
    public ExposedReference<TimelineLogic> timelineLogic;
    [Header("BGM")]
    public ExposedReference<AudioSource> BGMAudioSource;
    public ExposedReference<AudioSource> NPCAudioSource;


    private EndingPraiseNPC _endingPraiseNPC;
    private SaySubtitle _subtitleBG;
    private TimelineLogic _timelineLogic;
    private AudioSource _BGMAudioSource;
    private AudioSource _NPCAudioSource;

    int nextNPCIdxInOrder = 0;
    bool inNPCBGM = false;

    public override void OnGraphStart(Playable playable)
    {
        _endingPraiseNPC = endingPraiseNPC.Resolve(playable.GetGraph().GetResolver());
        _subtitleBG = subtitleBG.Resolve(playable.GetGraph().GetResolver());
        _timelineLogic = timelineLogic.Resolve(playable.GetGraph().GetResolver());
        _BGMAudioSource = BGMAudioSource.Resolve(playable.GetGraph().GetResolver());
        _NPCAudioSource = NPCAudioSource.Resolve(playable.GetGraph().GetResolver());
        
        //nextNPCIdxInOrder = 0;

        //_NPCAudioSource.Stop();

    }
    static readonly List<EScene> testData = new List<EScene> { EScene.BusGame, EScene.TrashShooting, EScene.Racing, EScene.Delivery, EScene.CoinSkill };

public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        var scenes = GameLogicManager.Instance.endingData.finishedMiniGames;//testdata
        while(nextNPCIdxInOrder < EndingPraiseNPC.Order.Length)
        {
            EScene eCurSceneInOrder = EndingPraiseNPC.Order[nextNPCIdxInOrder++];
            foreach (var scene in scenes)
            {
                if(eCurSceneInOrder == scene)
                {
                    EndingNPCInfo endingNPCInfo = new EndingNPCInfo();
                    if (_endingPraiseNPC.SetNPCAndGetInfo(scene, ref endingNPCInfo))
                    {
                        if(!inNPCBGM)
                        {
                            // switch to NPC BGM
                            _NPCAudioSource.Play();
                            _BGMAudioSource.volume = 0;
                            inNPCBGM = true;
                        }

                        _subtitleBG.gameObject.SetActive(true);
                        _subtitleBG.Say(endingNPCInfo.line);

                        _timelineLogic.OnTimelineWaitingInput(eAction, -(1.0f/300.0f));
                        return;
                    }
                }
            }
        }

        if(inNPCBGM)
        {
            var lastInSecond = 1.0f;
            _NPCAudioSource.Stop();
            //_BGMAudioSource.volume = 1;
            //fade to normal BGM
            iTween.AudioTo(_BGMAudioSource.gameObject,
                iTween.Hash("volume", 1, "time", lastInSecond));

            inNPCBGM = false;

            _endingPraiseNPC.HideAll();
        }
    }


    //IEnumerator StartFadeCoroutine(float targetVolume, float duration, AudioMixer audioMixer, string parameterName)
    //{
    //    float currentTime = 0;
    //    float startVolume;


    //    audioMixer.GetFloat(parameterName, out startVolume); // 获取当前音量

    //    while (currentTime < duration)
    //    {
    //        currentTime += Time.deltaTime;
    //        float newVolume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
    //        audioMixer.SetFloat(parameterName, newVolume);
    //        yield return null;
    //    }

    //    audioMixer.SetFloat(parameterName, targetVolume);
    //}
}
