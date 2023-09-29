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

    public bool bFadeIn = true;
    public string[] subtitles = { 
        "左边那条路红绿灯有点多，右边的路是个上坡，我走哪条呢？",
        "只能用投硬币来决定了"
    };
    public bool bLeft = true;
    

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
