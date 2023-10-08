using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class BridgeLogic : MonoBehaviour
{
    [SerializeField] public GameObject FadeInGO = null;
    [SerializeField] public GameObject CoinTextLeft = null;
    [SerializeField] public GameObject CoinTextRight = null;
    [SerializeField] public GameObject Pos = null;
    [SerializeField] public Animator SpinningCoin = null;
    [SerializeField] public AudioSource BGMSource = null;
    [SerializeField] public AudioSource SFXSource = null;
    [SerializeField] public AudioClip walkingClip = null;

    public bool bFadeIn = true;
    public bool bLeft = true;
    public uint currentRoadMileStone = 0;



    public enum EPos
    {
        Start,
        Pos0,
        Pos1,
        Pos2,
        Pos3,
        Pos4,
        Pos5,
        Pos6,
        Target,
    }
    public EPos ePos = EPos.Pos0;

    private void Awake()
    {
        bFadeIn = GameLogicManager.Instance.bridgeData.bFadeIn;
        bLeft = GameLogicManager.Instance.bridgeData.bLeft;
        currentRoadMileStone = GameLogicManager.Instance.bridgeData.currentRoadMileStone;
        //GetComponent<TimelineLogic>().OnTimelineFinish += ()=>{ GameLogicManager.Instance.OnBridgeFinished(); };
        GetComponent<TimelineLogic>().OnTimelineFinish += OnBridgeTimelineFinish;
    }

    internal void OnBridgeTimelineFinish()
    {
        //
        SFXSource.clip = walkingClip;
        SFXSource.loop = false;
        SFXSource.Play();
        StartCoroutine(OnWalkingSFXEnd(Mathf.Min(3.0f, walkingClip.length)));
    }

    protected IEnumerator OnWalkingSFXEnd(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        BGMSource.Stop();
        SFXSource.Stop();
        GameLogicManager.Instance.OnBridgeFinished();
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.Find("CanvasFade").gameObject.SetActive(bFadeIn);
        //if (bFadeIn)
        //{
            //gameObject.GetComponent<PlayableDirector>().Pause();

            //iTween.ValueTo(gameObject, iTween.Hash(
            //"from", 1.0f,
            //"to", 0.0f,
            //"oncomplete", "OnFadeInComplete",
            //"oncompletetarget", gameObject,
            //"time", lastInSecond,
            //"easetype", iTween.EaseType.linear
        //));
        //}

        if (bLeft)
            CoinTextRight.transform.position += new Vector3(10000, 10000, 0);
        else
            CoinTextLeft.transform.position += new Vector3(10000, 10000, 0);

        //TODO
        //switch (ePos)
        //{ 
        
        //}

        SpinningCoin.Play("SpinningCoin");
    }



    // Update is called once per frame
    void Update()
    {
        
    }



    //void OnFadeInComplete()
    //{
    //    gameObject.GetComponent<PlayableDirector>().Pause();
    //}
}
