using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class RacingMoney : MonoBehaviour
{
    [SerializeField]
    TextMeshPro text;
    [SerializeField, Range(100, 150)]
    float speed;

    int amount;
    float currentAmount;
    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    private void Update()
    {
        Debug.Log(amount.ToString() + ":" + currentAmount.ToString());
        if(Mathf.Abs(amount - currentAmount) < 0.1f)
        {
            currentAmount = amount;
        }
        else if(amount > currentAmount)
        {
            currentAmount += Time.deltaTime * speed;
        }
        else if(amount < currentAmount)
        {
            currentAmount -= Time.deltaTime * speed;
        }
        ShowMoney();
    }

    void ShowMoney()
    {
        text.text = "";
        foreach(var c in ((int)currentAmount).ToString())
        {
            text.text += string.Format("<sprite={0}>", c);
        }
    }

    public void AddMoney(int extra)
    {
        amount += extra;
    }

    public void Reset()
    {
        amount = 0;
        currentAmount = 0;
    }
}
