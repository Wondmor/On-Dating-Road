using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrashShooting
{


    public class TrashShootingMusicBoard : MonoBehaviour
    {
        [SerializeField] float decale = 0.0f;
        [SerializeField] GameObject[] LeftNotePrefabs = null;
        [SerializeField] GameObject[] UpNotePrefabs = null;
        [SerializeField] GameObject[] RightNotePrefabs = null;
        [SerializeField] GameObject[] TrapNotePrefabs = null;
        [SerializeField] GameObject NoteBoardFrom = null;
        [SerializeField] GameObject NoteBoardTo = null;
        [SerializeField] GameObject NoteBoardCheck = null;
        [SerializeField] GameObject EffectNormal = null;
        [SerializeField] GameObject EffectPerfect = null;
        [SerializeField] GameObject EffectShootPerfect = null;
        [SerializeField] GameObject EffectShootNormal = null;
        [SerializeField] GameObject EffectShootFail = null;
        [SerializeField] GameObject[] Cans = { null, null, null};
        [SerializeField] TempoBox tempoBox = null;
        [SerializeField] ManholeCover manholeCover = null;
        [SerializeField] TrashShootingLight lightSystem = null;
        [SerializeField] TrashShootingScore scoreBoard = null;



        [SerializeField] MusicUnity musicUnity;

        public const float noteSpawnDiff = 5.0f;
        public const float destoryDiff = 15.0f;
        public float m_unitLength = 2.0f;// each bar
        public float m_UnitRange = 1.9f;
        public float m_PerfectRange = 1.5f;
        private float[] HitNoteWeights = { 0.8f, 1.1f,1.5f };

        public bool IsFinished {  get; private set; }

        public LinkedList<Trash> noteBoard = new LinkedList<Trash>();


        private Vector2 normalizedCheckToFrom = Vector2.zero;
        private Vector2 normalizedCheckToTo = Vector2.zero;
        private Vector2 checkPosition = Vector2.zero;
        //private float unitFromDiff = 0.0f;

        MusicInfo musicInfo = new MusicInfo();

        private float money = 0;
        private float positiveComment = 0;
        private float timeRate = 1.0f;
        private uint result = 0;
        private float noteCount = 0;
        private float hitNotes = 0;
        private float weightedHitNotes = 0;
        private float perfectNotes = 0;
        private float maxCombo = 0;

        private Vector2 unitToPosition(float _unit)
        {
            float unitDiff = _unit - musicUnity.MusicalTime * musicUnity.UnitPerBar; // Positive: before check.  Negative: after check.
            float positionDiff = m_unitLength * unitDiff;
            if (unitDiff >= 0)
            {
                return checkPosition + positionDiff * normalizedCheckToFrom;
            }
            else
            {
                return checkPosition - positionDiff * normalizedCheckToTo;
            }
        }

        private float positionToUnitDiff(Vector2 position)
        {
            float positionDiff = 0.0f;
            if (Vector2.Dot(position - checkPosition, normalizedCheckToFrom) > 0)
            {
                //before check
                positionDiff = (position - checkPosition).magnitude / normalizedCheckToFrom.magnitude;
            }
            else
            {
                //after check
                positionDiff = (checkPosition - position).magnitude / normalizedCheckToTo.magnitude;
            }
            return positionDiff / m_unitLength;
        }

        private float positionToUnit(Vector2 position)
        {
            return positionToUnitDiff(position) + musicUnity.MusicalTime * musicUnity.UnitPerBar;
        }

        public void SpawnNotes(Queue<MusicNote> _queueMusicNote)
        {
            if (musicUnity.CurrentMeter != null)
            {
                var curUnit = musicUnity.MusicalTime * musicUnity.UnitPerBar;
                var unitFrom = positionToUnit(NoteBoardFrom.transform.position); // the unit that the position of NoteBoardFrom represents.
                while (_queueMusicNote.Count > 0 &&
                    _queueMusicNote.Peek().unit < TrashShootingMusicBoard.noteSpawnDiff + unitFrom)
                {
                    SpawnNote(_queueMusicNote.Dequeue());
                }
            }
        }
        public void SpawnNote(MusicNote _musicNote)
        {
            GameObject goNote = null;
            switch (_musicNote.trashType)
            {
                case ETrashType.TrashL:
                    goNote = Instantiate(LeftNotePrefabs[UnityEngine.Random.Range(0, LeftNotePrefabs.Length)], this.gameObject.transform);
                    break;
                case ETrashType.TrashU:
                    goNote = Instantiate(UpNotePrefabs[UnityEngine.Random.Range(0, UpNotePrefabs.Length)], this.gameObject.transform);
                    break;
                case ETrashType.TrashR:
                    goNote = Instantiate(RightNotePrefabs[UnityEngine.Random.Range(0, RightNotePrefabs.Length)], this.gameObject.transform);
                    break;
                case ETrashType.Trap:
                    goNote = Instantiate(TrapNotePrefabs[UnityEngine.Random.Range(0, TrapNotePrefabs.Length)], this.gameObject.transform);
                    break;
            }

            goNote.GetComponent<Trash>().unit = _musicNote.unit + decale;
            noteBoard.AddLast(goNote.GetComponent<Trash>());
        }

        bool NormalTimingCheck(float _beatDiff)
        {
            return Mathf.Abs(_beatDiff) < m_UnitRange / 2.0f;
        }

        bool PerfectTimingCheck(float _beatDiff)
        {
            return Mathf.Abs(_beatDiff) < m_PerfectRange / 2.0f;
        }

        Trash UpdateNodesAndGetActiveNote(Vector2 from, Vector2 to, Vector2 check, ref bool bOutPerfect, ref List<Trash> missedNotes)
        {
            Trash activeNote = null;
            float activeDiff = 0.0f;

            Trash preNote = null;

            foreach (var note in noteBoard)
            {
                //float timeDiff = notePair.Key - curTime; // Positive: before check.  Negative: after check.
                float unitDiff = note.unit - musicUnity.MusicalTime * musicUnity.UnitPerBar; // Positive: before check.  Negative: after check.
                float positionDiff = m_unitLength * unitDiff;
                if (unitDiff >= 0)
                {
                    note.transform.position = check - positionDiff * (check - from);
                }
                else
                {
                    note.transform.position = check - positionDiff * (to - check);
                }

                if (unitDiff < -m_UnitRange / 2.0f)
                {
                    // Disabled effect
                    note.transform.localScale = new Vector2(1.0f, 1.0f);
                    note.OnEscape();
                    if (!note.Checked)
                    {
                        note.Checked = true;
                        missedNotes.Add(note);
                    }
                }
                else
                {
                    //note.transform.localScale = new Vector2(1.0f, 1.0f);
                    if (preNote == null || preNote.Checked)
                    {
                        if (activeNote == null  // first
                            && (NormalTimingCheck(unitDiff) || PerfectTimingCheck(unitDiff)))
                        {
                            activeNote = note;
                            activeDiff = unitDiff;
                        }

                    }
                }
                preNote = note;
            }


            bool bNormalRange = activeNote && NormalTimingCheck(activeDiff);
            bool bPerfectRange = activeNote && PerfectTimingCheck(activeDiff);
            bOutPerfect = bPerfectRange;

            EffectNormal.GetComponent<Renderer>().enabled = bNormalRange;
            EffectPerfect.GetComponent<Renderer>().enabled = bPerfectRange;

            if(activeNote && (bNormalRange || bPerfectRange))
                activeNote.transform.localScale = new Vector2(1.5f, 1.5f);


            return activeNote && (bNormalRange || bPerfectRange)? activeNote.GetComponent<Trash>() : null;
        }

        void RemoveUselessNotes()
        {
            if(noteBoard.First != noteBoard.Last)
            {
                while (noteBoard.First.Value.transform.position.x > destoryDiff && noteBoard.First.Value.Checked)
                {
                    var temp = noteBoard.First.Value;
                    noteBoard.RemoveFirst();
                    Destroy(temp.gameObject);
                }
            }
        }

        void SetNextPhase(MusicInfo.EDifficulty eDifficulty)
        {
            musicInfo.GenerateAPhase(eDifficulty);
            manholeCover.SetDifficulty(eDifficulty);
            lightSystem.SetDifficulty(eDifficulty);
        }

        public void StartGame()
        {
            musicUnity.Play();
            lightSystem.TurnOn();
        }

        public void StopGame()
        {
            lightSystem.TurnOff();
        }

        public uint GetScore(ref float addMoney, ref float addPositiveComments, ref float passTimeRate)
        {
            addMoney = money;
            addPositiveComments = positiveComment;
            passTimeRate = timeRate;

            return result;
        }


        // Start is called before the first frame update
        void Start()
        { 
            //NoteBoardFrom = transform.Find("NoteFrom").gameObject;
            //NoteBoardTo = transform.Find("NoteTo").gameObject;
            //NoteBoardCheck = transform.Find("NoteCheck").gameObject;
            //EffectNormal = transform.Find("effectNormal").gameObject;
            //EffectPerfect = transform.Find("effectPerfect").gameObject;
            //EffectShootPerfect = transform.Find("effectShootPerfect").gameObject;
            //EffectShootNormal = transform.Find("effectShootNormal").gameObject;
            //EffectShootFail = transform.Find("effectShootFail").gameObject;
            //Cans[0] = transform.Find("canL").gameObject;
            //Cans[1] = transform.Find("canU").gameObject;
            //Cans[2] = transform.Find("canR").gameObject;
            //tempoBox = transform.Find("TempoBox").gameObject.GetComponent<TempoBox>();
            //manholeCover = transform.Find("ManholeCover").gameObject.GetComponent<ManholeCover>();
            //lightSystem = transform.Find("Light").gameObject.GetComponent<TrashShootingLight>();
            //scoreBoard = transform.Find("ScoreBoard").gameObject.GetComponent<TrashShootingScore>();

            EffectNormal.GetComponent<Renderer>().enabled = false;
            EffectPerfect.GetComponent<Renderer>().enabled = false;
            EffectShootPerfect.GetComponent<Renderer>().enabled = false;
            EffectShootNormal.GetComponent<Renderer>().enabled = false;
            EffectShootFail.GetComponent<Renderer>().enabled = false;

            //musicUnity.Play();
            IsFinished = false;

            normalizedCheckToFrom = (Vector2)NoteBoardFrom.transform.position - (Vector2)NoteBoardCheck.transform.position;
            normalizedCheckToFrom.Normalize();
            normalizedCheckToTo = (Vector2)NoteBoardTo.transform.position - (Vector2)NoteBoardCheck.transform.position;
            normalizedCheckToTo.Normalize();
            checkPosition = (Vector2)NoteBoardCheck.transform.position;
            //unitFromDiff = positionToUnitDiff(NoteBoardFrom.transform.position);


            SetNextPhase(MusicInfo.EDifficulty.Easy);
        }


        // Update is called once per frame

        void Update()
        {
            //UnityEngine.Debug.LogFormat("MusicalTime {0}",
            //               musicUnity.MusicalTime
            //               );

            if (musicUnity.State == Music.PlayState.Finished)
            {
                // Music finished
                money = Mathf.Min(20, Mathf.RoundToInt(weightedHitNotes / 5)); //0-20
                positiveComment = 5 + Mathf.Pow(Mathf.Clamp01(weightedHitNotes / noteCount), 0.6f) * 35.0f;//5-40
                result = (uint)Mathf.CeilToInt(weightedHitNotes / noteCount * 5); //0-5 stars
                IsFinished = true;
            }
            else if(musicUnity.State == Music.PlayState.Playing)
            {
                //Difficulty control
                if(musicInfo.HasMorePhase() &&
                    musicUnity.MusicalTime * musicUnity.UnitPerBar >= musicInfo.curPhaseEnd)
                {
                    var nextDifficulty = musicInfo.curDifficulty;

                    var statics = scoreBoard.PhaseEnd();
                    float missRate = (float)statics.miss / (float)statics.notes;
                    float hitRate = (float)statics.perfect / (float)statics.notes + (float)statics.normal / (float)statics.notes;
                    float maxCombo = statics.maxCombo;
                    noteCount += statics.notes;
                    hitNotes += statics.perfect + statics.normal;
                    weightedHitNotes += (statics.perfect * 1.1f + statics.normal) * HitNoteWeights[(int)musicInfo.curDifficulty];
                    perfectNotes += statics.perfect;
                    this.maxCombo = Mathf.Max(maxCombo, this.maxCombo);


                    if(hitRate >=0.7 && musicInfo.curDifficulty != MusicInfo.EDifficulty.Hard)
                    {
                        nextDifficulty++;
                    }
                    else if(hitRate < 0.4 && musicInfo.curDifficulty != MusicInfo.EDifficulty.Easy)
                    {

                        nextDifficulty--;
                    }

                    SetNextPhase(nextDifficulty);
                }

                // Note check
                UpdateFrame();
            }
        }

        void FixedUpdate()
        {
            SpawnNotes(musicInfo.musicNotes);
        }

        void UpdateFrame()
        {
            var bPerfect = false;

            List<Trash> missedNotes = new List<Trash>();
            var activeNote = UpdateNodesAndGetActiveNote(NoteBoardFrom.transform.position, NoteBoardTo.transform.position, NoteBoardCheck.transform.position, ref bPerfect, ref missedNotes);

            tempoBox.SetIntensity(Mathf.Pow(Mathf.Sin(musicUnity.MusicalTime * 2.0f * Mathf.PI * 2.0f), 4.0f));

            // Miss
            foreach (var note in missedNotes)
            {
                if (note.type == ETrashType.Trap)
                    scoreBoard.TrapPerfect();
                else
                    scoreBoard.Miss((int)note.type);
            }
            missedNotes.Clear();

            // Input
            if (activeNote)
            {
                var inputAction = gameObject.GetComponent<TrashShootingInputAction>();

                bool bLeft = inputAction.left.WasPerformedThisFrame();
                bool bUp = inputAction.up.WasPerformedThisFrame();
                bool bRight = inputAction.right.WasPerformedThisFrame();
                if (bLeft || bUp || bRight)
                {
                    //UnityEngine.Debug.LogFormat("{0},{1}, {2}",
                    //   inputAction.left.WasPerformedThisFrame() ? "left" : "",
                    //   inputAction.up.WasPerformedThisFrame() ? "up" : "",
                    //   inputAction.right.WasPerformedThisFrame() ? "right" : ""
                    //   );
                }

                if (bLeft || bUp || bRight)
                {
                    int target = -1;
                    var shootType = activeNote.GetInputRes(bLeft, bUp, bRight, bPerfect, ref target);
                    //UnityEngine.Debug.LogFormat("ActiveNote:{0},{1}, Input:{2}, {3}, {4}, Result: shootType:{5}, target:{6}", activeNote.type, activeNote.unit, bLeft, bUp, bRight, shootType, target);
                    switch (shootType)
                    {
                        case EShootType.TrapPerfect:
                            scoreBoard.TrapPerfect();
                            break;
                        case EShootType.Miss:
                            scoreBoard.Miss(target);
                            break;
                        default:
                            //target can't be -1
                            noteBoard.Remove(activeNote);
                            activeNote.Shoot(shootType, Cans[target].transform.position);
                            scoreBoard.Shoot(shootType, target);
                            manholeCover.Shoot();
                            lightSystem.Shoot(target);
                            break;
                    }
                }
            }

            RemoveUselessNotes();
        }
    }
}