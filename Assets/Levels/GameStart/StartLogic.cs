using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameLogicManager.Instance.GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
