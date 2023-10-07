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
        flowchart.SetStringVariable("Money", GetMoney().ToString() + "\u5143");
    }

    int GetMoney()
    {
        int timeUsed = GameManager.Instance.RacingData.TimeUsed;
        return timeUsed * 20;
    }


    public void Restart()
    {
        SceneManager.LoadScene("Racing");
    }

    public void FinishGame()
    {
        GameLogicManager.Instance.OnMiniGameFinished(15, 0);
    }

}
