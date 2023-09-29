using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class WaitForInputHandler : MonoBehaviour
{
    [Header("结束后跳转到")]
    [SerializeField] public string sceneName = "";
    [SerializeField] public CommonInputAction input = null;

    internal void OnTimelineLastFrame()
    {
        GetComponent<PlayableDirector>().Pause();
        if (transform.Find("CanvasFade") && transform.Find("CanvasFade").GetComponent<FadeInOutScene>())
            transform.Find("CanvasFade").GetComponent<FadeInOutScene>().FadeOut(sceneName);
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
        if (bWaitingInput)
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
