using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RacingFinishControl : MonoBehaviour
{
    [SerializeField]
    Flowchart flowchart;
    private void Awake()
    {
        bool finished = GameManager.Instance.RacingData.RaceTime == 2;
        flowchart.SetBooleanVariable("Finished", finished);
        flowchart.SetStringVariable("Money", (GameManager.Instance.RacingData.Money - GetMoney(out _)).ToString() + "\u5143");
    }

    int GetMoney(out int level)
    {
        // get how many health do we have left
        int health = GameManager.Instance.RacingData.RemainHealth;
        int timeUsed = GameManager.Instance.RacingData.TimeUsed;

        float baseMoney = 20;

        if (health == 12 && timeUsed < 50)
        {
            level = 5;
            baseMoney *= 10;
        }
        else if (health >= 9 && timeUsed < 80)
        {
            level = 4;
            baseMoney *= 1;
        }
        else if (health >= 5 && timeUsed < 100)
        {
            level = 3;
            baseMoney *= 0.8f;
        }
        else if (health >= 3 && timeUsed < 120)
        {
            level = 2;
            baseMoney *= 0.6f;
        }
        else
        {
            level = 1;
            baseMoney *= 0.1f;
        }


        return (int)baseMoney;
    }


    public void Restart()
    {
        SceneManager.LoadScene("Racing");
    }

    public void FinishGame()
    {
        int level;
        float money = GetMoney(out level);
        GameManager.Instance.RacingData.Reset();
        GameLogicManager.Instance.OnMiniGameFinished(
            GameLogicManager.Instance.gameData.money + money, 
            GameLogicManager.Instance.gameData.positiveComment, 
            GameLogicManager.Instance.gameData.countDown - GameLogicManager.c_StandardGameDuration * (level >= 4 ? 0.7f : level >= 3 ? 1 : 1.2f));
    }

}
