using Fungus;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shopping : MonoBehaviour
{

    public enum Status
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
        public string giftname;
        public string sprite;
        public float price;
        public int type;
    }

    [Serializable]
    public struct ShopInfo
    {
        public ShopItem[] items;
    }

    ShopInfo shopInfo;

    [SerializeField]
    Flowchart flowchart;

    [SerializeField]
    GameObject itemPrefab, main;

    [SerializeField]
    TextMeshProUGUI moneyText;

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

    [SerializeField]
    EventSystem eventSystem;

    [SerializeField]
    GameObject trueButton, falseButton;

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

    bool pause = false;

    private void Awake()
    {
        TextAsset shopListJson = Resources.Load<TextAsset>("Shopping/shoplist");
        shopInfo = JsonUtility.FromJson<ShopInfo>(shopListJson.text);
        finalMainPosition = main.transform.localPosition + Vector3.up * 260;
        money = GameLogicManager.Instance.gameData.money;
    }
    // Start is called before the first frame update
    void Start()
    {
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
    }

    public void SetGameStatus(Status status)
    {
        this.status = status;
        switch (status)
        {
            case Status.INIT:
                break;
            case Status.MAIN:
                pause = false;
                flowchart.SetStringVariable("Desc", shopInfo.items[currentItem].description);
                flowchart.ExecuteIfHasBlock("ShowDesc");
                break;
            case Status.BUYING:
                pause = false;
                break;
            default:
                break;
        }
    }

    public void ShowLastLine()
    {
        showLastLine = true;
        currentItem = 12;
    }

    public void ShowGet()
    {
        dialog.ShowGet(items[currentItem].GetComponent<Image>().sprite, shopInfo.items[currentItem].name);
    }


    public void ShowGet(string name, Sprite sprite)
    {
        dialog.ShowGet(sprite, name);
    }

    public void UnPause()
    {
        pause = false;
    }

    public void ShoppingFinish(EGift gift = EGift.None)
    {
        if (gift == EGift.Free)
        {
            var freeGift = new ShopItem();
            freeGift.name = "千纸鹤";
            freeGift.price = 0;
            freeGift.sprite = "19";
            freeGift.description = "";
            freeGift.giftname = "宣传单折的千纸鹤";
            freeGift.type = (int)EGift.Free;

            gift = EGift.Free;
            GameLogicManager.Instance.OnShoppingFinished(gift, freeGift);
        }
        else
        {
            GameLogicManager.Instance.OnShoppingFinished(gift, shopInfo.items[currentItem]);
        }
    }

    // Update is called once per frame
    void Update()
    {

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

        if (pause)
            return;

        CommonInputAction.EType performed = GameManager.Instance.CommonInputAction.GetPerformedTypeThisFrame();
        if (performed != CommonInputAction.EType.None)
        {
            if (Time.fixedTime - lastInputTime > 0.1)
            {
                lastInputTime = Time.fixedTime;
            }
            else
            {
                return;
            }
        }


        if (status == Status.MAIN)
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
                flowchart.StopAllBlocks();
                flowchart.SetStringVariable("Desc", shopInfo.items[currentItem].description);
                flowchart.ExecuteIfHasBlock("ShowDesc");
                pause = true;
            }
            else if (performed == CommonInputAction.EType.Enter)
            {
                dialog.Setup(items[currentItem].GetComponent<Image>().sprite, shopInfo.items[currentItem].price);
                pause = true;
                flowchart.SetFloatVariable("Price", shopInfo.items[currentItem].price);
                flowchart.SetFloatVariable("CurrentMoney", money);
                flowchart.ExecuteIfHasBlock("ShowBuy");
            }
        }
        else if (status == Status.BUYING)
        {
            if (performed == CommonInputAction.EType.Directions)
            {
                Vector2 vec2 = GameManager.Instance.CommonInputAction.directions.ReadValue<Vector2>();
                Debug.Log(vec2);
                if (vec2.x < 0)
                {
                    eventSystem.SetSelectedGameObject(trueButton);
                }
                else if (vec2.x > 0)
                {
                    eventSystem.SetSelectedGameObject(falseButton);
                }
            }
            if (performed == CommonInputAction.EType.Enter)
            {
                if(!eventSystem.alreadySelecting)
                {
                    eventSystem.SetSelectedGameObject(trueButton);
                }
            }
        }
    }
}
