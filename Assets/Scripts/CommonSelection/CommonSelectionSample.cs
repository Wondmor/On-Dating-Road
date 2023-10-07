using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonSelectionSample : MonoBehaviour
{

    public CommonSelection selection;
    public void TestSelection(bool selected)
    {
        Debug.Log(selected);
    }
    private void Start()
    {
        selection.ShowChoice();
    }
}
