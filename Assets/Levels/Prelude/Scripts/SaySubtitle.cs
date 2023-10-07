using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaySubtitle : MonoBehaviour
{
    [SerializeField] SayDialog dialog;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Say(string _text)
    {
        dialog.gameObject.SetActive(true);
        dialog.Say(_text, true, false, false, true, false, null, null);
    }
}
