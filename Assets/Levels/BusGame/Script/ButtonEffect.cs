using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour
{
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("鼠标");
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }
}