using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SubtitlePlayer : MonoBehaviour
{
    [SerializeField]
    string subtitleFile;

    [SerializeField]
    float leastTime = 0.5f;

    [SerializeField]
    UnityEvent onSubtitleEnd;
    [SerializeField]
    UnityEvent<int> onSubtitleSection;

    [SerializeField]
    bool oneByOne;

    string[] subtitleText;
    int currentSubtitle = 0;
    int currentSection = 0;
    int currentChar = 0;
    float lastSubtitleTime = 0;

    TextMeshProUGUI tmText;

    // Start is called before the first frame update
    void Awake()
    {
        subtitleText = Resources.Load<TextAsset>(string.Format("Subtitles/{0}", subtitleFile)).text.Split("\n");
        tmText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        NextLine();
    }

    void NextLine()
    {
        if(currentSubtitle >= subtitleText.Length)
        {
            return;
        }
        lastSubtitleTime = Time.fixedTime;
        currentChar = 0;

        string text = subtitleText[currentSubtitle];
        if(text == "section")
        {
            onSubtitleSection?.Invoke(currentSection);
            currentSection++;
        }
        else if(text == "end")
        {
            onSubtitleEnd?.Invoke();
        }
        else
        {
            tmText.text = text;
            if(oneByOne)
            {
                tmText.maxVisibleCharacters = 0;
                accumulateTime = 0.1f;
            }
        }
        currentSubtitle++;
    }

    // Update is called once per frame
    static float accumulateTime = 0;
    void Update()
    {
        if(Time.fixedTime - lastSubtitleTime < leastTime)
        {
            return;
        }

        if(oneByOne && currentChar < tmText.GetParsedText().Length)
        {
            if(GameManager.Instance.CommonInputAction.GetPerformedTypeThisFrame() == CommonInputAction.EType.Enter)
            {
                currentChar = tmText.GetParsedText().Length;
                tmText.maxVisibleCharacters = currentChar;
                return;
            }

            if(accumulateTime > 0.1)
            {
                currentChar++;
                tmText.maxVisibleCharacters = currentChar;
                accumulateTime = 0;
            }
            else
            {
                accumulateTime += Time.deltaTime;
            }
            return;
        }

        if(GameManager.Instance.CommonInputAction.GetPerformedTypeThisFrame() == CommonInputAction.EType.Enter)
        {
            NextLine();
        }
    }

    public void LoadSubtitle(string filename)
    {
        subtitleText = Resources.Load<TextAsset>(string.Format("Subtitles/{0}", filename)).text.Split("\n");
        currentSection = 0;
        currentSubtitle = 0;
        accumulateTime = 0;
        NextLine();
    }
}
