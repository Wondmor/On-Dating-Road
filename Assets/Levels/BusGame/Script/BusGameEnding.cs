using Fungus;
using UnityEngine;

public class BusGameEnding : MonoBehaviour
{

    [SerializeField]
    Flowchart flowchart;
    private void Awake()
    {
        flowchart.SetIntegerVariable("Score", Score.playerScore);

        int money, comment;
        float time;
        GetMoneyCommentTime(out money, out comment, out time);

        flowchart.SetIntegerVariable("Money", money);
    }

    void GetMoneyCommentTime(out int money, out int comment, out float time)
    {
        money = 0;
        comment = 0;
        time = 1.0f;
        switch (Score.playerScore)
        {
            case 0:
                money = 0;
                comment = 0;
                time = 1.2f;
                break;
            case 1:
                money = 2;
                comment = 4;
                time = 1.1f;
                break;
            case 2:
                money = 10;
                comment = 20;
                time = 1.0f;
                break;
            case 3:
                money = 16;
                comment = 32;
                time = 0.8f;
                break;
            case 4:
                money = 20;
                comment = 40;
                time = 0.7f;
                break;
        }


    }

    public void Ending()
    {
        int money, comment;
        float time;
        GetMoneyCommentTime(out money, out comment, out time);
        GameLogicManager.Instance.OnMiniGameFinished(
           GameLogicManager.Instance.gameData.money + money,
           GameLogicManager.Instance.gameData.positiveComment + comment,
           GameLogicManager.Instance.gameData.countDown - GameLogicManager.c_StandardGameDuration * time);
    }
}
