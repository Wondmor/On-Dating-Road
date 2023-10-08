using Fungus;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class SkewerGameController : MonoBehaviour
{
    public enum GameStatus
    {
        INIT,
        OPENING,
        MAIN,
        FINISH,
        END,
    }

    public enum IngameSubType
    {
        WRONG_MEAT,
        WRONG_STICK,
        CANCEL,
        CANCEL_CONFIRM,
        CANCEL_RECALL,
    }


    [SerializeField]
    TextMeshProUGUI NumberText;

    [SerializeField]
    GameObject canvas, endCanvas, playGround, stickPrefab, meatPrefab, workingSelection, finishSelection;

    [SerializeField]
    Flowchart flowchart;

    [SerializeField]
    float margin;

    LinkedList<SkewerStick> sticks;
    LinkedList<SkewerStick> finishedSticks;

    LinkedList<SkewerMeat> meats;

    SkewerMeat singleMeat;
    SkewerStick singleStick;

    Timer timer;

    GameStatus status;

    int totalNumber = 0;

    bool preventInput = false;

    int currentMeat = 0;

    string[] ingameSubs;

    void Awake()
    {
        timer = gameObject.AddComponent<Timer>();

        ingameSubs = Resources.Load<TextAsset>("Subtitles/SkewerIngame").text.Split("\n");
    }

    void InitializeMainGame()
    {
        playGround.SetActive(true);
        sticks = new LinkedList<SkewerStick>();
        finishedSticks = new LinkedList<SkewerStick>();
        meats = new LinkedList<SkewerMeat>();

        singleMeat = Instantiate(meatPrefab, playGround.transform).GetComponent<SkewerMeat>();
        singleMeat.gameObject.SetActive(false);
        singleStick = Instantiate(stickPrefab, playGround.transform).GetComponent<SkewerStick>();
        singleStick.gameObject.SetActive(false);

        for (int i = 0; i < 17; i++)
        {
            var stick = Instantiate(stickPrefab, playGround.transform).GetComponent<SkewerStick>();
            stick.transform.localPosition = new Vector3(0 - i * margin, -1, 0);
            if (i > 4)
            {
                if (Random.value < 0.8)
                {
                    stick.SetupStick(SkewerStick.StickType.NORMAL);
                }
                else
                {
                    stick.SetupStick((SkewerStick.StickType)Random.Range((int)SkewerStick.StickType.CHOPSTICK, (int)SkewerStick.StickType.STRAW + 1));
                }
            }
            sticks.AddLast(stick);
        }

        for (int i = 0; i < 10; i++)
        {
            var meat = Instantiate(meatPrefab, playGround.transform).GetComponent<SkewerMeat>();
            meat.transform.localPosition = new Vector3(0 + i * margin, 4, 0);
            if (Random.value < 0.9)
            {

                meat.SetupMeat((currentMeat % 2 == 0) ? SkewerMeat.Type.LEAN : SkewerMeat.Type.FAT);
                currentMeat = (currentMeat + 1) % 3;
            }
            else
            {
                meat.SetupMeat((currentMeat % 2 == 0) ? SkewerMeat.Type.FAT : SkewerMeat.Type.LEAN);
            }
            meats.AddLast(meat);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (status == GameStatus.MAIN)
        {
            GetInput();
        }
    }

    void GetInput()
    {
        if (preventInput)
            return;
        switch (GameManager.Instance.CommonInputAction.GetPerformedTypeThisFrame())
        {
            case CommonInputAction.EType.Enter:
                PreventInput();
                AddMeat();
                break;
            case CommonInputAction.EType.Directions:
                Vector2 vec2 = GameManager.Instance.CommonInputAction.directions.ReadValue<Vector2>();
                if (vec2.y < 0)
                {
                    PreventInput();
                    RemoveStick(true);
                }
                else if (vec2.y > 0)
                {
                    PreventInput();
                    RemoveMeat(true);
                }
                break;
            case CommonInputAction.EType.Cancel:
                // TODO: double check
                finishSelection.GetComponent<CommonSelection>().ShowChoice();
                break;
        }
    }

    void PreventInput()
    {
        preventInput = true;
        timer.Add(() =>
        {
            preventInput = false;
        }, 0.6f);
    }

    public void StartGame(bool start)
    {
        if (start)
        {
            flowchart.ExecuteIfHasBlock("GameInit");
        }
        else
        {
            GameLogicManager.Instance.OnMiniGameRefused();
        }
    }

    public void ShowEntergameSelection()
    {
        workingSelection.GetComponent<CommonSelection>().ShowChoice();
    }

    public void LeaveGame(bool leave)
    {
        if (!leave)
        {
            SetGameStatus(GameStatus.FINISH);
        }
    }

    public void SetGameStatus(GameStatus status)
    {
        this.status = status;
        switch (status)
        {
            case GameStatus.INIT:
                break;
            case GameStatus.OPENING:
                break;
            case GameStatus.MAIN:
                canvas.SetActive(false);
                InitializeMainGame();
                break;
            case GameStatus.FINISH:
                flowchart.SetStringVariable("Money", string.Format("{0}.{1}\u5143", totalNumber * 5 / 10, totalNumber * 5 % 10));
                flowchart.ExecuteIfHasBlock("GameEnd");
                break;
            case GameStatus.END:
                float baseMoney = (float)totalNumber * 5 / 10;
                float baseComment = 40;

                if(totalNumber > 40)
                {
                    baseMoney *= 10;
                }

                float timeRatio = 0.8f + (totalNumber / 10) * 0.05f;

                GameLogicManager.Instance.OnMiniGameFinished(
                    GameLogicManager.Instance.gameData.money + baseMoney,
                    GameLogicManager.Instance.gameData.positiveComment + baseComment,
                    GameLogicManager.Instance.gameData.countDown - GameLogicManager.c_StandardGameDuration * timeRatio);

                break;
            default:
                Debug.Log("Wrong status?" + status.ToString());
                break;
        }
    }

    public GameStatus GetGameStatus()
    {
        return status;
    }


    void AddMeat()
    {
        var stick = sticks.First.Value;
        if (stick.CurrentMeat == 3)
        {
            FinishStick();
            totalNumber++;
            NumberText.text = string.Format("{0:D10}", totalNumber * 5);
            return;
        }

        if (stick.Type != SkewerStick.StickType.NORMAL)
        {
            // wrong stick
            flowchart.SetStringVariable("IngameWord", ingameSubs[(int)IngameSubType.WRONG_STICK]);
            flowchart.ExecuteIfHasBlock("Ingame");
            return;
        }

        if ((stick.CurrentMeat % 2 == 0 && meats.First.Value.MeatType != SkewerMeat.Type.LEAN) ||
            (stick.CurrentMeat % 2 == 1 && meats.First.Value.MeatType != SkewerMeat.Type.FAT))
        {
            // wrong meat
            flowchart.SetStringVariable("IngameWord", ingameSubs[(int)IngameSubType.WRONG_MEAT]);
            flowchart.ExecuteIfHasBlock("Ingame");
            return;
        }

        stick.ShowMeat();
        RemoveMeat(false);
        MoveMeat();
    }

    void AddStick(SkewerStick stick)
    {
        stick.transform.localPosition = sticks.Last.Value.transform.position + Vector3.left * margin;
        if (Random.value < 0.8)
        {
            stick.SetupStick(SkewerStick.StickType.NORMAL);
        }
        else
        {
            stick.SetupStick((SkewerStick.StickType)Random.Range((int)SkewerStick.StickType.CHOPSTICK, (int)SkewerStick.StickType.STRAW + 1));
        }
        sticks.AddLast(stick);
    }

    void RemoveStick(bool withAni)
    {
        var stick = sticks.First.Value;

        if (stick.CurrentMeat != 0)
        {
            // show
            return;
        }

        sticks.RemoveFirst();
        if (withAni)
        {
            ShowStickRemoveAni(stick.Type);
        }
        AddStick(stick);

        MoveStick(false);
    }

    void FinishStick()
    {
        var stick = sticks.First.Value;
        sticks.RemoveFirst();

        finishedSticks.AddLast(stick);

        if (finishedSticks.Count > 7)
        {
            stick = finishedSticks.First.Value;
            finishedSticks.RemoveFirst();
            AddStick(stick);
        }

        MoveStick(true);
    }

    void RemoveMeat(bool withAni)
    {
        var meat = meats.First.Value;
        meats.RemoveFirst();

        if (withAni)
        {
            ShowMeatRemoveAni(meat.MeatType);
        }

        if (Random.value < 0.9)
        {

            meat.SetupMeat((currentMeat % 2 == 0) ? SkewerMeat.Type.LEAN : SkewerMeat.Type.FAT);
            currentMeat = (currentMeat + 1) % 3;
        }
        else
        {
            meat.SetupMeat((currentMeat % 2 == 0) ? SkewerMeat.Type.FAT : SkewerMeat.Type.LEAN);
        }

        meat.transform.localPosition = meats.Last.Value.transform.localPosition + Vector3.right * margin;

        meats.AddLast(meat);


        MoveMeat();
    }

    void ShowMeatRemoveAni(SkewerMeat.Type type)
    {
        singleMeat.gameObject.SetActive(true);
        singleMeat.transform.localPosition = new Vector3(0, 4, 0);
        singleMeat.SetupMeat(type);

        iTween.MoveTo(singleMeat.gameObject, iTween.Hash(
            "position", singleMeat.transform.localPosition + new Vector3(-3, 3, 0),
            "local", true,
            "time", 0.5f,
            "easetype", iTween.EaseType.linear,
            "oncomplete", "OnCompleteMeatAni",
            "oncompletetarget", gameObject));
    }

    void OnCompleteMeatAni()
    {
        singleMeat.gameObject.SetActive(false);
    }

    void ShowStickRemoveAni(SkewerStick.StickType type)
    {
        singleStick.gameObject.SetActive(true);
        singleStick.transform.localPosition = new Vector3(0, -1, 0);
        singleStick.SetupStick(type);

        iTween.MoveTo(singleStick.gameObject, iTween.Hash(
            "position", singleStick.transform.localPosition + new Vector3(3, -3, 0),
            "local", true,
            "time", 0.5f,
            "easetype", iTween.EaseType.linear,
            "oncomplete", "OnCompleteStickAni",
            "oncompletetarget", gameObject));
    }

    void OnCompleteStickAni()
    {
        singleStick.gameObject.SetActive(false);
    }

    public void MoveStick(bool moveFinish)
    {
        foreach (SkewerStick stick in sticks)
        {
            iTween.MoveTo(stick.gameObject, stick.transform.position + Vector3.right * margin, 0.5f);
        }

        if (moveFinish)
        {
            foreach (SkewerStick stick in finishedSticks)
            {
                iTween.MoveTo(stick.gameObject, stick.transform.position + Vector3.right * margin, 0.5f);
            }
        }
    }

    public void MoveMeat()
    {
        foreach (SkewerMeat meat in meats)
        {
            iTween.MoveTo(meat.gameObject, meat.transform.position + Vector3.left * margin, 0.5f);
        }
    }
}
