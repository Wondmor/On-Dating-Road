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
    bool speedup = false;
    bool crossingLine = false;

    // controllers
    RacingMoney moneyControl;
    RacingProgressDot progressControl;
    RacingTimer timerControl;
    RacingMapControl mapControl;
    RacingHealth healthControl;
    RacingFlowControl flowControl;
    RacingAudioControl audioControl;
    Timer timer;

    // bike type
    BIKE_TYPE bikeType;
    GameObject bike;

    [SerializeField]
    GameObject[] bikePrefabs;

    public float VerticalSpeed { get => verticalSpeed; set => verticalSpeed = value; }
    public bool Stun { get => stun; set => stun = value; }
    public bool Recovering { get => recovering; private set => recovering = value; }
    public bool Speedup { get => speedup; private set => speedup = value; }
    public bool CrossingLine { get => crossingLine; set => crossingLine = value; }

    // Start is called before the first frame update
    void Start()
    {
        moneyControl = gameControl.GetComponent<RacingMoney>();
        progressControl = gameControl.GetComponent<RacingProgressDot>();
        timerControl = gameControl.GetComponent<RacingTimer>();
        mapControl = gameControl.GetComponent<RacingMapControl>();
        healthControl = gameControl.GetComponent<RacingHealth>();
        timer = gameControl.GetComponent<Timer>();
        flowControl = gameControl.GetComponent<RacingFlowControl>();
        audioControl = gameControl.GetComponent<RacingAudioControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bike == null)
            return;


        SetSpeed();

        if(flowControl.GetGameStatus() == RacingFlowControl.GAME_STATUS.CROSS_LINE || flowControl.GetGameStatus() == RacingFlowControl.GAME_STATUS.STOP)
        {
            bike.transform.localRotation = Quaternion.identity;
            transform.localPosition += Vector3.up * verticalSpeed * Time.deltaTime;
        }

        if(healthControl.IsDead() && flowControl.GetGameStatus() != RacingFlowControl.GAME_STATUS.DEAD)
        {
            flowControl.SetGameStatus(RacingFlowControl.GAME_STATUS.DEAD);
            GetComponent<BoxCollider2D>().enabled = false;
            return;
        }

        if (!Stun)
        {
            // get direction and speed
            Vector3 currentPos = transform.localPosition;
            float axis = Input.GetAxis("Horizontal");
            currentPos.x += axis * horizontalSpeed * Time.deltaTime;
            currentPos.x = Mathf.Clamp(currentPos.x, MIN_X, MAX_X);
            transform.localPosition = currentPos;

            // skill trigger?
            if(GameManager.Instance.CommonInputAction.GetPerformedTypeThisFrame() == CommonInputAction.EType.Enter)
            {
                TriggerSpeedUp();
            }

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

        if(Recovering)
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

    public void SetUpBikeType(BIKE_TYPE type)
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

    public void InitBike()
    {
        transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        bike.transform.localRotation = Quaternion.identity;
        stun = false;
        recovering = false;
        speedup = false;
        verticalSpeed = 0;
    }

    void SetSpeed()
    {
        verticalSpeed = baseVerticalSpeed;
        bool onGrass = false;
        // TODO! check if dead

        if (Stun)
        {
            verticalSpeed = 0;
        }

        else if (healthControl.IsDead())
        {
            verticalSpeed = 0;
        }
        else
        {

            if (recovering)
            {
                verticalSpeed *= 0.5f;
            }
            // check if pause
            // check if on grass
            onGrass = OnGrass();
            if (onGrass && bikeType != BIKE_TYPE.GRASS)
            {
                verticalSpeed = verticalSpeed * grassSpeedDownRatio;
            }

            if (Speedup)
            {
                verticalSpeed *= 2;
            }

            if (transform.localPosition.y > 50)
            {
                verticalSpeed = 0;
            }
        }

        if (flowControl.GetGameStatus() == RacingFlowControl.GAME_STATUS.START || 
            flowControl.GetGameStatus() == RacingFlowControl.GAME_STATUS.CROSS_LINE)
        {
            if (healthControl.IsDead() || recovering)
            {
                audioControl.StopPlayDrive();
            }
            else
            {
                audioControl.PlayDrive(onGrass);
            }
        }
    }

    public void TriggerSpeedUp()
    {
        if(flowControl.GetGameStatus() != RacingFlowControl.GAME_STATUS.START || Speedup || recovering || bikeType != BIKE_TYPE.FIRE)
        {
            return;
        }

        Speedup = true;
        audioControl.PlaySpeedup();

        timer.Add(() =>
        {
            Speedup = false;
        }, 10f);
    }

    bool OnGrass()
    {
        // There are two ways we on grass
        // 1. we on looping grass
        // 2. we on single grass
        if (transform.localPosition.x >= GRASS_POS)
        {
            return true;
        }
        else 
        {
            Vector2 worldPoint = transform.position;
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 30, 1 << 7);
            if(hit.collider != null)
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HIT! : " + collision.gameObject.name);
        RacingEnemy enemy = collision.GetComponent<RacingEnemy>();
        if (enemy != null)
        {
            enemy.Hit();
            mapControl.ShowHitEffectAt(enemy.transform.position);
            Stun = true;

            if (!WinByType(bikeType, enemy.BikeType))
            {
                healthControl.AddHealth(-1);
                Rotate();
                // Reset
                ResetIn(0.9f, 2f);
                audioControl.PlayHitBike(true);
            }
            else
            {
                // reset stun
                timer.Add(() => { Stun = false; }, 0.1f);
                audioControl.PlayHitBike(false);
            }


            return;
        }

        RacingCoin coin = collision.GetComponent<RacingCoin>();
        if(coin != null)
        {
            coin.Hit();

            audioControl.PlayCoin();
            return;
        }

        RacingShit shit = collision.GetComponent<RacingShit>();
        if (shit != null)
        {
            mapControl.ShowHitEffectAt(shit.transform.position);
            shit.Hit();

            Stun = true;

            healthControl.AddHealth(-1);
            Rotate();
            // Reset
            ResetIn(0.9f, 2f);

            audioControl.PlayHitShit();

            return;
        }

        RacingCat cat = collision.GetComponent<RacingCat>();
        if(cat != null)
        {
            cat.Hit();

            Stun = true;
            healthControl.AddHealth(-1);
            Rotate();
            // Reset
            ResetIn(0.9f, 2f);

            audioControl.PlayHitCat();

            return;
        }

        if(bikeType != BIKE_TYPE.WATER && collision.GetComponent<RacingPond>() != null)
        {
            Stun = true;
            healthControl.AddHealth(-1);
            Rotate();
            // Reset
            ResetIn(0.9f, 2f);

            audioControl.PlayHit();
        }

        if(collision.name == "FinishLine(Clone)")
        {
            flowControl.SetGameStatus(RacingFlowControl.GAME_STATUS.STOP);
            audioControl.PlayFinish();
            return;
        }
    }

    private void ResetIn(float time, float recoverTime)
    {
        if (Recovering)
        {
            return;
        }
        GetComponent<BoxCollider2D>().enabled = false;
        Speedup = false;
        timer.Add(() =>
        {
            Recovering = true;
            Stun = false;
            ResetBike();
            // reset status in time
            timer.Add(() =>
            {
                Recovering = false;
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
