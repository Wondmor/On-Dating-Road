using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int playerScore = 0;

    public void displayScore()
    {
        Debug.Log(playerScore);
    }
}
