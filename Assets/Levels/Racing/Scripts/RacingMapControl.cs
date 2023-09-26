using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingMapControl : MonoBehaviour
{
    [SerializeField]
    GameObject roadPrefab, playGround, pondPrefab, lefPondPrefab, grassPrefab, singleGrassPrefab, catPrefab, stumpPrefab, hitEffectPrefab;

    [SerializeField]
    GameObject[] enemyPrefab;

    bool pause = false;
    float distanceCover = 0;

    GameObject[] roads;
    GameObject[] normalGrasses;
    Animator[] hitEffects; // very simple effect, just one playback

    List<RacingEnemy>[] enemies;

    [SerializeField]
    float roadHeight = 10.7f; // basiclly the road height
    [SerializeField]
    float normalGrassHeight = 5.36f; 
    [SerializeField]
    float totalDistance = 200f; // basiclly the road height

    RacingPlayerControl playerControl;
    RacingProgressDot progressDot;

    Timer timer;
    // Start is called before the first frame update
    void Start()
    {
        progressDot = GetComponent<RacingProgressDot>();
        timer = GetComponent<Timer>();
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

        // create list
        enemies = new List<RacingEnemy>[3];
        for(int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = new List<RacingEnemy>();
        }

        // stop rolling
        pause = true;
    }

    // Update is called once per frame
    static float updateItemsTime = 0;
    void Update()
    {
        if (pause)
            return;

        // Create other things
        if (updateItemsTime > 1.5f)
        {
            if (Random.value < 0.5)
            {
                CreateEnemy();
            }
            updateItemsTime = 0;
        }
        else
        {
            updateItemsTime += Time.deltaTime;
        }

        float distance = playerControl.VerticalSpeed * Time.deltaTime;
        // add cover distance
        distanceCover += distance;
        // set up distance marker
        progressDot.SetPercent(distanceCover / totalDistance);

        MoveLoopThings(distance);
        MoveEnemy(distance);

    }

    void MoveLoopThings(float distance)
    {
        for(int i = 0; i < roads.Length; i++)
        {
            float newY = roads[i].transform.localPosition.y;
            newY -= distance;
            if(newY < -10)
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
                // move bike
                var bike = enemies[bikeType][i];
                float newY = bike.transform.localPosition.y;
                newY -= distance;
                bike.transform.localPosition = new Vector3(bike.transform.localPosition.x, newY, bike.transform.localPosition.z);

                if(newY < -8)
                {
                    bike.gameObject.SetActive(false);
                }
            }
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
        timer.Add(() => { 
            currentEffect.gameObject.SetActive(false);
            currentEffect.transform.parent = transform;
        }, 0.8f);
        currentEffectIndex = (currentEffectIndex + 1) % hitEffects.Length;
    }

    public void CreateEnemy()
    {
        int bikeType = Random.Range(0, 3);
        RacingEnemy enemy = null;

        // grab used ones
        for(int i = 0; i < enemies[bikeType].Count; i++)
        {
            if (!enemies[bikeType][i].isActiveAndEnabled)
            {
                enemy = enemies[bikeType][i];
                enemy.ResetTransforms();
                break;
            }
        }

        // no? create one
        if(enemy == null)
        {
            enemy = Instantiate(enemyPrefab[bikeType], playGround.transform).GetComponent<RacingEnemy>();
            enemies[bikeType].Add(enemy);
        }

        enemy.transform.localPosition = new Vector3(Random.value * 3 - 3, 12, -0.9f);
        enemy.gameObject.SetActive(true);
    }
}
