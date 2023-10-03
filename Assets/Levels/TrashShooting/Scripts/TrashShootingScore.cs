using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrashShooting
{
    public class TrashShootingScore : MonoBehaviour
    {
        [SerializeField] SpriteRenderer[] HappyBins = null;
        [SerializeField] SpriteRenderer[] AngryBins = null;

        const float c_UnitScore = 100.0f;
        const float c_PerfectMulti = 1.5f;
        RollingNumberImage scoreImage = null;
        RollingNumberImage comboImage = null;

        float[] scores = { 0.0f, 0.0f, 0.0f };
        float score = 0;
        float combo = 0;

        float[] HappyEmojiDeadline = { -1.0f, -1.0f, -1.0f };
        float[] AngryEmojiDeadline = { -1.0f, -1.0f, -1.0f };






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
                        addScore *= c_PerfectMulti;

                    SetEmoji(_target, true);
                    SetScore(score + addScore);
                    SetCombo(combo + 1);
                }
            }
            else
            {
                SetEmoji(_target, false);
                SetCombo(0);
            }

        }

        public void Miss(int _target)
        {
            SetCombo(0);
        }

        public void TrapPerfect()
        {
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