using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RacingPlayerControl : MonoBehaviour
{
    public enum BIKE_TYPE
    {
        FIRE, 
        GRASS,
        WATER
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

    // controllers
    RacingMoney moneyControl;
    RacingProgressDot progressControl;
    RacingTimer timerControl;

    // bike type
    BIKE_TYPE bikeType;
    GameObject bike;

    [SerializeField]
    GameObject[] bikePrefabs;

    public float VerticalSpeed { get => verticalSpeed; set => verticalSpeed = value; }

    // Start is called before the first frame update
    void Start()
    {
        moneyControl = gameControl.GetComponent<RacingMoney>();
        progressControl = gameControl.GetComponent<RacingProgressDot>();
        timerControl = gameControl.GetComponent<RacingTimer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bike == null)
            return;

        SetSpeed();
        // get direction and speed
        Vector3 currentPos = transform.localPosition;
        currentPos.x += Input.GetAxis("Horizontal") * horizontalSpeed * Time.deltaTime;
        currentPos.x = Mathf.Clamp(currentPos.x, MIN_X, MAX_X);
        transform.localPosition = currentPos;
    }

    public void SetUpBikeType(int type)
    {
        bikeType = (BIKE_TYPE)type;
        if(bike != null)
        {
            Destroy(bike);
        }

        bike = Instantiate(bikePrefabs[(int)type], transform);
        bike.transform.localPosition = Vector3.zero;
    }

    public void ResetBike()
    {
        transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
    }

    void SetSpeed()
    {
        verticalSpeed = baseVerticalSpeed;
        // TODO! check if dead
        // check if pause
        // check if on grass
        bool onGrass = OnGrass();
        if(onGrass && bikeType != BIKE_TYPE.GRASS)
        {
            verticalSpeed = verticalSpeed * grassSpeedDownRatio;
        }
    }

    bool OnGrass()
    {
        // There are two ways we on grass
        // 1. we on looping grass
        if(transform.localPosition.x >= GRASS_POS)
        {
            return true;
        }
        return false;
    }
}
