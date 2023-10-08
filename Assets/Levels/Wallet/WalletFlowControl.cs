using Fungus;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WalletFlowControl : MonoBehaviour
{
    [SerializeField]
    GameObject mainGame, preGame, postGame;

    [SerializeField]
    CommonSelection preChoice, finishChoice;

    [SerializeField]
    TextMeshProUGUI phoneMoneyText;

    [SerializeField]
    Flowchart flowchart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame(bool start)
    {
        if(start)
        {
            flowchart.ExecuteIfHasBlock("StartGame");
        }
        else
        {
            GameLogicManager.Instance.OnMiniGameRefused();
        }
    }

    public void ShowChoice()
    {
        preChoice.ShowChoice();
    }

    public void FinishGame()
    {
        mainGame.SetActive(false);
        preGame.SetActive(false);
        postGame.SetActive(true);
        flowchart.ExecuteIfHasBlock("End");
    }

    public void ShowFinishChoice()
    {
        finishChoice.ShowChoice();
    }

    public void EndGame(bool money)
    {
        if(money)
        {
            phoneMoneyText.text = "38888\u5143";
            // show money
            flowchart.ExecuteIfHasBlock("Money");
        }
        else
        {
            
            // show car
            flowchart.ExecuteIfHasBlock("Car");
        }
    }

    public void EndWithCar()
    {
        GameLogicManager.Instance.OnCoinSkillFinished(0, 20);
    }

    public void EndWithMoney()
    {
        GameLogicManager.Instance.OnCoinSkillFinished(38888, 0);
    }
}
