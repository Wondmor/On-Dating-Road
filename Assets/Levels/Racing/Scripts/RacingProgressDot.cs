using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RacingProgressDot : MonoBehaviour
{
    [SerializeField]
    Transform startPos, endPos, dotPos;

    float percent = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dotPos.localPosition = Vector3.Lerp(startPos.localPosition, endPos.localPosition, percent);
    }

    public void SetPercent(float percent)
    {
        this.percent = percent;
    }

    private void Reset()
    {
        percent = 0;
    }
}
