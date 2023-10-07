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
        flowchart.SetStringVariable("Money", (GameManager.Instance.RacingData.Money - GetMoney()).ToString() + "\u5143");
    }

    int GetMoney()
    {
        // get how many health do we have left
        int health = GameManager.Instance.RacingData.RemainHealth;
        int timeUsed = GameManager.Instance.RacingData.TimeUsed;

        int level = 1;
        float baseMoney = 20;

        if (health == 6 && timeUsed < 30)
        {
            level = 5;
            baseMoney *= 10;
        }
        else if(health >= 5 && timeUsed < 60)
        {
            level = 4;
            baseMoney *= 1;
        }
        else if(health >= 3 && timeUsed < 70)
        {
            level = 3;
            baseMoney *= 0.8f;
        }
        else if(health >= 2 && timeUsed < 90)
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
        GameLogicManager.Instance.OnMiniGameFinished(GetMoney(), 0);
    }

}
