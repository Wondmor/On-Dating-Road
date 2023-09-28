using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingFlowControl : MonoBehaviour
{
    public enum GAME_STATUS
    {
        CHOOSE,
        INIT,
        START,
        PAUSE,
        STOP,
        END
    }

    // controllers
    RacingMoney moneyControl;
    RacingProgressDot progressControl;
    RacingTimer timerControl;
    RacingMapControl mapControl;
    RacingHealth healthControl;
    Timer timer;

    int bikeType = 0;

    [SerializeField]
    RacingPlayerControl playerControl;

    // Start is called before the first frame update
    void Start()
    {
        moneyControl = GetComponent<RacingMoney>();
        progressControl = GetComponent<RacingProgressDot>();
        timerControl = GetComponent<RacingTimer>();
        mapControl = GetComponent<RacingMapControl>();
        timer = GetComponent<Timer>();
        healthControl = GetComponent<RacingHealth>();
    }

    private void ResetEverything()
    {
        progressControl.SetPercent(0);
        timerControl.ResetClock();
        // pause count
        timerControl.PauseCount();
        mapControl.ResetMap();
        healthControl.ResetHealth();
    }

    public void SetGameStatus(GAME_STATUS status)
    {
        Debug.Log(status.ToString());
        switch (status)
        {
            case GAME_STATUS.INIT:
                ResetEverything();
                timer.Add(() => { SetGameStatus(GAME_STATUS.START); }, 3);
                break;
            case GAME_STATUS.PAUSE:
                timerControl.PauseCount();
                break;
            case GAME_STATUS.STOP:
                break;
            case GAME_STATUS.START:
                timerControl.StartCount();
                mapControl.GameStart();
                // I really want to use some delegation here, but let's keep it simple and easy
                playerControl.SetUpBikeType(bikeType);
                break;
            case GAME_STATUS.CHOOSE:
                ResetEverything();
                // TODO: choose!
                bikeType = Random.Range(0, 3);
                timer.Add(() => { SetGameStatus(GAME_STATUS.INIT); }, 1);
                break;
        }
    }

    public void Test()
    {
        SetGameStatus(GAME_STATUS.CHOOSE);
    }
}
