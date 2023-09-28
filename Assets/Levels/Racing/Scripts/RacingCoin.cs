using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class RacingCoin : MonoBehaviour
{
    [SerializeField]
    int MAX_NUM, MIN_NUM;

    Vector3 dest = new Vector3(5.57999992f,-4.6500001f, -3);

    int amount = 0;

    RacingMoney moneyController;

    public int Amount { get => amount; }

    public void SetupCoin(RacingMoney moneyController)
    {
        amount = Random.Range(MIN_NUM, MAX_NUM + 1);
        this.moneyController = moneyController;
    }

    public void Hit()
    {
        Vector3[] path = new Vector3[3];
        path[0] = transform.position; // start
        path[1] = (transform.position + dest) / 2 + Vector3.up * 1.0f; // peek
        path[2] = dest;


        iTween.MoveTo(gameObject, iTween.Hash(
            "path", path,
            "time", 0.8,
            "easetype", iTween.EaseType.linear,
            "oncomplete", "OnComplete",
            "oncompletetarget", gameObject));
    }

    public void OnComplete()
    {
        moneyController.AddMoney(Amount);
        gameObject.SetActive(false);
    }
}
