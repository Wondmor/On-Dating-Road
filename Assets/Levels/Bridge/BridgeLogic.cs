using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] public Sprite afterPreludeBG = null;
    [SerializeField] public Sprite afterBusGameBG = null;
    [SerializeField] public Sprite afterTrashshootingBG = null;
    [SerializeField] public Sprite afterSkewerBG = null;
    [SerializeField] public Sprite afterRacingBG = null;
    [SerializeField] public Sprite afterDeliveryBG = null;

    public bool bFadeIn = true;
    public bool bLeft = true;
    public uint currentRoadMileStone = 0;
    public EScene prevGame = EScene.Prelude;



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
        prevGame = GameLogicManager.Instance.bridgeData.prevGame;
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

        Sprite bgSprite = null;
        switch(prevGame)
        {
            case EScene.Prelude: bgSprite = afterPreludeBG;  break;
            case EScene.BusGame: bgSprite = afterBusGameBG; break;
            case EScene.TrashShooting: bgSprite = afterTrashshootingBG; break;
            case EScene.Skewer: bgSprite = afterSkewerBG; break;
            case EScene.Racing: bgSprite = afterRacingBG; break;
            case EScene.Delivery: bgSprite = afterDeliveryBG; break;
        }

        if(bgSprite != null)
            transform.Find("Canvas/BG").GetComponent<Image>().sprite = bgSprite;

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
