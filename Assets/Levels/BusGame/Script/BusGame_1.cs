using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;
using UnityEngine.SceneManagement;

public class BusGame_1 : MonoBehaviour
{
    public DialogBoxController dialogBoxController;
    public LevelLoader levelLoader;
    public Flowchart flowchart;
    public GameObject[] child;
    public GameObject choice;

    public Image countTime;
    Sprite newSprite;


    //int playerScore = Score.playerScore;

    bool pass = false;
    int level = 1;

    float totalTime = 5.0f;
    float currentTime;


    Vector2 minBounds;  // 移动范围的左下角边界
    Vector2 maxBounds;  // 移动范围的右上角边界
    float moveInterval = 0.1f; // 移动间隔时间
    float moveDuration = 1.0f; // 移动持续时间
    bool isMoving = false;



    void Awake()
    {
        StartCoroutine(Plot_1());
        currentTime = totalTime;
    }

    void Update()
    {
        currentTime -= Time.deltaTime;

        if(currentTime<=0)
        {
            Debug.Log("fail");
        }
        if(pass)
        {
            currentTime = totalTime;
            pass = false;
            level += 1;
            switch (level)
            {
                case 1:
                {
                    StartCoroutine(Plot_1());
                    break;
                }
                case 2:
                {
                    StartCoroutine(Plot_2());
                    break;
                }
                case 3:
                {
                    StartCoroutine(Plot_3());
                    break;
                }
                case 4:
                {
                    StartCoroutine(Plot_4());
                    break;
                }
                case 5:
                {
                    StartCoroutine(Plot_5());
                    break;
                }
                case 6:
                {
                    StartCoroutine(Plot_6());
                    break;
                }
            }
        }

    }

    IEnumerator Plot_1()
    {
        yield return new WaitForSeconds(1f);
        child[0].SetActive(true);
        Debug.Log("Plot_1");
        flowchart.ExecuteBlock("Plot_1");
        /*
        Transform playerTransform = child[0].transform;
        Vector3 moveDirection = new Vector3(-1,0,0);
        while (playerTransform.position.x > -1.0f)
        {
            playerTransform.Translate(moveDirection * 2 * Time.deltaTime);
            yield return null;
        }
        */
        /*
        dialogBoxController.SpawnDialogBox(new Vector3(-4f, -4f, 0), Quaternion.identity);
        yield return new WaitForSeconds(2);
        dialogBoxController.SpawnDialogBox(new Vector3(4f, -4f, 0), Quaternion.Euler(0, 180f, 0));
        yield return new WaitForSeconds(1f);
        */

        yield return new WaitForSeconds(1f);
        choice.SetActive(true);

    }

     IEnumerator Plot_2()
     {
        yield return new WaitForSeconds(1f);
        Debug.Log("Plot_2");
        child[1].SetActive(true);
        Transform transform = child[1].transform;
        Vector3 moveDirection = new Vector3(0,0,0);
        flowchart.ExecuteBlock("Plot_2");

        yield return new WaitForSeconds(1f);
        choice.SetActive(true);


        while (true)
        {
            if (!isMoving)
            {
                isMoving = true;

                // 随机生成目标位置
                Vector3 targetPosition = new Vector3(
                    Random.Range(minBounds.x, maxBounds.x),
                    Random.Range(minBounds.y, maxBounds.y),
                    transform.position.z
                );

                // 计算移动方向和速度
                Vector3 direction = (targetPosition - transform.position).normalized;
                float startTime = Time.time;
                float elapsedTime = 0f;

                // 在一定时间内平滑移动到目标位置
                while (elapsedTime < moveDuration)
                {
                    transform.position += direction * Time.deltaTime * 5;
                    elapsedTime = Time.time - startTime;
                    yield return null;
                }

                isMoving = false;

                // 等待一段时间再进行下一次移动
                yield return new WaitForSeconds(moveInterval);
            }

            yield return null;
        }
            
        
     }
     IEnumerator Plot_3()
     {
        yield return new WaitForSeconds(1f);
        Debug.Log("Plot_3");
        child[2].SetActive(true);
        flowchart.ExecuteBlock("Plot_3");

        yield return new WaitForSeconds(1f);
        choice.SetActive(true);
     }
     IEnumerator Plot_4()
     {
        yield return new WaitForSeconds(1f);
        Debug.Log("Plot_4");
        child[3].SetActive(true);
        flowchart.ExecuteBlock("Plot_4");

        yield return new WaitForSeconds(1f);
        choice.SetActive(true);
     }
     IEnumerator Plot_5()
     {
        yield return new WaitForSeconds(1f);
        Debug.Log("Plot_5");
        child[4].SetActive(true);
        flowchart.ExecuteBlock("Plot_5");

        yield return new WaitForSeconds(1f);
        choice.SetActive(true);
     }
     IEnumerator Plot_6()
     {
        yield return new WaitForSeconds(1f);
        Debug.Log("Plot_6");
        child[5].SetActive(true);
        flowchart.ExecuteBlock("Plot_6");

        yield return new WaitForSeconds(1f);
        choice.SetActive(true);
     }

    
    public void Yes()
    {
        Debug.Log("Yes");
        choice.SetActive(false);
        for(int i=0 ; i<=5 ; i++)
        {
            child[i].SetActive(false);
        }
        Score.playerScore += 1;
        Debug.Log(Score.playerScore);
        pass = true;
        if(Score.playerScore >= 6)
        {
            string targetSceneName = "BusGame_Success";
            StartCoroutine(levelLoader.LoadLevelByName(targetSceneName));
        }
        
    }
    public void No()
    {
        Debug.Log("No");
        choice.SetActive(false);
        
        string targetSceneName = "BusGame_Fail";
        StartCoroutine(levelLoader.LoadLevelByName(targetSceneName));
    }
}
