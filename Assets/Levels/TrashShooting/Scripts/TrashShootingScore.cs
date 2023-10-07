using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TrashShooting.TrashShootingScore;
using static UnityEngine.GraphicsBuffer;

namespace TrashShooting
{
    public class TrashShootingScore : MonoBehaviour
    {
        [SerializeField] SpriteRenderer[] HappyBins = null;
        [SerializeField] SpriteRenderer[] AngryBins = null;
        [SerializeField] AudioClip aPerfect = null;
        [SerializeField] AudioClip aNormal = null;
        [SerializeField] AudioClip aMiss = null;
        AudioSource audioSource;

        const float c_UnitScore = 100.0f;
        const float c_PerfectMulti = 1.5f;
        RollingNumberImage scoreImage = null;
        RollingNumberImage comboImage = null;

        float[] scores = { 0.0f, 0.0f, 0.0f };
        float score = 0;
        float combo = 0;

        float[] HappyEmojiDeadline = { -1.0f, -1.0f, -1.0f };
        float[] AngryEmojiDeadline = { -1.0f, -1.0f, -1.0f };

        public struct ScoreStatic
        {
            public uint maxCombo;
            public uint notes;
            public uint perfect;
            public uint normal;
            public uint miss;
            public uint[] perfects;
            public uint[] normals;
            public uint[] misses;

        }

        List<ScoreStatic> scoreStatics {  get; set; }
        ScoreStatic curStatics = new ScoreStatic
        {
            maxCombo = 0,
            notes = 0,
            perfect = 0,
            normal = 0,
            miss = 0,
            perfects = new uint[4] { 0, 0, 0, 0 },
            normals = new uint[4] { 0, 0, 0, 0 },
            misses = new uint[4] { 0, 0, 0, 0 }
        };

        //
        public ScoreStatic PhaseEnd()
        {
            scoreStatics.Add(curStatics);

            curStatics = new ScoreStatic
            {
                maxCombo = 0,
                notes = 0,
                perfect = 0,
                normal = 0,
                miss = 0,
                perfects = new uint[4] { 0, 0, 0, 0 },
                normals = new uint[4] { 0, 0, 0, 0 },
                misses = new uint[4] { 0, 0, 0, 0 }
            };
            return scoreStatics[scoreStatics.Count - 1];
        }



        public void Shoot(EShootType _shootType, int _target)
        {
            bool bWin = false;
            bool bPerfect = false;
            switch (_shootType)
            {
                case EShootType.Perfect:
                case EShootType.TrapPerfect:
                    bPerfect = true;
                    bWin = true;
                    break;
                case EShootType.Normal:
                    bWin = true;
                    break;
            }

            if (bWin)
            {
                if (_target >= 0)
                {
                    var addScore = GetComboMulti(combo) * c_UnitScore;
                    if (bPerfect)
                    {
                        addScore *= c_PerfectMulti;

                        curStatics.perfect++;
                        curStatics.perfects[_target]++;
                        curStatics.notes++;
                        audioSource.clip = aPerfect;
                    }
                    else
                    {
                        curStatics.normal++;
                        curStatics.normals[_target]++;
                        curStatics.notes++;
                        audioSource.clip = aNormal;
                    }

                    SetEmoji(_target, true);
                    SetScore(score + addScore);
                    SetCombo(combo + 1);
                }
            }
            else
            {
                curStatics.miss++;
                curStatics.misses[_target]++;
                curStatics.notes++;
                audioSource.clip = aMiss;

                SetEmoji(_target, false);
                SetCombo(0);
            }
            audioSource.Play();

        }

        public void Miss(int _target)
        {
            curStatics.miss++;
            curStatics.misses[_target]++;
            curStatics.notes++;

            SetCombo(0);
        }

        public void TrapPerfect()
        {
            curStatics.miss++;
            curStatics.misses[3]++;
            curStatics.notes++;

            SetCombo(combo + 1);
        }



        // Start is called before the first frame update
        void Start()
        {
            scoreImage = transform.Find("rollingScore").GetComponent<RollingNumberImage>();
            comboImage = transform.Find("rollingCombo").GetComponent<RollingNumberImage>();

            SetScore(score);
            SetCombo(0);

            for(int i = 0; i < 3; ++i)
            {
                HappyBins[i].gameObject.SetActive(false);
                AngryBins[i].gameObject.SetActive(false);
            }

            scoreStatics = new List<ScoreStatic>();

            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateEmojiState();
        }

        float GetComboMulti(float _combo)
        {
            return Mathf.Clamp(_combo, 0.0f, 500.0f) / 100.0f + 1.0f; // [0,5]
        }

        void SetScore(float _score)
        {
            linearValue(ref score, _score, scoreImage.gameObject);
        }

        void SetCombo(float _combo)
        {
            curStatics.maxCombo = Mathf.RoundToInt(_combo) > curStatics.maxCombo ? (uint)Mathf.RoundToInt(_combo) : curStatics.maxCombo;
            linearValue(ref combo, _combo, comboImage.gameObject);
        }

        void SetEmoji(int _target, bool bHappy)
        {
            const float c_lastTime = 1.0f;
            if (bHappy)
                HappyEmojiDeadline[_target] = Time.time + c_lastTime;
            else
                AngryEmojiDeadline[_target] = Time.time + c_lastTime;            
        }

        void UpdateEmojiState()
        {
            for(int i = 0; i < 3; ++i)
            {
                HappyBins[i].gameObject.SetActive(HappyEmojiDeadline[i] >= Time.time);
                AngryBins[i].gameObject.SetActive(AngryEmojiDeadline[i] >= Time.time);
            }
            
        }

        void linearValue(ref float target, float value, GameObject go)
        {
            var oldValue = target;
            target = value;


            iTween.ValueTo(go, iTween.Hash(
                "from", oldValue,
                "to", target,
                "time", Mathf.Pow((Mathf.Abs(target - oldValue) / 100.0f), 0.5f),
                "onupdate", "UpdateValue",
                "onupdatetarget", go,
                "easetype", iTween.EaseType.linear
            ));
        }
    }

}