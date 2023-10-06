using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BusGame_Start : MonoBehaviour
{
    public CommonSelection commonSelection;
    public Animator transition;
    public float transitionTime = 1.2f;

    public GameObject background;
    public GameObject guide;

    void Start()
    {
        commonSelection.ShowChoice();

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

        }
    }
}
