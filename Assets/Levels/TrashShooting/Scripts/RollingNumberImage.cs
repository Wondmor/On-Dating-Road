using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollingNumberImage : MonoBehaviour
{
    [SerializeField] int DigitCount = 8;
    [SerializeField] public Sprite[] digitSprites = null;
    List<Image> numbers = null;

    // Start is called before the first frame update
    void Start()
    {
        numbers = new List<Image>();
        for(int i = 0; i < DigitCount; ++i)
        {
            numbers.Add(transform.Find(i.ToString()).GetComponent<Image>());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void UpdateValue(float newValue)
    {
        int nValue = Mathf.RoundToInt( newValue);
        for(int i = 0; i < DigitCount; ++i) 
        {
            numbers[i].sprite = digitSprites[nValue%10];
            nValue /= 10;
        }
    }
}
