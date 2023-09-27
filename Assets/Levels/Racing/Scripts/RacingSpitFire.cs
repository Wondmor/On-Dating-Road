using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RacingSpitFire : MonoBehaviour
{
    RacingPlayerControl playerControl;
    // Start is called before the first frame update
    void Start()
    {
        playerControl = GetComponentInParent<RacingPlayerControl>();
        Assert.IsNotNull(playerControl);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
