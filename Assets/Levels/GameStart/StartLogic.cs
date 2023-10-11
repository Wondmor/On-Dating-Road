using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StartLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.CommonInputAction.enter.WasPerformedThisFrame())
        {
            if (transform.Find("CanvasFadeOut") && transform.Find("CanvasFadeOut").GetComponent<FadeInOutScene>())
            {
                var fadeOut = transform.Find("CanvasFadeOut").GetComponent<FadeInOutScene>();
                fadeOut.gameObject.SetActive(true);
                fadeOut.FadeOut(gameObject, "InvokeOnFadeoutFinish");
            }
            else
            {
                InvokeOnFadeoutFinish();
                //SceneManager.LoadScene(sceneName);

            }


        }
    }

    internal void InvokeOnFadeoutFinish()
    {
        GameLogicManager.Instance.GameStart();
    }
}
