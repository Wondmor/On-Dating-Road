using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrashShooting
{
    public struct MusicNote
    {
        public ETrashType trashType;
        public float pos;

        public MusicNote(ETrashType _type, float _pos) 
        {
            trashType = _type; 
            pos = _pos;
        }
    }

    class MusicInfo
    {
        public AudioClip bgm = null;
        //MusicNotes musicNotes;
        public List<MusicNote> musicNotes = new List<MusicNote>();

        public void Init()
        {
            musicNotes.Clear();
            ETrashType[] typePool = { ETrashType.TrashL, ETrashType.TrashU, ETrashType.TrashR, ETrashType.Trap };
            float randomNodeTempo = 40.0f;
            for (int i = 0; i < 400; ++i)
            {
                musicNotes.Add(new MusicNote(typePool[UnityEngine.Random.Range(0, 4)], randomNodeTempo));
                randomNodeTempo += 4.0f;// 0.25f * 4.1f;// UnityEngine.Random.Range(2, 10);
            }
        }
    }


    public class TrashShootingLogic : MonoBehaviour
    {
        Queue<MusicNote> queueMusicNote = new Queue<MusicNote>();
        MusicInfo musicInfo = new MusicInfo();

        [SerializeField] TrashShootingMusicBoard musicBoard = null;
        

        // Start is called before the first frame update
        void Start()
        {

            musicInfo.bgm = Resources.Load<AudioClip>("Adam Bow - Death and Taxes Soundtrack - 16 Skelevator (Radio Effect)");
            musicInfo.Init();
            foreach (MusicNote musicNote in musicInfo.musicNotes)
            {
                queueMusicNote.Enqueue(musicNote);
            }
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

        void FixedUpdate()
        {
            musicBoard.SpawnNotes(queueMusicNote);            
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
