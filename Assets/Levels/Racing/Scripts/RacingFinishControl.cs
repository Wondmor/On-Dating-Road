using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RacingFinishControl : MonoBehaviour
{
    [SerializeField]
    GameObject halfWayCanvas, finishCanvas;

    GameObject currentPhone;

    [SerializeField]
    Sprite failSprite, navSprite;

    Timer timer;

    bool waitingForEnter = false;
    bool finished = false;

    int step = 0;

    private void Awake()
    {
        timer = GetComponent<Timer>();
        finished = GameManager.Instance.RacingData.RaceTime == 2;
        if (finished)
        {
            halfWayCanvas.SetActive(false);
            finishCanvas.SetActive(true);
            currentPhone = finishCanvas.transform.Find("Phone").gameObject;
        }
        else
        {
            halfWayCanvas.SetActive(true);
            finishCanvas.SetActive(false);
            currentPhone = halfWayCanvas.transform.Find("Phone").gameObject;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        NextStep();
    }

    void NextStep()
    {
        if (finished)
        {
            FinishNextStep();
        }
        else
        {
            HalfwayNextStep();
        }
    }

    int GetMoney()
    {
        int timeUsed = GameManager.Instance.RacingData.TimeUsed;
        return timeUsed * 20;
    }

    void FinishNextStep()
    {
        switch (step)
        {
            case 0:
                // show phone
                currentPhone.GetComponentInChildren<TextMeshProUGUI>().text = string.Format("{0}\u5143", GetMoney());
                iTween.ValueTo(currentPhone, iTween.Hash(
                    "from", 0.0f,
                    "to", 1.0f,
                    "time", 2.0f,
                    "onupdate", "UpdatePhoneRectUp",
                    "onupdatetarget", gameObject,
                    "oncomplete", "OnComplete",
                    "oncompletetarget", gameObject,
                    "easetype", iTween.EaseType.linear
                    ));
                break;
            case 1:
                GameLogicManager.Instance.OnMiniGameFinished(GameManager.Instance.RacingData.Money - GetMoney(), 0);
                break;
        }
        step++;
    }

    void HalfwayNextStep()
    {
        waitingForEnter = false;
        switch (step)
        {
            case 0:
                // show phone
                iTween.ValueTo(currentPhone, iTween.Hash(
                    "from", 0.0f,
                    "to", 1.0f,
                    "time", 2.0f,
                    "onupdate", "UpdatePhoneRectUp",
                    "onupdatetarget", gameObject,
                    "oncomplete", "OnComplete",
                    "oncompletetarget", gameObject,
                    "easetype", iTween.EaseType.linear
                    ));
                break;
            case 1:
                currentPhone.GetComponent<Image>().sprite = failSprite;
                timer.Add(() =>
                {
                    waitingForEnter = true;
                }, 0.5f);
                break;
            case 2:
                currentPhone.GetComponent<Image>().sprite = navSprite;
                timer.Add(() =>
                {
                    waitingForEnter = true;
                }, 0.5f);
                break;
            case 3:
                SceneManager.LoadScene("Racing");
                break;
        }
        step++;
    }

    void UpdatePhoneRectUp(float newValue)
    {
        currentPhone.transform.localPosition = new Vector3(currentPhone.transform.localPosition.x, newValue * 1080f - 1080f, currentPhone.transform.localPosition.z);
    }

    void OnComplete()
    {
        waitingForEnter = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingForEnter)
        {
            if (GameManager.Instance.CommonInputAction.GetPerformedTypeThisFrame() == CommonInputAction.EType.Enter)
            {
                NextStep();
            }
        }
    }
}
