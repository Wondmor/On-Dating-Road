using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeliveryPersonFallYellowBlue
{
    public GameObject yellow;
    public GameObject blue;
}

public class DeliveryPersonFallAnimMgr : MonoBehaviour
{
    public List<DeliveryPersonFallYellowBlue> yellowBlueList;

    public void SetYellow(bool yellow)
    {
        foreach (var yellowBlue in yellowBlueList)
        {
            yellowBlue.yellow.SetActive(yellow);
            yellowBlue.blue.SetActive(!yellow);
        }
    }
}
