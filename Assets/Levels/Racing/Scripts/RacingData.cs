using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingData
{
    RacingPlayerControl.BIKE_TYPE bikeType = RacingPlayerControl.BIKE_TYPE.FIRE;
    int timeUsed = 0;
    int money = 0;
    int raceTime = 0;

    public RacingPlayerControl.BIKE_TYPE BikeType { get => bikeType; set => bikeType = value; }
    public int TimeUsed { get => timeUsed; set => timeUsed = value; }
    public int Money { get => money; set => money = value; }
    public int RaceTime { get => raceTime; set => raceTime = value; }
}
