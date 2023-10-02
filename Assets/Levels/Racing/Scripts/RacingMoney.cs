using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
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
    int currentAmount;
    // Start is called before the first frame update
    void Start()
    {
        ShowMoney();
    }

    static float time = 0;
    private void FixedUpdate()
    {
        if(time < 0.05)
        {
            time += Time.deltaTime;
            return;
        }
        else
        {
            time = 0;
        }
        if(amount > currentAmount)
        {
            currentAmount++;
            ShowMoney();
        }
        else if(amount < currentAmount)
        {
            currentAmount--;
            ShowMoney();
        }
    }

    void ShowMoney()
    {
        string text = "";
        foreach(var c in ((int)currentAmount).ToString())
        {
            text += string.Format("<sprite={0}>", c);
        }
        this.text.text = text;
    }

    public void AddMoney(int extra)
    {
        amount += extra;
    }

    public void ResetMoney()
    {
        amount = 0;
        currentAmount = 0;
    }


    public void SetMoney(int money)
    {
        amount = money;
        currentAmount = money;
        ShowMoney();
    }

    public int GetMoney()
    {
        return amount;
    }

    public void Test()
    {
        AddMoney(500);
    }
}
