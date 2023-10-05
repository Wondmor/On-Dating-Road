using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace TrashShooting
{
    public class TrashShootingLight : MonoBehaviour
    {

        Light2D ColorLight = null;
        Light2D WhiteLight = null;
        Light2D FalloutLight = null;
        Animator LightL = null;
        Animator LightU = null;
        Animator LightR = null;

        MusicInfo.EDifficulty eDifficulty = MusicInfo.EDifficulty.Easy;
        // Start is called before the first frame update
        void Start()
        {
            ColorLight = GetComponent<Light2D>();
            WhiteLight = transform.Find("LightWhite").GetComponent<Light2D>();
            FalloutLight = transform.Find("LightFallout").GetComponent<Light2D>();
            LightL = transform.Find("TargetLightL").GetComponent<Animator>();
            LightU = transform.Find("TargetLightU").GetComponent<Animator>();
            LightR = transform.Find("TargetLightR").GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetDifficulty(MusicInfo.EDifficulty difficulty)
        {
            eDifficulty = difficulty;
            switch (eDifficulty)
            {
                case MusicInfo.EDifficulty.Easy:
                    WhiteLight.intensity = 1;
                    FalloutLight.intensity = 0;
                    ColorLight.intensity = 1;
                    break;
                case MusicInfo.EDifficulty.Normal:
                    WhiteLight.intensity = 0.5f;
                    FalloutLight.intensity = 0.5f;
                    ColorLight.intensity = 5;
                    break;
                case MusicInfo.EDifficulty.Hard:
                    WhiteLight.intensity = 0.0f;
                    FalloutLight.intensity = 1.0f;
                    ColorLight.intensity = 10f;
                    break;
            }
        }

        public void Shoot(int _target)
        {
            if (_target >= 0 && _target <= 2)
            {
                var targetLight = _target == 0 ? LightL : (_target == 1 ? LightU : LightR);
                switch (eDifficulty)
                {
                    case MusicInfo.EDifficulty.Easy:
                        break;
                    case MusicInfo.EDifficulty.Normal:
                        targetLight.SetTrigger("ShootNormal");
                        break;
                    case MusicInfo.EDifficulty.Hard:
                        targetLight.SetTrigger("ShootHard");
                        break;
                }
            }
        }
    }
}