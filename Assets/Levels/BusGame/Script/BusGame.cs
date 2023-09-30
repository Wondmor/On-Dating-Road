using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Fungus;

public class BusGame : MonoBehaviour
{
    public LevelLoader levelLoader;
    public DialogBoxController dialogBoxController;
    public GameObject background;



    void Awake()
    {
        
        //StartCoroutine(BusGamePlot());
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            background.SetActive(false);
            StartCoroutine(wait(2));
            
        }
    }

    IEnumerator BusGamePlot()
    {
        dialogBoxController.SpawnDialogBox(new Vector3(-1.25f, 1.5f, 0), Quaternion.identity);
        yield return new WaitForSeconds(2);
        dialogBoxController.SpawnDialogBox(new Vector3(3.2f, 1.7f, 0), Quaternion.Euler(0, 180f, 0));
    }

    IEnumerator wait(float time)
    {

        yield return new WaitForSeconds(time);
        StartCoroutine(levelLoader.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        
    }
}
