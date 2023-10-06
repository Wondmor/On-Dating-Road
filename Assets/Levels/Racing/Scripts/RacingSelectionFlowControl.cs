using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RacingSelectionFlowControl : MonoBehaviour
{
    [SerializeField]
    GameObject phone;
    [SerializeField]
    Flowchart flowchart;
    public void OnSelection(bool selected)
    {
        if(selected == true)
        {
            flowchart.ExecuteIfHasBlock("Select");
        }
        else
        {
            GameLogicManager.Instance.OnMiniGameRefused();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Racing");
    }

    public void Awake()
    {
        flowchart.SetBooleanVariable("PlayedGame", GameManager.Instance.RacingData.TimeUsed > 0);
    }
}
