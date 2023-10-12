using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInOutScene : MonoBehaviour
{
    public enum EType
    {
        FadeIn,
        FadeOut,
        None
    }

    [SerializeField] public EType fadeType = EType.None;
    [SerializeField] public Image blackScreen = null;
    [SerializeField] public float lastInSecond = 2.0f;

    private string sceneName = "";

    public void FadeOut(string sceneName)
    {
        GetBlackScreen().gameObject.SetActive(true);
        this.sceneName = sceneName;
        iTween.ValueTo(GetBlackScreen().gameObject, iTween.Hash(
            "from", 0.0f,
            "to", 1.0f,
            "time", lastInSecond,
            "onupdate", "UpdateValue",
            "onupdatetarget", gameObject,
            "oncomplete", "OnComplete",
            "oncompletetarget", gameObject,
            "easetype", iTween.EaseType.linear
        ));
    }
    public void FadeOut(GameObject onCompleteTarget, string onComplete)
    {
        GetBlackScreen().gameObject.SetActive(true);
        iTween.ValueTo(GetBlackScreen().gameObject, iTween.Hash(
            "from", 0.0f,
            "to", 1.0f,
            "time", lastInSecond,
            "onupdate", "UpdateValue",
            "onupdatetarget", gameObject,
            "oncomplete", onComplete,
            "oncompletetarget", onCompleteTarget,
            "easetype", iTween.EaseType.linear
        ));
    }

    void FadeIn()
    {
        GetBlackScreen().gameObject.SetActive(true);
        iTween.ValueTo(GetBlackScreen().gameObject, iTween.Hash(
            "from", 1.0f,
            "to", 0.0f,
            "onupdate", "UpdateValue",
            "onupdatetarget", gameObject,
            "oncomplete", "OnFadeInComplete",
            "oncompletetarget", gameObject,
            "time", lastInSecond,
            "easetype", iTween.EaseType.linear
        ));
    }

    internal void OnFadeInComplete()
    {
        if(this != null)
        {
            Destroy(this.gameObject);
        }
    }

    void UpdateValue(float newValue)
    {
        var col = GetBlackScreen().color;
        col.a = newValue;
        GetBlackScreen().color = col;
    }

    void OnComplete()
    {
        SceneManager.LoadScene(sceneName);
        //Destroy(gameObject);
    }

    Image GetBlackScreen()
    { 
        if (blackScreen == null)
            blackScreen = transform.Find("BlackScreen").GetComponent<Image>();
        return blackScreen;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Fade In
        if (fadeType == EType.FadeIn)
        {
            FadeIn();
        }
        else if(fadeType == EType.FadeOut)
        {
            UpdateValue(0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
