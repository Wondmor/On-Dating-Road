using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrashShootingStory : MonoBehaviour
{
    [SerializeField] Image introduction = null;
    [SerializeField] Image storyBG = null;
    [SerializeField] Image SubtitleBG = null;
    [SerializeField] TextMeshProUGUI text = null;
    [SerializeField] CommonSelection skipSelection = null;

    public delegate void OnFinished();
    public OnFinished onFinished= null;

    string[] lines = { "哎呦，喇个小伙子，帮帮忙好伐",
        "今天阿不资道为什么垃圾这么多，愁死个人了",
        "帮忙阿姨把这些垃圾丢一下好伐啦？呷呷侬喔" };
    int linesId = 0;
    float waitTime = 0.5f;

    enum EState
    {
        Lines,
        AskSkip,
        AskSkipFinished,
        Introduction
    }

    EState state = EState.Lines;

    public void OnSkipSelection(bool bHelp)
    {
        if (!bHelp)
        {
            GameLogicManager.Instance.OnMiniGameRefused();
        }
        else
        {
            skipSelection.gameObject.SetActive(false);
            introduction.gameObject.SetActive(true);
            state = EState.AskSkipFinished;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        storyBG.gameObject.SetActive(true);
        introduction.gameObject.SetActive(false);
        SubtitleBG.gameObject.SetActive(true);
        skipSelection.gameObject.SetActive(false);
        text.text = lines[linesId];
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case EState.Lines:
                if(GameManager.Instance.CommonInputAction.enter.WasPerformedThisFrame())
                {
                    if(linesId < lines.Length - 1)
                        text.text = lines[++linesId];
                    else
                    {
                        SubtitleBG.gameObject.SetActive(false);
                        skipSelection.gameObject.SetActive(true);
                        skipSelection.ShowChoice();
                        state = EState.AskSkip;
                    }
                }
                break;
            case EState.AskSkip:
                break;
            case EState.AskSkipFinished:
                state = EState.Introduction;
                break;
            case EState.Introduction:
                if(waitTime > 0)
                    waitTime -= Time.deltaTime;
                else if (GameManager.Instance.CommonInputAction.enter.WasPerformedThisFrame())
                {
                    onFinished();
                    gameObject.SetActive(false);
                }
                break;
        }
    }
}
