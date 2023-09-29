using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
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
        blackScreen.gameObject.SetActive(true);
        this.sceneName = sceneName;
        iTween.ValueTo(blackScreen.gameObject, iTween.Hash(
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

    void FadeIn()
    {
        blackScreen.gameObject.SetActive(true);
        iTween.ValueTo(blackScreen.gameObject, iTween.Hash(
            "from", 1.0f,
            "to", 0.0f,
            "onupdate", "UpdateValue",
            "onupdatetarget", gameObject,
            "time", lastInSecond,
            "easetype", iTween.EaseType.linear
        ));

        Destroy(this.gameObject, lastInSecond + 0.001f);
    }

    void UpdateValue(float newValue)
    {
        var col = blackScreen.color;
        col.a = newValue;
        blackScreen.color = col;
    }

    void OnComplete()
    {
        SceneManager.LoadScene(sceneName);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Fade In
        if (fadeType == EType.FadeIn)
        {
            FadeIn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
