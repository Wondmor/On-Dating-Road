using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RacingMapControl : MonoBehaviour
{
    [SerializeField]
    GameObject roadPrefab, playGround, pondPrefab, lefPondPrefab, grassPrefab, singleGrassPrefab, catPrefab, hitEffectPrefab, coinPrefab, shitPrefab;

    [SerializeField]
    GameObject[] enemyPrefab;

    bool pause = false;
    float distanceCover = 0;

    GameObject[] roads;
    GameObject[] normalGrasses;
    Animator[] hitEffects; // very simple effect, just one playback

    List<RacingEnemy>[] enemies;
    List<RacingCoin> coins;
    List<GameObject> shits;
    List<RacingCat> cats;

    GameObject singleGrass;
    GameObject stumpAtGrass;
    GameObject pond;
    GameObject leftPond;

    [SerializeField]
    float roadHeight = 10.7f; // basiclly the road height
    [SerializeField]
    float normalGrassHeight = 5.36f;
    [SerializeField]
    float totalDistance = 200f; // basiclly the road height
    [SerializeField, Range(1, 10)]
    float itemUpdateFrequency = 10f; // min distance we update items
    [SerializeField, Range(10, 20)]
    float vanishDistance = 10f;

    float lastCatDistance = 0;

    RacingPlayerControl playerControl;
    RacingMoney moneyController;
    RacingProgressDot progressDot;

    Dictionary<string, LinkedList<float>> ItemQueues;

    Timer timer;
    // Start is called before the first frame update
    void Awake()
    {
        progressDot = GetComponent<RacingProgressDot>();
        timer = GetComponent<Timer>();
        moneyController = GetComponent<RacingMoney>();
        playerControl = playGround.GetComponentInChildren<RacingPlayerControl>();
        // create all the roads
        roads = new GameObject[4];
        for (int i = 0; i < roads.Length; i++)
        {
            roads[i] = Instantiate(roadPrefab, playGround.transform);
            roads[i].transform.localPosition = new Vector3(0, i * roadHeight, 0);
        }

        // create right side grass
        normalGrasses = new GameObject[8];
        for (int i = 0; i < normalGrasses.Length; i++)
        {
            normalGrasses[i] = Instantiate(grassPrefab, playGround.transform);
            normalGrasses[i].transform.localPosition = new Vector3(2.7f, i * normalGrassHeight - 8, 0.1f);
        }

        // create Hit effects, just assume we will not have more than 8 hit in one sec
        hitEffects = new Animator[8];
        for (int i = 0; i < hitEffects.Length; i++)
        {
            hitEffects[i] = Instantiate(hitEffectPrefab, playGround.transform).GetComponent<Animator>();
            hitEffects[i].gameObject.SetActive(false);
        }

        ItemQueues = new Dictionary<string, LinkedList<float>>();

        // create list
        enemies = new List<RacingEnemy>[3];
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = new List<RacingEnemy>();
        }
        ItemQueues.Add("enemies", new LinkedList<float>());


        coins = new List<RacingCoin>();
        ItemQueues.Add("coins", new LinkedList<float>());
        shits = new List<GameObject>();
        ItemQueues.Add("shits", new LinkedList<float>());

        cats = new List<RacingCat>();

        singleGrass = Instantiate(singleGrassPrefab, playGround.transform);
        singleGrass.transform.localPosition = new Vector3(1.7f, 0, -0.1f);
        stumpAtGrass = singleGrass.transform.Find("stump").gameObject;
        singleGrass.SetActive(false);

        pond = Instantiate(pondPrefab, playGround.transform);
        pond.SetActive(false);

        leftPond = Instantiate(lefPondPrefab, playGround.transform);
        leftPond.SetActive(false);

        // stop rolling
        pause = true;
    }

    // Update is called once per frame
    static float updateItemsDistance = 0;
    void Update()
    {
        if (pause)
            return;


        float distance = playerControl.VerticalSpeed * Time.deltaTime;
        // add cover distance
        distanceCover += distance;
        // set up distance marker
        progressDot.SetPercent(distanceCover / totalDistance);

        // Create other things
        // Creation should be based on distance rather than time
        if (updateItemsDistance > itemUpdateFrequency)
        {
            bool hasGround = CreateGround();
            if (!hasGround)
            {
                CreateItems();
            }
            updateItemsDistance = 0;
        }
        else
        {
            updateItemsDistance += distance;
        }


        MoveLoopThings(distance);

        MoveGround(distance);

        MoveEnemy(distance);
        MoveCoin(distance);
        MoveShit(distance);
        MoveCat(distance);

    }

    void MoveLoopThings(float distance)
    {
        for (int i = 0; i < roads.Length; i++)
        {
            float newY = roads[i].transform.localPosition.y;
            newY -= distance;
            if (newY < -10)
            {
                int previous = (roads.Length + i - 1) % roads.Length;
                newY = roads[previous].transform.localPosition.y + roadHeight;
            }

            roads[i].transform.localPosition = new Vector3(roads[i].transform.localPosition.x, newY, roads[i].transform.localPosition.z);
        }

        for (int i = 0; i < normalGrasses.Length; i++)
        {
            float newY = normalGrasses[i].transform.localPosition.y;
            newY -= distance;
            if (newY < -10)
            {
                int previous = (normalGrasses.Length + i - 1) % normalGrasses.Length;
                newY = normalGrasses[previous].transform.localPosition.y + normalGrassHeight;
            }

            normalGrasses[i].transform.localPosition = new Vector3(normalGrasses[i].transform.localPosition.x, newY, normalGrasses[i].transform.localPosition.z);
        }
    }

    void MoveGround(float distance)
    {
        if (singleGrass.activeSelf)
        {
            MoveSingleItem(distance, singleGrass.transform, -13);
        }

        if (pond.activeSelf)
        {
            MoveSingleItem(distance, pond.transform);
        }

        if (leftPond.activeSelf)
        {
            MoveSingleItem(distance, leftPond.transform);
        }
    }

    void MoveEnemy(float distance)
    {
        for (int bikeType = 0; bikeType < 3; bikeType++)
        {
            for (int i = 0; i < enemies[bikeType].Count; i++)
            {
                if (!enemies[bikeType][i].isActiveAndEnabled)
                {
                    continue;
                }
                MoveSingleItem(distance, enemies[bikeType][i].transform, -8, 2 + Random.value * 1.5f);
            }
        }
    }

    void MoveCoin(float distance)
    {
        foreach (RacingCoin coin in coins)
        {
            if (!coin.isActiveAndEnabled)
            {
                continue;
            }

            MoveSingleItem(distance, coin.transform);
        }
    }

    void MoveShit(float distance)
    {
        foreach (GameObject shit in shits)
        {
            if (!shit.activeSelf)
            {
                continue;
            }

            MoveSingleItem(distance, shit.transform);
        }
    }

    void MoveCat(float distance)
    {
        foreach (RacingCat cat in cats)
        {
            if (!cat.isActiveAndEnabled)
            {
                continue;
            }

            MoveSingleItem(distance, cat.transform);
        }
    }

    void MoveSingleItem(float distance, Transform item, float deactiveY = -8, float speed = 0)
    {
        float newY = item.transform.localPosition.y;
        newY -= distance - speed * Time.deltaTime;
        item.transform.localPosition = new Vector3(item.transform.localPosition.x, newY, item.transform.localPosition.z);

        if (newY < deactiveY)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void ResetMap()
    {
        for (int i = 0; i < roads.Length; i++)
        {
            roads[i].transform.localPosition = new Vector3(0, i * roadHeight, 0);
        }

        for (int i = 0; i < normalGrasses.Length; i++)
        {
            normalGrasses[i].transform.localPosition = new Vector3(2.7f, i * normalGrassHeight - 8.0f, 0.1f);
        }

        foreach (var coin in coins)
        {
            coin.gameObject.SetActive(false);
        }

        foreach (var shit in shits)
        {
            shit.gameObject.SetActive(false);
        }

        foreach (var cat in cats)
        {
            cat.gameObject.SetActive(false);
        }

        for (int bikeType = 0; bikeType < 3; bikeType++)
        {
            foreach(var bike in enemies[bikeType])
            {
                bike.gameObject.SetActive(false);
            }
        }

        pond.SetActive(false);
        singleGrass.SetActive(false);
        leftPond.SetActive(false);

        distanceCover = 0;
        lastCatDistance = 0;
        foreach (var itemQ in ItemQueues)
        {
            itemQ.Value.Clear();
        }

        pause = true;
    }

    public void GameStart()
    {
        pause = false;
    }

    public void GameStop()
    {
        pause = true;
    }

    static int currentEffectIndex = 0;
    public void ShowHitEffectAt(Transform transform)
    {
        Animator currentEffect = hitEffects[currentEffectIndex];
        currentEffect.transform.parent = transform;
        currentEffect.transform.localPosition = -0.9f * Vector3.forward;
        currentEffect.gameObject.SetActive(true);
        currentEffect.Play("hit");
        // take the easy way out
        timer.Add(() =>
        {
            currentEffect.gameObject.SetActive(false);
            currentEffect.transform.parent = this.transform;
        }, 0.8f);
        currentEffectIndex = (currentEffectIndex + 1) % hitEffects.Length;
    }

    public void ShowHitEffectAt(Vector3 position)
    {
        Animator currentEffect = hitEffects[currentEffectIndex];
        currentEffect.transform.position = position;
        currentEffect.gameObject.SetActive(true);
        currentEffect.Play("hit");
        // take the easy way out
        timer.Add(() =>
        {
            currentEffect.gameObject.SetActive(false);
            currentEffect.transform.parent = transform;
        }, 0.8f);
        currentEffectIndex = (currentEffectIndex + 1) % hitEffects.Length;
    }

    // We are trying to create grassland/pond here
    public bool CreateGround()
    {
        if (singleGrass.activeSelf || pond.activeSelf || leftPond.activeSelf)
        {
            return false;
        }
        if (Random.value > 0.1f)
        {
            return false;
        }

        float randomChoice = Random.value;
        // create grass
        if (!singleGrass.activeSelf && randomChoice < 0.5f)
        {
            singleGrass.SetActive(true);
            if (Random.value < 0.3f)
            {
                stumpAtGrass.SetActive(true);
            }
            else
            {
                stumpAtGrass.SetActive(false);
            }
            singleGrass.transform.localPosition = new Vector3(1.7f, 10f, -0.2f);

            return true;
        }
        else if (!leftPond.activeSelf && randomChoice < 0.75f)
        {
            leftPond.SetActive(true);
            leftPond.transform.localPosition = new Vector3(-2.25f, 10f, -0.2f);
            return true;
        }
        else if (!pond.activeSelf)
        {
            pond.SetActive(true);
            pond.transform.localPosition = new Vector3(Random.Range(-1.9f, 0.3f), 10f, -0.2f);
            return true;
        }

        return false;
    }

    public void CreateItems()
    {
        // This is the most important algo we are having in this game I think
        /* The Idea here is just to devide a road into 4 slots, and the 5th slot is constant grassland
           So we can put things into these slots, just to make sure they never touch each other.

           Another very important thing here is just to keep all the items amount "PROPER!"
           I came up with this very simple idea, we got a desired number, and we buff the chance of getting such
           item when amount is low, and nerf it when high.
         */
        const int desireEnemies = 2;
        const int desireCoins = 20;
        const int desireShits = 1;

        // update item queue
        foreach (var itemQ in ItemQueues)
        {
            while (itemQ.Value.First != null && distanceCover - itemQ.Value.First.Value > vanishDistance)
            {
                itemQ.Value.RemoveFirst();
            }
        }

        bool[] slots = { false, false, false, false, false };
        int usedSlots = 0;

        // cat is different
        if (distanceCover - lastCatDistance > 15)
        {
            if (Random.value < 0.1)
            {
                lastCatDistance = distanceCover;
                CreateCat(GetPositionBasedOnSlot(0) + Vector3.right * 6);
                usedSlots = slots.Length;
            }
        }

        // Create Enemy
        // Count first, this is super super bad, but I just don't want to write something more complecated
        int enemyCount = ItemQueues["enemies"].Count;

        while (usedSlots < 4 /* we can only use first four slots */ && RandBasedOnDesireNumber(desireEnemies, enemyCount))
        {
            enemyCount += 3;
            int slot = ChooseSlot(slots, 0, 4);
            usedSlots++;
            slots[slot] = true;
            CreateEnemy(GetPositionBasedOnSlot(slot));
            ItemQueues["enemies"].AddLast(distanceCover);
        }

        // Create Coin, we let coin took all the left places if possible
        int shitCount = ItemQueues["shits"].Count;

        // one shit per line
        if (usedSlots < 5 && RandBasedOnDesireNumber(desireShits, shitCount))
        {
            int slot = ChooseSlot(slots, 0, slots.Length);
            usedSlots++;
            slots[slot] = true;
            CreateShit(GetPositionBasedOnSlot(slot));
            ItemQueues["shits"].AddLast(distanceCover);
        }

        // Create Coin, we let coin took all the left places if possible
        int coinCount = ItemQueues["coins"].Count;

        // one coin per line
        if (usedSlots < 5 && RandBasedOnDesireNumber(desireCoins, coinCount))
        {
            int slot = ChooseSlot(slots, 0, slots.Length);
            usedSlots++;
            slots[slot] = true;
            CreateCoin(GetPositionBasedOnSlot(slot));
            ItemQueues["coins"].AddLast(distanceCover);
        }
    }

    static Vector3 GetPositionBasedOnSlot(int slot)
    {
        return new Vector3(-3.5f + 7.0f / 5.0f * (slot + 0.5f), 10f + Random.value, -3f);
    }

    private int ChooseSlot(bool[] slots, int start, int end)
    {
        int[] choices = new int[slots.Length];
        int index = 0;
        for (int i = start; i < end && i < slots.Length; i++)
        {
            if (!slots[i])
            {
                choices[index] = i;
                index++;
            }
        }

        return choices[Random.Range(0, index)];
    }

    private bool RandBasedOnDesireNumber(float desireNumber, float currentNumber)
    {
        float ratio = (currentNumber - desireNumber) / desireNumber + 0.8f; // this is to move the function graph left
        float chance = -ratio / Mathf.Sqrt(1 + ratio * ratio) + 1.0f;

        if (Random.value * 2 < chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CreateEnemy(Vector3 position)
    {
        int bikeType = Random.Range(0, 3);
        RacingEnemy enemy = null;

        // grab used ones
        for (int i = 0; i < enemies[bikeType].Count; i++)
        {
            if (!enemies[bikeType][i].isActiveAndEnabled)
            {
                enemy = enemies[bikeType][i];
                enemy.ResetTransforms();
                break;
            }
        }

        // no? create one
        if (enemy == null)
        {
            enemy = Instantiate(enemyPrefab[bikeType], playGround.transform).GetComponent<RacingEnemy>();
            enemies[bikeType].Add(enemy);
        }

        enemy.transform.localPosition = position + Vector3.back;
        enemy.gameObject.SetActive(true);
    }

    public void CreateCoin(Vector3 position)
    {
        RacingCoin coin = null;
        for (int i = 0; i < coins.Count; i++)
        {
            if (!coins[i].isActiveAndEnabled)
            {
                coin = coins[i];
                break;
            }
        }

        if (coin == null)
        {
            coin = Instantiate(coinPrefab, playGround.transform).GetComponent<RacingCoin>();
            coins.Add(coin);
        }

        coin.transform.localPosition = position;
        coin.SetupCoin(moneyController);
        coin.gameObject.SetActive(true);
    }

    public void CreateShit(Vector3 position)
    {
        GameObject shit = null;
        for (int i = 0; i < shits.Count; i++)
        {
            if (!shits[i].activeSelf)
            {
                shit = shits[i];
                break;
            }
        }

        if (shit == null)
        {
            shit = Instantiate(shitPrefab, playGround.transform);
            shits.Add(shit);
        }

        shit.transform.localPosition = position;
        shit.gameObject.SetActive(true);
    }

    public void CreateCat(Vector3 position)
    {
        RacingCat cat = null;
        for (int i = 0; i < cats.Count; i++)
        {
            if (!cats[i].isActiveAndEnabled)
            {
                cat = cats[i];
                break;
            }
        }

        if (cat == null)
        {
            cat = Instantiate(catPrefab, playGround.transform).GetComponent<RacingCat>();
            cats.Add(cat);
        }

        cat.transform.localPosition = position;
        cat.SetupCat();
        cat.gameObject.SetActive(true);
    }

    public void Test(int a)
    {
        RandBasedOnDesireNumber(8, a);
    }
}
