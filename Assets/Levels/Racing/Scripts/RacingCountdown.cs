using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingCountdown : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites;

    [SerializeField]
    Timer timer;

    [SerializeField]
    RacingFlowControl racingFlowControl;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    GameObject renderers;

    int currentSprite = 0;
    // Start is called before the first frame update

    public void StartCountDown()
    {
        renderers.SetActive(true);
        currentSprite = 0;
        CountDown();
    }

    void CountDown()
    {
        spriteRenderer.sprite = sprites[currentSprite];
        currentSprite++;
        if(currentSprite == sprites.Length)
        {
            timer.Add(() =>
            {
                racingFlowControl.SetGameStatus(RacingFlowControl.GAME_STATUS.START);
                renderers.SetActive(false);
            }, 0.5f);
        }
        else
        {
            timer.Add(() => { CountDown(); }, 0.9f);
        }
    }
}
