using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingData
{
    RacingPlayerControl.BIKE_TYPE bikeType = RacingPlayerControl.BIKE_TYPE.FIRE;
    int timeUsed = 0;

    public RacingPlayerControl.BIKE_TYPE BikeType { get => bikeType; set => bikeType = value; }
    public int TimeUsed { get => timeUsed; set => timeUsed = value; }
}
