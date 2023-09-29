using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimelineLogic : MonoBehaviour
{
    [Header("结束后跳转到")]
    [SerializeField] public string sceneName = "";
    [SerializeField] public CommonInputAction input = null;

    internal void OnTimelineLastFrame()
    {
        GetComponent<PlayableDirector>().Pause();
        if (transform.Find("CanvasFadeOut") && transform.Find("CanvasFadeOut").GetComponent<FadeInOutScene>())
        {
            var fadeOut = transform.Find("CanvasFadeOut").GetComponent<FadeInOutScene>();
            fadeOut.gameObject.SetActive(true);
            fadeOut.FadeOut(sceneName);
        }
        else
            SceneManager.LoadScene(sceneName);
    }

    bool bWaitingInput = false;
    CommonInputAction.EType eType = CommonInputAction.EType.None;
    internal void OnTimelineWaitingInput(CommonInputAction.EType eType)
    {
        GetComponent<PlayableDirector>().Pause();
        this.eType = eType;
        bWaitingInput = true;
    }

    //LinkedList<CutsceneBehaviour>

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(bWaitingInput)
        {
            if (input.GetPerformedTypeThisFrame() == eType)
            {
                bWaitingInput = false;
                eType = CommonInputAction.EType.None;

                var dir = GetComponent<PlayableDirector>();
                dir.time += 0.001;
                dir.Play();
                //GetComponent<PlayableDirector>().Play();
            }
        }

    }
}
