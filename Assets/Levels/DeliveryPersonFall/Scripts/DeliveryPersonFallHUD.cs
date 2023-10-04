using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class DeliveryPersonFallHUD : MonoBehaviour
{
    public List<GameObject> heartsGOList;

    public void SetHeartCountObservable(IObservable<int> observable)
    {
        observable.Subscribe(count =>
        {
            for(int i = 0; i < heartsGOList.Count ; i++)
            {
                heartsGOList[i].SetActive(i < count);
            }
        }).AddTo(this);
    }
}
