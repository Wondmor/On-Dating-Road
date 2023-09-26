using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusGame_1 : MonoBehaviour
{
    public DialogBoxController dialogBoxController;
    public GameObject player;
    public GameObject Choice;

    public Image CountTime;
    Sprite newSprite;

    //public List<GameObject> Choice= new List<GameObject>();

    void Awake()
    {
        StartCoroutine(Plot_1());
        
    }

    IEnumerator Plot_1()
    {
        Debug.Log("1");
        yield return new WaitForSeconds(1f);
        player.SetActive(true);
        Transform playerTransform = player.transform;
        Vector3 moveDirection = new Vector3(-1,0,0);
        while (playerTransform.position.x > -1.0f) // 假设要移动到x坐标小于-5的位置
        {
            playerTransform.Translate(moveDirection * 2 * Time.deltaTime);
            yield return null; // 等待下一帧
        }

        Debug.Log("2");

        dialogBoxController.SpawnDialogBox(new Vector3(-4f, -4f, 0), Quaternion.identity);
        yield return new WaitForSeconds(2);
        dialogBoxController.SpawnDialogBox(new Vector3(4f, -4f, 0), Quaternion.Euler(0, 180f, 0));
        yield return new WaitForSeconds(1f);

        Debug.Log("3");
        Choice.SetActive(true);
        StartCoroutine(Countdown());

        Debug.Log("4");

    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1f);

        newSprite = Resources.Load<Sprite>("21");
        CountTime.sprite = newSprite;
        Debug.Log("3.2");
        
        yield return new WaitForSeconds(1f);

        newSprite = Resources.Load<Sprite>("22");
        CountTime.sprite = newSprite;
        Debug.Log("3.1");
        
        yield return new WaitForSeconds(1f);
        Debug.Log("3.0");
        yield return null; 
    }
    public void Yes()
    {
        Debug.Log("Yes");
        return;
    }
    public void No()
    {
        Debug.Log("No");
        return;
    }
}
