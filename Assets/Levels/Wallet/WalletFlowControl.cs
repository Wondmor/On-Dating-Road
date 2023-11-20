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
    CommonSelection finishChoice;

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
        GameManager.Instance.LogManager.Log("coinskill_choose", "choose", "car");
        GameLogicManager.Instance.OnCoinSkillFinished(GameLogicManager.Instance.gameData.money, 
            GameLogicManager.Instance.gameData.positiveComment,
            Mathf.Max( GameLogicManager.Instance.gameData.countDown, 0) + GameLogicManager.c_CoinSkillSaveTime); // Force no late
    }

    public void EndWithMoney()
    {
        GameManager.Instance.LogManager.Log("coinskill_choose", "choose", "money");
        GameLogicManager.Instance.OnCoinSkillFinished(GameLogicManager.Instance.gameData.money + 38888, 
            GameLogicManager.Instance.gameData.positiveComment,
            GameLogicManager.Instance.gameData.countDown);
    }
}
