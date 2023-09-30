using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RacingSpitFire : MonoBehaviour
{
    RacingPlayerControl playerControl;
    SpriteRenderer spriteRender;
    // Start is called before the first frame update
    void Start()
    {
        playerControl = GetComponentInParent<RacingPlayerControl>();
        spriteRender = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(playerControl);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerControl != null)
        {
            if(!playerControl.Speedup)
            {
                spriteRender.enabled = false;
            }
            else
            {
                if (playerControl.Recovering)
                {
                    // show recovering blink
                    if (Time.fixedTime % .5 < .2)
                    {
                        spriteRender.enabled = false;
                    }
                    else
                    {
                        spriteRender.enabled = true;
                    }
                }
                else
                {
                    spriteRender.enabled = true;
                }
            }
        }
    }
}
