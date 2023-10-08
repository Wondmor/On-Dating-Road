using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class AudioSourceControlPlayable : BasicPlayableBehaviour
{
    public override double duration => 1 / 300.0f;
    [Header("BGM & SFX")]
    public ExposedReference<AudioSource> audioSource;
    public ExposedReference<AudioClip> audioClip;
    [SerializeField] bool bStart = false;
    [SerializeField] bool bStop = false;
    [SerializeField] bool bLoop = false;
    [SerializeField] bool bVolume = false;
    [SerializeField] float volume = 1.0f;


    private AudioSource _audioSource;
    private AudioClip _audioClip;

    public override void OnGraphStart(Playable playable)
    {
        _audioSource = audioSource.Resolve(playable.GetGraph().GetResolver());
        _audioClip = audioClip.Resolve(playable.GetGraph().GetResolver());
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if(_audioClip != null)
        {
            _audioSource.clip = _audioClip;
        }

        _audioSource.loop = bLoop;

        if(bVolume)
        {
            _audioSource.volume = volume;
        }

        if (bStart)
        {
            _audioSource.Play();
        }
        if (bStop)
        {
            _audioSource.Stop();
        }

    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }
}
