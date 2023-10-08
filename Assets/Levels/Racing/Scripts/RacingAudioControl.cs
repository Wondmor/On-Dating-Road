using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingAudioControl : MonoBehaviour
{
    AudioSource driveAudio;
    AudioSource sfx;
    AudioSource sfx2;
    AudioSource bgm;

    [SerializeField]
    AudioClip countDown, driveNormal, driveGrass, hitBike, hitCat, hitShit, hit, rotate, finish, speedup, coin;

    [SerializeField, Range(0, 1)]
    float driveAudioSound, sfxSound;
    // Start is called before the first frame update
    void Awake()
    {
        bgm = GetComponent<AudioSource>();

        driveAudio = gameObject.AddComponent<AudioSource>();
        sfx = gameObject.AddComponent<AudioSource>();
        sfx2 = gameObject.AddComponent<AudioSource>();

        driveAudio.playOnAwake = false;
        driveAudio.loop = true;
        driveAudio.volume = 0.7f;

        driveAudio.Stop();

        sfx.playOnAwake = false;
        sfx2.playOnAwake = false;
    }

    public void PlayCountDown()
    {
        sfx.clip = countDown;
        sfx.Play();
    }

    public void PlayDrive(bool onGrass)
    {
        if (onGrass)
        {
            driveAudio.clip = driveGrass;
        }
        else
        {
            driveAudio.clip = driveNormal;
        }
        if (!driveAudio.isPlaying)
        {
            driveAudio.Play();
        }
    }

    public void StopPlayDrive()
    {
        driveAudio.Stop();
    }

    public void PlayHit()
    {
        sfx.clip = hit;
        sfx.Play();
        sfx2.clip = this.rotate;
        sfx2.Play(10000);
    }

    public void PlayHitBike(bool rotate)
    {
        sfx.clip = hitBike;
        sfx.Play();
        if (rotate)
        {
            sfx2.clip = this.rotate;
            sfx2.Play(10000);
        }
    }
    public void PlayHitCat()
    {
        sfx.clip = hitCat;
        sfx.Play();
        sfx2.clip = hit;
        sfx2.Play();
    }

    public void PlayHitShit()
    {
        sfx.clip = hitShit;
        sfx.Play();
        sfx2.clip = this.rotate;
        sfx2.Play(10000);
    }

    public void PlaySpeedup()
    {
        sfx.clip = speedup;
        sfx.Play();
    }

    public void PlayFinish()
    {
        sfx.clip = finish;
        sfx.Play();
    }

    public void BGMVolumeDown(float time = 1.0f)
    {
        iTween.ValueTo(gameObject, iTween.Hash(
                    "from", 0.8f,
                    "to", 0.0f,
                    "time", time,
                    "onupdate", "UpdateBGMToLow",
                    "onupdatetarget", gameObject,
                    "easetype", iTween.EaseType.linear
            ));
    }

    public void UpdateBGMToLow(float value)
    {
        bgm.volume = value;
    }

    public void PlayCoin()
    {
        sfx.clip = coin;
        sfx.Play();
    }
}
