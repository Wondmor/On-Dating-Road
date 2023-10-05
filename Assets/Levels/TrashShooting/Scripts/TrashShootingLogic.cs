using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrashShooting
{
    public struct MusicNote
    {
        public ETrashType trashType;
        public float unit;

        public MusicNote(ETrashType _type, float _pos) 
        {
            trashType = _type; 
            unit = _pos;
        }
    }

    public class MusicInfo
    {
        public enum EDifficulty : int
        {
            Easy = 0,
            Normal = 1,
            Hard = 2,
        }

        public EDifficulty curDifficulty { get; private set; }

        static readonly ETrashType[] c_typePool = { ETrashType.TrashL, ETrashType.TrashU, ETrashType.TrashR, ETrashType.Trap };
        static readonly float[] c_intervals = { 8.0f, 4.0f, 2.0f };
        const float c_musicFrom = 40.0f;
        const float c_musicTo = 240.0f;
        const float c_unitPerBar = 16.0f;
        struct MusicPhase
        {
            public float From; //Included, in unit
            public float To; //Excluded, in unit
            public float Interval; // in unit

            public MusicPhase(float _From, float _To, float _Interval)
            {
                From = _From;
                To = _To;
                Interval = _Interval;
            }
        }

        public AudioClip bgm = null;
        //MusicNotes musicNotes;
        public Queue<MusicNote> musicNotes = null;
        Queue<MusicPhase> musicPhaseQueue = null;
        public float curPhaseEnd { private set; get; }
        public MusicInfo()
        {
            musicPhaseQueue = new Queue<MusicPhase>();
            musicPhaseQueue.Enqueue(new MusicPhase(2  * c_unitPerBar, 16 * c_unitPerBar, c_intervals[(int)curDifficulty]));
            musicPhaseQueue.Enqueue(new MusicPhase(18 * c_unitPerBar, 32 * c_unitPerBar, c_intervals[(int)curDifficulty]));
            musicPhaseQueue.Enqueue(new MusicPhase(34 * c_unitPerBar, 48 * c_unitPerBar, c_intervals[(int)curDifficulty]));
            musicPhaseQueue.Enqueue(new MusicPhase(50 * c_unitPerBar, 58 * c_unitPerBar, c_intervals[(int)curDifficulty]));

            musicNotes = new Queue<MusicNote>();
            musicNotes.Clear();

            curDifficulty = EDifficulty.Easy;
        }

        public bool HasMorePhase()
        {
            return musicPhaseQueue.Count > 0;
        }

        public void GenerateAPhase(EDifficulty eDifficulty)
        {
            curDifficulty = eDifficulty;
            var phase = musicPhaseQueue.Dequeue();
            curPhaseEnd = phase.To;

            for (float fUnit = phase.From; fUnit < phase.To; fUnit += c_intervals[(int)eDifficulty])
            {
                musicNotes.Enqueue(new MusicNote(c_typePool[UnityEngine.Random.Range(0, 3)], fUnit));
            }
            //float randomNodeTempo = 40.0f;
            //for (int i = 0; i < 400; ++i)
            //{
            //    musicNotes.Add(new MusicNote(c_typePool[UnityEngine.Random.Range(0, 3)], randomNodeTempo));
            //    randomNodeTempo += 4.0f;// 0.25f * 4.1f;// UnityEngine.Random.Range(2, 10);
            //}
        }


    }


    public class TrashShootingLogic : MonoBehaviour
    {

        [SerializeField] TrashShootingMusicBoard musicBoard = null;
        

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if(musicBoard.IsFinished)
            {
                musicBoard.gameObject.SetActive(false);
                GameLogicManager.Instance.OnMiniGameFinished(0, 0);
            }
        }

        void Pause()
        {
            //thtodo
        }

        void Quit()
        {
            //thtodo
        }
    }
}
