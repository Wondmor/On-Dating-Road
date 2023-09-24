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
        RollingNumberText[] scoreTexts = { null, null, null };
        RollingNumberText comboText = null;

        float[] scores = { 0.0f, 0.0f, 0.0f };
        int combo = 0;





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

                    SetScoreToText(scores[_target] + addScore, _target);
                    SetComboToText(combo + 1);
                }
            }
            else
            {
                SetComboToText(0);
            }

        }

        public void Miss(int _target)
        {
            SetComboToText(0);
        }

        public void TrapPerfect()
        {
            SetComboToText(combo + 1);
        }



        // Start is called before the first frame update
        void Start()
        {
            scoreTexts[0] = transform.Find("left").GetComponent<RollingNumberText>();
            scoreTexts[1] = transform.Find("up").GetComponent<RollingNumberText>();
            scoreTexts[2] = transform.Find("right").GetComponent<RollingNumberText>();
            comboText = transform.Find("combo").GetComponent<RollingNumberText>();

            SetScoreToText(scores[0], 0);
            SetScoreToText(scores[1], 1);
            SetScoreToText(scores[2], 2);
            SetComboToText(0);
        }

        // Update is called once per frame
        void Update()
        {

        }

        float GetComboMulti(int _combo)
        {
            return Mathf.Clamp(_combo, 0.0f, 500.0f) / 100.0f + 1.0f; // [0,5]
        }

        void SetComboToText(int _combo)
        {
            var oldCombo = combo;
            combo = _combo;


            iTween.ValueTo(comboText.gameObject, iTween.Hash(
                "from", oldCombo,
                "to", combo,
                "time", Mathf.Pow((Mathf.Abs(combo-oldCombo)/100.0f), 0.5f),
                "onupdate", "UpdateValue",
                "onupdatetarget", comboText.gameObject,
                "easetype", iTween.EaseType.linear
            ));
        }

        void SetScoreToText(float _score, int _target)
        {
            if (_target >= 0)
            {
                var oldScore = scores[_target];
                scores[_target] = _score;


                iTween.ValueTo(scoreTexts[_target].gameObject, iTween.Hash(
                    "from", oldScore,
                    "to", scores[_target],
                    "time", Mathf.Pow((Mathf.Abs(scores[_target] - oldScore) / 100.0f), 0.5f),
                    "onupdate", "UpdateValue",
                    "onupdatetarget", scoreTexts[_target].gameObject,
                    "easetype", iTween.EaseType.linear
                )); 
            }
        }
    }

}