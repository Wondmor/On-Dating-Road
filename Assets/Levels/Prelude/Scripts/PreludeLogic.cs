using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreludeLogic : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<TimelineLogic>().OnTimelineFinish += () => { GameLogicManager.Instance.OnPreludeFinished(); };
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
