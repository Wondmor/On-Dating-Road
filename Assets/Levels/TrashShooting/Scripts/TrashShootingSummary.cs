using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrashShootingSummary : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool IsFinished = false;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CommonInputAction.enter.WasPerformedThisFrame())
            IsFinished = true;
    }

    public void SetScore(uint result, float addMoney, float addPositiveComments, float passTimeRate)
    {
        
        //text.text = string.Format("{0} stars \n money +{1} \n positiveComment +{2} \n timeRate x{3}",
        //    result, addMoney, addPositiveComments, passTimeRate);
        
        text.text = string.Format("{0}",Mathf.RoundToInt(addMoney));
    }
}
