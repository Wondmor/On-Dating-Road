using Fungus;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WalletGameControl
    : MonoBehaviour
{

    [Serializable]
    public struct WalletItemInfos
    {
        public WalletItemInfo[] items;
    }


    [Serializable]
    public struct WalletItemInfo
    {
        public string name;
        public string desc;
        public string sprite;
        public string small_sprite;
        public int count;
        public bool show;
        public bool finish;
    }

    public class MapInfo
    {
        public int type = 0;
        public bool visited = false;
        public int itemType = -1;
        public bool picked = false;
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    bool isMoving = false;

    [SerializeField]
    Sprite[] sprites;

    [SerializeField]
    WalletItem center, left, right, up, down;

    [SerializeField]
    GameObject tiles, pickedItemPrefab;

    [SerializeField]
    WalletFlowControl flowControl;

    [SerializeField]
    Flowchart flowchart;

    [SerializeField]
    AudioClip moveAudio, pickAudio;

    WalletItemInfos itemInfos;
    List<Sprite> itemSprites;
    List<Sprite> itemSmallSprites;


    MapInfo[,] map;
    List<int> itemsPicked;
    List<GameObject> pickedObject;

    int currentTileRow;
    int currentTileCol;
    int pickedCoin = 0;

    bool gamefinish = false;
    bool gamePaused = false;

    AudioSource sfx;

    // Start is called before the first frame update
    void Awake()
    {
        itemSprites = new List<Sprite>();
        itemSmallSprites = new List<Sprite>();
        itemInfos = JsonUtility.FromJson<WalletItemInfos>(Resources.Load<TextAsset>("Wallet/wallet_items").text);
        itemsPicked = new List<int>();
        pickedObject = new List<GameObject>();

        foreach (WalletItemInfo itemInfo in itemInfos.items)
        {
            itemSprites.Add(Resources.Load<Sprite>(string.Format("Wallet/{0}", itemInfo.sprite)));
            itemSmallSprites.Add(Resources.Load<Sprite>(string.Format("Wallet/{0}", itemInfo.small_sprite)));
        }

        GenerateMap();

        SetTile();
        SetPickedItems();

        sfx = gameObject.AddComponent<AudioSource>();
    }

    void GenerateMap()
    {
        List<int> items4Choose = new List<int>();

        for (int i = 0; i < itemInfos.items.Length; i++)
        {
            for (int j = 0; j < itemInfos.items[i].count; j++)
            {
                items4Choose.Add(i);
            }
        }


        int mapSize = 2;
        map = new MapInfo[mapSize * 3, mapSize * 3];

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                map[i, j] = new MapInfo();
                map[i, j].picked = false;
            }
        }

        while (items4Choose.Count > 0)
        {
            foreach (MapInfo mapInfo in map)
            {
                if (items4Choose.Count <= 0)
                {
                    break;
                }

                if (Random.value < 0.1 && mapInfo.itemType == -1)
                {
                    int choose = Random.Range(0, items4Choose.Count);
                    mapInfo.itemType = items4Choose[choose];
                    items4Choose.RemoveAt(choose);
                }
            }
        }

        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                int type = Random.Range(0, 2);
                map[i * 3, j * 3].type = type * 9 + 0;
                map[i * 3, j * 3 + 1].type = type * 9 + 1;
                map[i * 3, j * 3 + 2].type = type * 9 + 2;
                map[i * 3 + 1, j * 3].type = type * 9 + 3;
                map[i * 3 + 1, j * 3 + 1].type = type * 9 + 4;
                map[i * 3 + 1, j * 3 + 2].type = type * 9 + 5;
                map[i * 3 + 2, j * 3].type = type * 9 + 6;
                map[i * 3 + 2, j * 3 + 1].type = type * 9 + 7;
                map[i * 3 + 2, j * 3 + 2].type = type * 9 + 8;

            }
        }

        currentTileRow = 5;
        currentTileCol = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (gamefinish || gamePaused)
            return;

        if (GameManager.Instance.CommonInputAction.GetPerformedTypeThisFrame() == CommonInputAction.EType.Directions)
        {
            Vector2 vec2 = GameManager.Instance.CommonInputAction.directions.ReadValue<Vector2>();
            float angle = Vector2.SignedAngle(Vector2.right, vec2);
            Debug.Log(angle);
            if (angle > -45 && angle < 45)
            {
                MoveTile(Direction.Left);
            }
            else if (angle > 45 && angle < 135)
            {
                MoveTile(Direction.Down);
            }
            else if (angle > 135 || angle < -135)
            {
                MoveTile(Direction.Right);
            }
            else
            {
                MoveTile(Direction.Up);
            }
        }
        else if (GameManager.Instance.CommonInputAction.GetPerformedTypeThisFrame() == CommonInputAction.EType.Enter)
        {
            MapInfo currMap = map[currentTileCol, currentTileRow];
            if (!currMap.picked && currMap.itemType != -1)
            {
                Debug.Log(itemInfos.items[currMap.itemType].name);
                if (itemInfos.items[currMap.itemType].show)
                {
                    itemsPicked.Add(currMap.itemType);
                }
                else
                {
                    pickedCoin++;
                }
                currMap.picked = true;
                gamePaused = true;

                flowchart.SetIntegerVariable("ItemType", currMap.itemType);
                flowchart.ExecuteIfHasBlock("IngameItemNarrative");
            }
        }
    }
    public void TestMove(int d)
    {
        MoveTile((Direction)Random.Range(0, 4));
    }

    public void ItemPicked()
    {
        gamePaused = false;
        SetTile();
        SetPickedItems();
        sfx.clip = pickAudio;
        sfx.Play();

        int finishCnt = 0;
        foreach (int item in itemsPicked)
        {
            if (itemInfos.items[item].finish)
            {
                finishCnt++;
            }
        }


        if (finishCnt == 2)
        {
            flowchart.StopAllBlocks();
            flowchart.SetBooleanVariable("GameEnd", true);
            flowchart.ExecuteIfHasBlock("IngameItemNarrative");
        }
    }

    public void MoveTile(Direction direction)
    {
        if (isMoving)
        {
            return;
        }

        isMoving = true;
        sfx.clip = moveAudio;
        sfx.Play();

        Vector3 position = Vector3.zero;
        switch (direction)
        {
            case Direction.Left:
                position += Vector3.left * 1920;
                currentTileRow++;
                break;
            case Direction.Right:
                position += Vector3.right * 1920;
                currentTileRow--;
                break;
            case Direction.Up:
                position += Vector3.up * 1080;
                currentTileCol++;
                break;
            case Direction.Down:
                position += Vector3.down * 1080;
                currentTileCol--;
                break;
        }

        if (currentTileRow < 0)
        {
            currentTileRow = map.GetLength(0) - 1;
        }
        if (currentTileRow >= map.GetLength(0))
        {
            currentTileRow = 0;
        }

        if (currentTileCol < 0)
        {
            currentTileCol = map.GetLength(1) - 1;
        }
        if (currentTileCol >= map.GetLength(1))
        {
            currentTileCol = 0;
        }

        iTween.MoveTo(tiles, iTween.Hash("islocal", true, "time", 0.2f, "position", position, "oncomplete", "OnCompleteMove", "oncompletetarget", gameObject, "easetype", iTween.EaseType.linear));

    }

    void OnCompleteMove()
    {
        isMoving = false;
        SetTile();
    }

    void SetTile()
    {
        Debug.Log("Current: " + currentTileCol + currentTileRow);
        int leftRow = currentTileRow - 1;
        int rightRow = currentTileRow + 1;
        int upCol = currentTileCol - 1;
        int downCol = currentTileCol + 1;

        leftRow = leftRow < 0 ? map.GetLength(0) - 1 : leftRow;
        rightRow = rightRow >= map.GetLength(0) ? 0 : rightRow;
        upCol = upCol < 0 ? map.GetLength(1) - 1 : upCol;
        downCol = downCol >= map.GetLength(1) ? 0 : downCol;

        SetupImageForMap(center, currentTileCol, currentTileRow);
        SetupImageForMap(left, currentTileCol, leftRow);
        SetupImageForMap(right, currentTileCol, rightRow);
        SetupImageForMap(up, upCol, currentTileRow);
        SetupImageForMap(down, downCol, currentTileRow);
        tiles.transform.localPosition = Vector3.zero;
    }

    void SetupImageForMap(WalletItem mapItem, int col, int row)
    {
        Sprite tilesprite = sprites[map[col, row].type];
        Sprite itemsprite = null;
        int itemType = map[col, row].itemType;
        if (map[col, row].itemType >= 0 && !map[col, row].picked)
        {
            itemsprite = itemSprites[itemType];
        }
        mapItem.Set(tilesprite, itemsprite);
    }

    void SetPickedItems()
    {
        while (pickedObject.Count < itemsPicked.Count)
        {
            GameObject tmp = Instantiate(pickedItemPrefab, transform);
            tmp.GetComponent<Image>().sprite = itemSmallSprites[itemsPicked[pickedObject.Count]];
            int index = pickedObject.Count;
            tmp.transform.localPosition = new Vector3(index / 5 * (-200) + 870, index % 5 * 200 + -430, 0);
            pickedObject.Add(tmp);
        }
    }
}
