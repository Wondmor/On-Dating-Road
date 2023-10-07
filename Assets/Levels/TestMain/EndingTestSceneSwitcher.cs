using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingTestSceneSwitcher : MonoBehaviour
{
    enum EEndingTest:int
    {
        BeLate,
        FreeGift,
        BadCheap,
        NiceCheap,
        BadNormal,
        NiceNormal,
        BadGood,
        NiceGood
    }

    List<string> names = new List<string>{
        "BeLate",
        "FreeGift",
        "BadCheap",
        "NiceCheap",
        "BadNormal",
        "NiceNormal",
        "BadGood",
        "NiceGood" };

    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI sliderText;
    Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();

        slider.onValueChanged.AddListener(OnSliderChange);
        button.onClick.AddListener(OnButtonClick);
    }

    public void OnSliderChange(float value)
    {
        sliderText.text = names[Mathf.RoundToInt(value)];
    }
    public void OnButtonClick()
    {
        EEndingTest type =  (EEndingTest)Mathf.RoundToInt(slider.value);

        EndingData data = new EndingData();
        Shopping.ShopItem giftInfo = new Shopping.ShopItem();

        switch(type)
        {
            case EEndingTest.BeLate:
                data.eEnding = EEnding.BeLate;
                data.eGift = EGift.None;
                break;
            case EEndingTest.FreeGift:
                data.eEnding = EEnding.BadCharacter;
                data.eGift = EGift.Free;
                giftInfo.sprite = "19";
                giftInfo.giftname = "传单折的千纸鹤";
                break;
            case EEndingTest.BadCheap:
                data.eEnding = EEnding.BadCharacter;
                data.eGift = EGift.Bad;
                giftInfo.sprite = "14";
                giftInfo.giftname = "爱情同心锁";
                break;
            case EEndingTest.NiceCheap:
                data.eEnding = EEnding.GoodCharacter;
                data.eGift = EGift.Bad;
                giftInfo.sprite = "14";
                giftInfo.giftname = "爱情同心锁";
                break;
            case EEndingTest.BadNormal:
                data.eEnding = EEnding.BadCharacter;
                data.eGift = EGift.Normal;
                giftInfo.sprite = "7";
                giftInfo.giftname = "吞吞吐吐牌设计师合作款时尚单肩包";
                break;
            case EEndingTest.NiceNormal:
                data.eEnding = EEnding.GoodCharacter;
                data.eGift = EGift.Normal;
                giftInfo.sprite = "7";
                giftInfo.giftname = "吞吞吐吐牌设计师合作款时尚单肩包";
                break;
            case EEndingTest.BadGood:
                data.eEnding = EEnding.BadCharacter;
                data.eGift = EGift.Good;
                giftInfo.sprite = "1";
                giftInfo.giftname = "镇店之宝安哥拉心形粉钻钻戒";
                break;
            case EEndingTest.NiceGood:
                data.eEnding = EEnding.GoodCharacter;
                data.eGift = EGift.Good;
                giftInfo.sprite = "1";
                giftInfo.giftname = "镇店之宝安哥拉心形粉钻钻戒";
                break;
        }


        giftInfo.type = (int)data.eGift;
        data.giftInfo = giftInfo;
        GameLogicManager.Instance.SetEndingDataForTest(data);
        SceneManager.LoadScene(transform.Find("Text").GetComponent<TextMeshProUGUI>().text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
