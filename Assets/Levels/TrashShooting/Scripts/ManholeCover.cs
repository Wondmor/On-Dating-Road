using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TrashShooting.MusicInfo;

namespace TrashShooting
{
    public class ManholeCover : MonoBehaviour
    {
        [SerializeField] Animator manholeCover = null;
        [SerializeField] Animator lighten = null;
        TrashShooting.MusicInfo.EDifficulty eDifficulty;
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void SetDifficulty(MusicInfo.EDifficulty eDifficulty)
        {
            this.eDifficulty = eDifficulty;
            lighten.gameObject.SetActive(this.eDifficulty != EDifficulty.Easy);

        }

        public void Shoot()
        {
            manholeCover.SetTrigger("Shoot");
            UnityEngine.Debug.LogFormat("ManholeCover Shoot");

            if(this.eDifficulty == EDifficulty.Hard)
            {
                lighten.SetTrigger("Shoot");
            }
        }
    }
}