using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DialogBoxController : MonoBehaviour
{
    public GameObject dialogBox;
    
    void Update() 
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            SpawnDialogBox(worldPosition,Quaternion.identity);
        }
        */



    }

    
    public GameObject SpawnDialogBox(Vector3 place, Quaternion rotation)
    {
        Debug.Log(place);
        GameObject box = Instantiate(dialogBox, place, rotation);
        return box;
    }

    
}
