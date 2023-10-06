using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingLogic : MonoBehaviour
{
    [SerializeField] GameObject BeLate = null;


    // Start is called before the first frame update
    void Start()
    {
        var endingData = GameLogicManager.Instance.endingData;

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CommonInputAction.enter.WasPerformedThisFrame())
            GameLogicManager.Instance.OnEndingFinished();
    }


    public void GetGiftInfo(bool bGet, ref Sprite sprite, ref string giftName)
    {
        //thtodo
    }
}
