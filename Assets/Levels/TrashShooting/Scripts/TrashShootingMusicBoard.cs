using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TrashShooting
{
    public class TrashShootingMusicBoard : MonoBehaviour
    {
        [SerializeField] GameObject[] LeftNotePrefabs = null;
        [SerializeField] GameObject[] UpNotePrefabs = null;
        [SerializeField] GameObject[] RightNotePrefabs = null;
        [SerializeField] GameObject[] TrapNotePrefabs = null;
        /*[SerializeField] */GameObject NoteBoardFrom = null;
        /*[SerializeField] */GameObject NoteBoardTo = null;
        /*[SerializeField] */GameObject NoteBoardCheck = null;
        /*[SerializeField] */GameObject EffectNormal = null;
        /*[SerializeField] */GameObject EffectPerfect = null;
        /*[SerializeField] */GameObject EffectShootPerfect = null;
        /*[SerializeField] */GameObject EffectShootNormal = null;
        /*[SerializeField] */GameObject EffectShootFail = null;
        /*[SerializeField] */GameObject[] Cans = { null, null, null};
        /*[SerializeField] */TempoBox tempoBox = null;
        [SerializeField] TrashShootingScore scoreBoard = null;



        [SerializeField] MusicUnity musicUnity;

        public const float noteSpawnDiff = -5.0f;
        public const float destoryDiff = 15.0f;
        public float m_tempoLength = 2.0f;// each bar
        public float m_UnitRange = 1.9f;
        public float m_PerfectRange = 1.5f;

        public LinkedList<Trash> noteBoard = new LinkedList<Trash>();

        public void SpawnNotes(Queue<MusicNote> _queueMusicNote)
        {
            var curBeat = musicUnity.MusicalTime * musicUnity.UnitPerBar;
            while (_queueMusicNote.Count > 0 && curBeat - _queueMusicNote.Peek().pos >= TrashShootingMusicBoard.noteSpawnDiff)
            {
                SpawnNote(_queueMusicNote.Dequeue());
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

            goNote.GetComponent<Trash>().pos = _musicNote.pos;
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
                float beatDiff = note.pos - musicUnity.MusicalTime * musicUnity.UnitPerBar; // Positive: before check.  Negative: after check.
                float posDiff = m_tempoLength * beatDiff;
                if (beatDiff >= 0)
                {
                    note.transform.position = check - posDiff * (check - from);
                }
                else
                {
                    note.transform.position = check - posDiff * (to - check);
                }

                if (beatDiff < -m_UnitRange / 2.0f)
                {
                    // Disabled effect
                    note.transform.localScale = new Vector2(0.5f, 0.5f);
                    if (!note.Checked)
                    {
                        note.Checked = true;
                        missedNotes.Add(note);
                    }
                }
                else
                {
                    note.transform.localScale = new Vector2(1.0f, 1.0f);
                    if (preNote == null || preNote.Checked)
                    {
                        if (activeNote == null  // first
                            && (NormalTimingCheck(beatDiff) || PerfectTimingCheck(beatDiff)))
                        {
                            activeNote = note;
                            activeDiff = beatDiff;
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
            while (noteBoard.First.Value.transform.position.x > destoryDiff && noteBoard.First.Value.Checked)
            {
                var temp = noteBoard.First.Value;
                noteBoard.RemoveFirst();
                Destroy(temp);
            }
        }


        // Start is called before the first frame update
        void Start()
        { 
            NoteBoardFrom = transform.Find("NoteFrom").gameObject;
            NoteBoardTo = transform.Find("NoteTo").gameObject;
            NoteBoardCheck = transform.Find("NoteCheck").gameObject;
            EffectNormal = transform.Find("effectNormal").gameObject;
            EffectPerfect = transform.Find("effectPerfect").gameObject;
            EffectShootPerfect = transform.Find("effectShootPerfect").gameObject;
            EffectShootNormal = transform.Find("effectShootNormal").gameObject;
            EffectShootFail = transform.Find("effectShootFail").gameObject;
            Cans[0] = transform.Find("canL").gameObject;
            Cans[1] = transform.Find("canU").gameObject;
            Cans[2] = transform.Find("canR").gameObject;
            tempoBox = transform.Find("TempoBox").gameObject.GetComponent<TempoBox>();
            //scoreBoard = transform.Find("ScoreBoard").gameObject.GetComponent<TrashShootingScore>();

            EffectNormal.GetComponent<Renderer>().enabled = false;
            EffectPerfect.GetComponent<Renderer>().enabled = false;
            EffectShootPerfect.GetComponent<Renderer>().enabled = false;
            EffectShootNormal.GetComponent<Renderer>().enabled = false;
            EffectShootFail.GetComponent<Renderer>().enabled = false;

            //musicUnity.Play();
        }


        // Update is called once per frame

        void Update()
        {
            if (!musicUnity.IsPlayingOrSuspended)
                return;

            var bPerfect = false;

            List<Trash> missedNotes = new List<Trash>();
            var activeNote = UpdateNodesAndGetActiveNote(NoteBoardFrom.transform.position, NoteBoardTo.transform.position, NoteBoardCheck.transform.position, ref bPerfect, ref missedNotes);

            tempoBox.SetIntensity(Mathf.Pow(Mathf.Cos(musicUnity.MusicalTime* Mathf.PI*2.0f),4.0f));

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
                    UnityEngine.Debug.LogFormat("{0},{1}, {2}",
                       inputAction.left.WasPerformedThisFrame() ? "left" : "",
                       inputAction.up.WasPerformedThisFrame() ? "up" : "",
                       inputAction.right.WasPerformedThisFrame() ? "right" : ""
                       );
                }

                if (bLeft || bUp || bRight)
                {
                    int target = -1;
                    var shootType = activeNote.GetInputRes(bLeft, bUp, bRight, bPerfect, ref target);
                    UnityEngine.Debug.LogFormat("ActiveNote:{0},{1}, Input:{2}, {3}, {4}, Result: shootType:{5}, target:{6}", activeNote.type, activeNote.pos, bLeft, bUp, bRight, shootType, target);
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
                            break;
                    }
                }
            }

            RemoveUselessNotes();
        }
    }
}