using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingSelectionFlowControl : MonoBehaviour
{
    [SerializeField]
    GameObject phone;
    public void OnSelection(bool selected)
    {
        if(selected == true)
        {
            phone.SetActive(true);
        }
        else
        {
            GameLogicManager.Instance.OnMiniGameFinished(0, -5);
        }
    }

    private void Start()
    {
        GetComponent<CommonSelection>().ShowChoice();
    }
}
