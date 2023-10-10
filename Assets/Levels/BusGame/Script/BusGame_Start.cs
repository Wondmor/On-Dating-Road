using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BusGame_Start : MonoBehaviour
{
    public CommonSelection commonSelection;
    public Animator transition;
    public float transitionTime = 1.2f;

    public GameObject background;
    public GameObject guide;
    public Flowchart flowchart;

    void Start()
    {
        StartCoroutine(Plot());

    }

    public IEnumerator StartLoadLevel(int levelIndex)
    {
        yield return new WaitForSeconds(1f);
        bool hasClicked = false;
        background.SetActive(false);
        guide.SetActive(true);
        while (!hasClicked)
        {
            if (Input.anyKeyDown)
            {
                hasClicked = true;
            }

            yield return null;
        }
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
    public void SelectionResult(bool help)
    {
        commonSelection.gameObject.SetActive(false);
        if (help)
        {
            StartCoroutine(StartLoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
        else
        {
            GameLogicManager.Instance.OnMiniGameRefused();
        }
    }

    IEnumerator Plot()
    {
        yield return new WaitForSeconds(1f);
        flowchart.ExecuteBlock("Plot");
        bool anyKeyPressed = false;
        while (!anyKeyPressed)
        {
            // 检测是否有任意键被按下
            if (Input.anyKeyDown)
            {
                anyKeyPressed = true;
            }

            yield return null; // 等待下一帧
        }
        commonSelection.ShowChoice();
    }
}
