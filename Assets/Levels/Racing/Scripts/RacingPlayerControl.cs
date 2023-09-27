using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Collections.AllocatorManager;

public class RacingPlayerControl : MonoBehaviour
{
    public enum BIKE_TYPE
    {
        FIRE,
        GRASS,
        WATER
    }

    static public bool WinByType(BIKE_TYPE myType, BIKE_TYPE otherType)
    {
        if (myType == BIKE_TYPE.FIRE && otherType == BIKE_TYPE.GRASS)
            return true;
        if (myType == BIKE_TYPE.WATER && otherType == BIKE_TYPE.FIRE)
            return true;
        if (myType == BIKE_TYPE.GRASS && otherType == BIKE_TYPE.WATER)
            return true;

        return false;
    }

    [SerializeField]
    GameObject gameControl;

    [SerializeField]
    float horizontalSpeed;  // the speed we move horizontal

    [SerializeField]
    float grassSpeedDownRatio = 0.5f;

    [SerializeField]
    float baseVerticalSpeed; // base speed, never change

    float verticalSpeed;  // the speed we move vertical, thiss will affect the speed of a map rolling

    float MIN_X = -3, MAX_X = 3; // This is the width of the road
    float GRASS_POS = 1.8f;

    bool stun = false;
    bool recovering = false;

    // controllers
    RacingMoney moneyControl;
    RacingProgressDot progressControl;
    RacingTimer timerControl;
    RacingMapControl mapControl;
    RacingHealth healthControl;
    Timer timer;

    // bike type
    BIKE_TYPE bikeType;
    GameObject bike;

    [SerializeField]
    GameObject[] bikePrefabs;

    public float VerticalSpeed { get => verticalSpeed; set => verticalSpeed = value; }
    public bool Stun { get => stun; set => stun = value; }

    // Start is called before the first frame update
    void Start()
    {
        moneyControl = gameControl.GetComponent<RacingMoney>();
        progressControl = gameControl.GetComponent<RacingProgressDot>();
        timerControl = gameControl.GetComponent<RacingTimer>();
        mapControl = gameControl.GetComponent<RacingMapControl>();
        healthControl = gameControl.GetComponent<RacingHealth>();
        timer = gameControl.GetComponent<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bike == null)
            return;

        SetSpeed();

        if (!Stun && !recovering)
        {
            // get direction and speed
            Vector3 currentPos = transform.localPosition;
            float axis = Input.GetAxis("Horizontal");
            currentPos.x += axis * horizontalSpeed * Time.deltaTime;
            currentPos.x = Mathf.Clamp(currentPos.x, MIN_X, MAX_X);
            transform.localPosition = currentPos;

            // tilt?
            if (axis > 0)
            {
                bike.transform.rotation = Quaternion.AngleAxis(-5.0f, Vector3.forward);
            }
            else if (axis < 0)
            {
                bike.transform.rotation = Quaternion.AngleAxis(5.0f, Vector3.forward);
            }
            else
            {
                bike.transform.rotation = Quaternion.identity;
            }
        }

        if(recovering)
        {
            // show recovering blink
            if (Time.fixedTime % .5 < .2)
            {
                bike.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                bike.GetComponent<Renderer>().enabled = true;
            }
        }
        else
        {
            bike.GetComponent<Renderer>().enabled = true;
        }
    }

    public void SetUpBikeType(int type)
    {
        bikeType = (BIKE_TYPE)type;
        if (bike != null)
        {
            Destroy(bike);
        }

        bike = Instantiate(bikePrefabs[(int)type], transform);
        bike.transform.localPosition = Vector3.zero;
    }

    public void ResetBike()
    {
        transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        bike.transform.localRotation = Quaternion.identity;
    }

    void SetSpeed()
    {
        verticalSpeed = baseVerticalSpeed;
        // TODO! check if dead

        if (Stun)
        {
            verticalSpeed = 0;
            return;
        }

        if (healthControl.IsDead())
        {
            verticalSpeed = 0;
            return;
        }
        // check if pause
        // check if on grass
        bool onGrass = OnGrass();
        if (onGrass && bikeType != BIKE_TYPE.GRASS)
        {
            verticalSpeed = verticalSpeed * grassSpeedDownRatio;
        }
    }

    bool OnGrass()
    {
        // There are two ways we on grass
        // 1. we on looping grass
        if (transform.localPosition.x >= GRASS_POS)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HIT!");
        RacingEnemy enemy = collision.GetComponent<RacingEnemy>();
        if (enemy != null)
        {
            enemy.Hit();
            mapControl.ShowHitEffectAt(enemy.transform);
            Stun = true;

            if (!WinByType(bikeType, enemy.BikeType))
            {
                healthControl.AddHealth(-1);
                Rotate();
                // Reset
                ResetIn(0.5f, 2f);
            }
            else
            {
                // reset stun
                timer.Add(() => { Stun = false; }, 0.1f);
            }

            return;
        }

        RacingCoin coin = collision.GetComponent<RacingCoin>();
        if(coin != null)
        {
            coin.Hit();
        }
    }

    private void ResetIn(float time, float recoverTime)
    {
        if (recovering)
        {
            return;
        }
        timer.Add(() =>
        {
            recovering = true;
            ResetBike();
            GetComponent<BoxCollider2D>().enabled = false;
            // reset status in time
            timer.Add(() =>
            {
                Stun = false; recovering = false;
                GetComponent<BoxCollider2D>().enabled = true;
            }, recoverTime);
        }, time );
    }

    private void Rotate()
    {
        Animator animator = bike.GetComponent<Animator>();
        animator.SetTrigger("rotate");
    }


}
