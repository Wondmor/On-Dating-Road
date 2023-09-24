using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollingNumberText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void UpdateValue(float newValue)
    {
        gameObject.GetComponent<Text>().text = Mathf.RoundToInt(newValue).ToString();
    }






}
