using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalletFlowControl : MonoBehaviour
{
    [SerializeField]
    GameObject mainGame, preGame, postGame;

    [SerializeField]
    CommonSelection preChoice, finishChoice;
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
            mainGame.SetActive(true);
            preGame.SetActive(false);
            postGame.SetActive(false);
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
    }

    public void ShowFinishChoice()
    {
        finishChoice.ShowChoice();
    }

    public void EndGame(bool money)
    {
        if(money)
        {
            // show money
        }
        else
        {
            // show car
        }
    }
}
