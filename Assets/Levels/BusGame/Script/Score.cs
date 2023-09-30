using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int playerScore;

    public void start()
    {
        playerScore = 0;
    }

    public void displayScore()
    {
        Debug.Log(playerScore);
    }
}
