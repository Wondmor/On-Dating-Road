using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusGameEnding : MonoBehaviour
{

    public void Ending()
    {
        switch(Score.playerScore)
        {
            case 0:
            {
                GameLogicManager.Instance.OnMiniGameFinished(0,0);
                break;
            }
            case 1:
            {
                GameLogicManager.Instance.OnMiniGameFinished(2,4);
                break;
            }
            case 2:
            {
                GameLogicManager.Instance.OnMiniGameFinished(10,20);
                break;
            }
            case 3:
            {
                GameLogicManager.Instance.OnMiniGameFinished(16,32);
                break;
            }
            case 4:
            {   
                GameLogicManager.Instance.OnMiniGameFinished(20,40);
                break;
            }

        }
    }
}
