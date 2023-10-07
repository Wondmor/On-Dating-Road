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
            UnityEngine.Debug.LogFormat("GenerateAPhase {0}, {1}",
                           phase.From, phase.To);

            for (float fUnit = phase.From; fUnit < phase.To; fUnit += c_intervals[(int)eDifficulty])
            {
                musicNotes.Enqueue(new MusicNote(c_typePool[UnityEngine.Random.Range(0, 3)], 
                    fUnit));
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
        [SerializeField] Canvas scoreCanvas = null;
        [SerializeField] TrashShootingStory musicStory = null;
        [SerializeField] TrashShootingSummary summary = null;

        enum EStage
        {
            Before,
            Game,
            After,
            AfterFinished,
        }

        EStage eStage = EStage.Before;

        private void Awake()
        {
            musicStory.onFinished = OnStoryFinished;
        }

        // Start is called before the first frame update
        void Start()
        {
            musicStory.onFinished = OnStoryFinished;
        }

        public void OnStoryFinished()
        {
            eStage = EStage.Game;
            musicStory.gameObject.SetActive(false);
            musicBoard.gameObject.SetActive(true);
            scoreCanvas.gameObject.SetActive(true);
            musicBoard.StartGame();
        }

        public void OnAfterFinished()
        {
            musicBoard.gameObject.SetActive(false);
            float addMoney = 0, addPositiveComments = 0, passTimeRate = 0;
            musicBoard.GetScore(ref addMoney, ref addPositiveComments, ref passTimeRate);
            float money = GameLogicManager.Instance.gameData.money + addMoney;
            float positiveComment = GameLogicManager.Instance.gameData.positiveComment + addPositiveComments;
            float time = GameLogicManager.Instance.gameData.countDown - GameLogicManager.c_StandardGameDuration * passTimeRate;

            GameLogicManager.Instance.OnMiniGameFinished(money, positiveComment, time);
        }

        // Update is called once per frame
        void Update()
        {
            switch(eStage)
            {
                case EStage.Game:
                    if (musicBoard.IsFinished)
                    {
                        musicBoard.StopGame();
                        musicBoard.gameObject.SetActive(false);
                        scoreCanvas.gameObject.SetActive(false);
                        summary.gameObject.SetActive(true);

                        float addMoney = 0, addPositiveComments = 0, passTimeRate = 0;
                        uint result = musicBoard.GetScore(ref addMoney, ref addPositiveComments, ref passTimeRate);
                        summary.SetScore(result, addMoney, addPositiveComments, passTimeRate);
                        eStage = EStage.After;
                    }
                    break;
                case EStage.After:
                    if(summary.IsFinished)
                    {
                        eStage = EStage.AfterFinished;
                        OnAfterFinished();
                    }
                    break;
                default:
                    break;
            }

        }
    }
}
