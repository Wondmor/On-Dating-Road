using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrashShooting
{
    public class TrashShootingScore : MonoBehaviour
    {

        const float c_UnitScore = 100.0f;
        const float c_PerfectMulti = 1.5f;
        //RollingNumberText[] scoreTexts = { null, null, null };
        //RollingNumberText comboText = null;
        RollingNumberImage scoreImage = null;
        RollingNumberImage comboImage = null;

        float[] scores = { 0.0f, 0.0f, 0.0f };
        float score = 0;
        float combo = 0;





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

                    //SetScoreToText(scores[_target] + addScore, _target);
                    //SetComboToText(combo + 1);
                    SetScore(score + addScore);
                    SetCombo(combo + 1);
                }
            }
            else
            {
                //SetComboToText(0);
                SetCombo(0);
            }

        }

        public void Miss(int _target)
        {
            //SetComboToText(0);
            SetCombo(0);
        }

        public void TrapPerfect()
        {
            //SetComboToText(combo + 1);
            SetCombo(combo + 1);
        }



        // Start is called before the first frame update
        void Start()
        {
            //scoreTexts[0] = transform.Find("left").GetComponent<RollingNumberText>();
            //scoreTexts[1] = transform.Find("up").GetComponent<RollingNumberText>();
            //scoreTexts[2] = transform.Find("right").GetComponent<RollingNumberText>();
            //comboText = transform.Find("combo").GetComponent<RollingNumberText>();
            scoreImage = transform.Find("rollingScore").GetComponent<RollingNumberImage>();
            comboImage = transform.Find("rollingCombo").GetComponent<RollingNumberImage>();

            //SetScoreToText(scores[0], 0);
            //SetScoreToText(scores[1], 1);
            //SetScoreToText(scores[2], 2);
            SetScore(score);
            //SetComboToText(0);
            SetCombo(0);
        }

        // Update is called once per frame
        void Update()
        {

        }

        float GetComboMulti(float _combo)
        {
            return Mathf.Clamp(_combo, 0.0f, 500.0f) / 100.0f + 1.0f; // [0,5]
        }

        //void SetComboToText(float _combo)
        //{
        //    linearValue(ref combo, _combo, comboText.gameObject);
        //}

        //void SetScoreToText(float _score, int _target)
        //{
        //    if (_target >= 0)
        //    {
        //        linearValue(ref scores[_target], _score, scoreTexts[_target].gameObject);
        //    }
        //}

        void SetScore(float _score)
        {
            linearValue(ref score, _score, scoreImage.gameObject);
        }

        void SetCombo(float _combo)
        {
            linearValue(ref combo, _combo, comboImage.gameObject);
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