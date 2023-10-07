using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Fail : MonoBehaviour
{

    public Sprite[] sprites;
    public Image image; 
    private void OnEnable() 
    {
        //int score = Score.playerScore;
        Debug.Log(Score.playerScore);
        switch(Score.playerScore)
        {
            case 0:
            {
                image.sprite = sprites[0];
                break;
            }
            case 1:
            {
                image.sprite = sprites[1];
                break;
            }
            case 2:
            {
                image.sprite = sprites[2];
                break;
            }
            case 3:
            {
                image.sprite = sprites[3];
                break;
            }
            case 4:
            {   
                image.sprite = sprites[4];
                break;
            }
            case 5:
            {
                image.sprite = sprites[5];
                break;
            }

        }
    }
}
