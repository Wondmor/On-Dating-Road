using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RacingFlowControl : MonoBehaviour
{
    public enum GAME_STATUS
    {
        NONE,
        INIT,
        START,
        PAUSE,
        STOP,
        DEAD,
        CROSS_LINE,
        END
    }

    // controllers
    RacingMoney moneyControl;
    RacingProgressDot progressControl;
    RacingTimer timerControl;
    RacingMapControl mapControl;
    RacingHealth healthControl;
    Timer timer;

    [SerializeField]
    RacingPlayerControl playerControl;

    [SerializeField]
    RacingCountdown countdown;

    [SerializeField]
    GameObject fadePrefab;

    GAME_STATUS status = GAME_STATUS.NONE;

    // Start is called before the first frame update
    void Start()
    {
        moneyControl = GetComponent<RacingMoney>();
        progressControl = GetComponent<RacingProgressDot>();
        timerControl = GetComponent<RacingTimer>();
        mapControl = GetComponent<RacingMapControl>();
        timer = GetComponent<Timer>();
        healthControl = GetComponent<RacingHealth>();

        SetGameStatus(GAME_STATUS.INIT);
    }

    private void ResetEverything()
    {
        progressControl.SetPercent(0);
        timerControl.SetClock(GameManager.Instance.RacingData.TimeUsed);
        // pause count
        timerControl.PauseCount();
        mapControl.ResetMap();
        healthControl.ResetHealth();

        moneyControl.SetMoney(GameManager.Instance.RacingData.Money);
    }

    public GAME_STATUS GetGameStatus()
    {
        return status;
    }
    public void SetGameStatus(GAME_STATUS status)
    {
        this.status = status;

        Debug.Log(status.ToString());
        switch (status)
        {
            case GAME_STATUS.INIT:
                timer.Clear();
                ResetEverything();
                playerControl.SetUpBikeType(GameManager.Instance.RacingData.BikeType);
                //fade in
                var fade = Instantiate(fadePrefab).GetComponent<FadeInOutScene>();
                fade.fadeType = FadeInOutScene.EType.FadeIn;
                fade.lastInSecond = 0.5f;
                timer.Add(() =>
                {
                    countdown.StartCountDown();
                }, 0.5f);
                break;
            case GAME_STATUS.PAUSE:
                timerControl.PauseCount();
                break;
            case GAME_STATUS.STOP:
                GameManager.Instance.RacingData.Money = moneyControl.GetMoney();
                GameManager.Instance.RacingData.TimeUsed = timerControl.GetTime();
                GameManager.Instance.RacingData.RaceTime++;
                timer.Add(() =>
                {
                    SceneManager.LoadScene("RacingFinish");
                }, 1f);
                break;
            case GAME_STATUS.START:
                timerControl.StartCount();
                mapControl.GameStart();
                break;
            case GAME_STATUS.DEAD:
                // when dead set time and money into racing data
                GameManager.Instance.RacingData.Money = moneyControl.GetMoney();
                GameManager.Instance.RacingData.TimeUsed = timerControl.GetTime();
                break;
            case GAME_STATUS.CROSS_LINE:
                break;
        }
    }
}
