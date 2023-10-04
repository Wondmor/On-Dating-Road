using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shopping : MonoBehaviour
{

    enum Status
    {
        INIT,
        MAIN,
        SECOND_ITEM,
        BUYING,
        SWITCHING,
        ASKING,
        ZERO,
        FINAL,
    }

    [Serializable]
    public struct ShopItem
    {
        public string name;
        public string description;
        public string sprite;
        public float price;
    }

    [Serializable]
    public struct ShopInfo
    {
        public string intro;
        public string lastLine;
        public string final;
        public string zero;
        public string praise;
        public string ask1;
        public string ask2;
        public string final_self1;
        public string final_self2;
        public string final_self3;
        public ShopItem[] items;
    }

    ShopInfo shopInfo;

    [SerializeField]
    TextMeshProUGUI mainText, moneyText;

    [SerializeField]
    GameObject itemPrefab, main;
    [SerializeField]
    Transform itemStartPosition, itemEndPosition;
    [SerializeField]
    ShoppingDialog dialog;

    [SerializeField]
    float speed = 3;

    [SerializeField]
    GameObject subtitle;

    [SerializeField]
    Sprite posterSprite, giftSprite;

    GameObject[] items;

    float marginX, marginY;

    float money;

    Status status;
    int currentItem = 0;
    float accumulateTime = 0;
    float lastInputTime = 0;

    bool showLastLine = false;
    Vector3 finalMainPosition;
    Vector3 mainDialogFinalPosition;

    EGift gift;

    TextMeshProUGUI subtitleText;

    private void Awake()
    {
        TextAsset shopListJson = Resources.Load<TextAsset>("Shopping/shoplist");
        shopInfo = JsonUtility.FromJson<ShopInfo>(shopListJson.text);
        finalMainPosition = main.transform.localPosition + Vector3.up * 260;
        mainDialogFinalPosition = mainText.transform.localPosition - Vector3.up * 240;
        subtitleText = subtitle.GetComponentInChildren<TextMeshProUGUI>();
        subtitle.SetActive(false);
        money = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        mainText.text = shopInfo.intro;

        marginX = (itemEndPosition.position.x - itemStartPosition.position.x) / 5;
        marginY = (itemEndPosition.position.y - itemStartPosition.position.y) / 2;

        items = new GameObject[shopInfo.items.Length];
        for (int i = 0; i < shopInfo.items.Length; i++)
        {
            items[i] = Instantiate(itemPrefab, main.transform);
            items[i].transform.position = itemStartPosition.position + Vector3.right * marginX * (i % 6) + Vector3.up * marginY * (i / 6);
            items[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(String.Format("Shopping/Sprites/{0}", shopInfo.items[i].sprite));
        }

        moneyText.text = String.Format("\uFFE5{0}", money);
        mainText.maxVisibleCharacters = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (accumulateTime > 0.05)
        {
            mainText.maxVisibleCharacters++;
            accumulateTime = 0;
        }
        else
        {
            accumulateTime += Time.deltaTime;
        }

        CommonInputAction.EType performed = GameManager.Instance.CommonInputAction.GetPerformedTypeThisFrame();
        if (Time.fixedTime - lastInputTime < 0.2)
        {
            performed = CommonInputAction.EType.None;
        }
        else if (performed != CommonInputAction.EType.None)
        {
            lastInputTime = Time.fixedTime;
        }

        if (performed == CommonInputAction.EType.Enter)
        {
            if (mainText.maxVisibleCharacters < mainText.GetParsedText().Length)
            {
                mainText.maxVisibleCharacters = mainText.GetParsedText().Length;
                return;
            }
        }

        if (status == Status.INIT)
        {
            if (performed != CommonInputAction.EType.None)
            {
                status = Status.MAIN;
                mainText.text = shopInfo.items[currentItem].description;
                mainText.maxVisibleCharacters = 0;
            }
        }
        else if (status == Status.MAIN)
        {
            if (performed == CommonInputAction.EType.Directions)
            {
                int currentRow = currentItem % 6;
                int currentLine = currentItem / 6;
                Vector2 vec2 = GameManager.Instance.CommonInputAction.directions.ReadValue<Vector2>();
                Debug.Log(vec2);
                if (vec2.x > 0)
                {
                    currentRow++;
                }
                else if (vec2.x < 0)
                {
                    currentRow--;
                }

                if (vec2.y > 0)
                {
                    currentLine--;
                }
                else if (vec2.y < 0)
                {
                    currentLine++;
                }

                currentRow = Math.Clamp(currentRow, 0, 5);
                currentLine = Math.Clamp(currentLine, 0 + ((showLastLine ? 2 : 0)), 1 + (showLastLine ? 1 : 0));

                currentItem = currentLine * 6 + currentRow;
                mainText.text = shopInfo.items[currentItem].description;
                mainText.maxVisibleCharacters = 0;
            }
            else if (performed == CommonInputAction.EType.Enter)
            {
                dialog.Setup(items[currentItem].GetComponent<Image>().sprite, shopInfo.items[currentItem].price);
                status = Status.BUYING;
            }

        }
        else if (status == Status.BUYING)
        {
            if (performed == CommonInputAction.EType.Enter)
            {
                dialog.Select(true);
                if (money >= shopInfo.items[currentItem].price)
                {
                    dialog.ShowGet(items[currentItem].GetComponent<Image>().sprite, shopInfo.items[currentItem].name);
                    if (currentItem < 6)
                    {
                        gift = EGift.Good;
                    }
                    else if (currentItem < 12)
                    {
                        gift = EGift.Normal;
                    }
                    else
                    {
                        gift = EGift.Bad;
                    }
                    Invoke("SetToFinal", 1f);
                }
                else
                {
                    dialog.NoEnough();
                    dialog.Invoke("Unshow", 0.5f);
                    if (showLastLine && money == 0)
                    {
                        status = Status.ZERO;
                    }
                    else
                    {
                        if (!showLastLine && money < shopInfo.items[11].price)
                        {
                            status = Status.ASKING;
                        }
                        else
                        {
                            status = Status.MAIN;
                        }
                    }
                }
            }
            else if (performed == CommonInputAction.EType.Cancel)
            {
                dialog.Select(false);
                dialog.Invoke("Unshow", 0.1f);
                if (money == 0)
                {
                    status = Status.ZERO;
                }
                else
                {
                    status = Status.MAIN;
                    if (!showLastLine && money < shopInfo.items[11].price)
                    {
                        status = Status.ASKING;
                    }
                }
            }
        }
        else if (status == Status.ASKING)
        {
            if (!subtitle.activeSelf)
            {
                subtitle.SetActive(true);
                subtitleText.text = shopInfo.ask1;
                mainText.text = "";
                mainText.maxVisibleCharacters = 0;
            }

            if (mainText.maxVisibleCharacters >= mainText.GetParsedText().Length)
            {
                if (performed == CommonInputAction.EType.Enter)
                {
                    if (subtitle.activeSelf && mainText.text == "")
                    {
                        mainText.text = shopInfo.lastLine;
                    }
                    else
                    {
                        status = Status.SWITCHING;
                    }
                }
            }
        }
        else if (status == Status.SWITCHING)
        {
            subtitle.SetActive(false);
            showLastLine = true;
            mainText.text = shopInfo.final;
            currentItem = 12;
            if (performed != CommonInputAction.EType.None)
            {
                mainText.text = shopInfo.items[currentItem].description;
                mainText.maxVisibleCharacters = 0;
                status = Status.MAIN;
            }
        }
        else if (status == Status.ZERO)
        {
            if (performed != CommonInputAction.EType.None)
            {
                FinalStep();
            }
        }
        else if (status == Status.FINAL)
        {
            if (performed != CommonInputAction.EType.None)
            {
                GameLogicManager.Instance.OnShoppingFinished(gift);
            }

        }

        for (int i = 0; i < items.Length; i++)
        {
            if (i == currentItem)
            {
                items[i].transform.localScale = Vector3.Lerp(items[i].transform.localScale, Vector3.one * 1.5f, Time.deltaTime * speed);
            }
            else
            {
                items[i].transform.localScale = Vector3.Lerp(items[i].transform.localScale, Vector3.one, Time.deltaTime * speed);
            }
        }

        if (showLastLine)
        {
            main.transform.localPosition = Vector3.Lerp(main.transform.localPosition, finalMainPosition, Time.deltaTime * speed);
            mainText.transform.localPosition = Vector3.Lerp(mainText.transform.localPosition, mainDialogFinalPosition, Time.deltaTime * speed);
        }

    }

    static int currentFinalStep = 0;
    void FinalStep()
    {
        switch (currentFinalStep)
        {
            case 0:
                subtitle.SetActive(true);
                subtitleText.text = shopInfo.ask2;
                mainText.text = "";
                mainText.maxVisibleCharacters = 0;
                break;
            case 1:
                mainText.text = shopInfo.zero;
                break;
            case 2:
                dialog.ShowGet(posterSprite, "宣传单");
                break;
            case 3:
                dialog.Unshow();
                break;
            case 4:
                subtitleText.text = shopInfo.final_self1;
                break;
            case 5:
                subtitleText.text = shopInfo.final_self2;
                break;
            case 6:
                subtitleText.text = shopInfo.final_self3;
                break;
            case 7:
                dialog.ShowGet(giftSprite, "宣传单折的千纸鹤");
                break;
            case 8:
                dialog.Unshow();
                break;
            case 9:
                mainText.text = shopInfo.praise;
                break;
            case 10:
                GameLogicManager.Instance.OnShoppingFinished(EGift.Bad);
                break;
            default:
                break;
        }
        currentFinalStep++;

    }

    void SetToFinal()
    {
        status = Status.FINAL;
    }
}
