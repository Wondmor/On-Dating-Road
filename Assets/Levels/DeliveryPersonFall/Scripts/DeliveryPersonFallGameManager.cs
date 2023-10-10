using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System;

public class DeliveryPersonFallGameManager : MonoBehaviour
{
    private const int INITIAL_LIFE_VALUE = 6;
    public List<string> levelNameList;

    public DeliveryPersonFallHand hand;
    public DeliveryPersonFallCargoLauncher cargoLauncher;
    public DeliveryPersonFallGround ground;
    public DeliveryPersonFallTutorialMenu tutorialMenu;
    public DeliveryPersonFallResultMenu resultMenu;
    public DeliveryPersonFallGameResultMenu gameResultMenu;
    public CommonSelection commonSelection;
    public Camera mainCamera;
    public AnimEventHandler animEventHandler;
    public Animator animator;
    public DeliveryPersonFallAnimMgr animMgr;
    public DeliveryPersonFallHUD hud;
    public GameObject bgGO;
    public AudioSource audioSource;

    bool catchCargo = false;
    bool catchExpensiveCargo = false;
    ReactiveProperty<int> cargoFallCountProp = new(0);
    int curLevelIndex = 0;
    int cargoCatchCount = 0;
    bool yellow;
    ReactiveProperty<int> heartCountProp = new(INITIAL_LIFE_VALUE);

    private void Awake()
    {
        cargoCatchCount = 0;
        hand.cam = mainCamera;
        hand.CatchCargo.Subscribe(cargo =>
        {
            string otherLayerName = LayerMask.LayerToName(cargo.layer);
            catchCargo = true;
            catchExpensiveCargo = otherLayerName.Contains("Expensive");
            cargoFallCountProp.Value += 1;
        }).AddTo(this);
        ground.CatchCargo.Subscribe(cargoCount =>
        {
            cargoFallCountProp.Value += cargoCount;
        }).AddTo(this);
        cargoFallCountProp.Subscribe(async fallCount =>
        {
            if (fallCount < 2)
                return;
            hand.SetMovable(false);
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            if (catchExpensiveCargo)
            {
                cargoCatchCount++;
            }  
            else
            {
                heartCountProp.Value--;
            }
            resultMenu.Show(catchCargo, catchExpensiveCargo, levelNameList[curLevelIndex], yellow, heartCountProp.Value);
            audioSource.Stop();
            hud.gameObject.SetActive(false);
        }).AddTo(this);
        resultMenu.OnHideFinished.Subscribe(async _ =>
        {
            Reset();
            if (heartCountProp.Value == 0 || curLevelIndex == levelNameList.Count - 1)
            {
                gameResultMenu.Show(GetMoney());
            }
            else
            {
                curLevelIndex++;
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
                StartLevel().Forget();
            } 
        }).AddTo(this);
        tutorialMenu.OnHide.Subscribe(_ =>
        {
            StartLevel().Forget();
        }).AddTo(this);
        animEventHandler.EventTriggeredStream.Subscribe(eventName =>
        {
            if(eventName == "Launch")
            {
                Launch();
            }
        }).AddTo(this);
        hud.SetHeartCountObservable(heartCountProp);
        gameResultMenu.OnHideFinished.Subscribe(_ =>
        {
            GameFinished();
        }).AddTo(this);
    }

    private void Start()
    {
        tutorialMenu.gameObject.SetActive(false);
        resultMenu.gameObject.SetActive(false);
        gameResultMenu.gameObject.SetActive(false);
        commonSelection.gameObject.SetActive(false);
        bgGO.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        commonSelection.gameObject.SetActive(true);
        commonSelection.ShowChoice();
    }

    public void SelectionResult(bool help)
    {
        bgGO.gameObject.SetActive(false);
        commonSelection.gameObject.SetActive(false);
        if (help)
        {
            heartCountProp.Value = INITIAL_LIFE_VALUE;
            tutorialMenu.ShowMenu();
        }
        else
        {
            GameLogicManager gameLogicMgrInstance = GameLogicManager.Instance;
            gameLogicMgrInstance.OnMiniGameRefused();
        }
    }

    async UniTaskVoid StartLevel()
    {
        hand.SetMovable(true);
        yellow = UnityEngine.Random.Range(0, 2) == 0;
        animMgr.SetYellow(yellow);
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        animator.SetTrigger("Play");
        audioSource.Play();
        hud.gameObject.SetActive(true);
    }

    public void Launch()
    {
        cargoLauncher.LaunchCargo(levelNameList[curLevelIndex]);
    }

    public void Reset()
    {
        catchCargo = false;
        catchExpensiveCargo = false;
        cargoFallCountProp.Value = 0;
        hand.Reset();
        cargoLauncher.Reset();
        ground.Reset();
        animator.SetTrigger("Reset");
        animator.SetTrigger("CatReset");
        animator.SetTrigger("GirlReset");
    }

    void GameFinished()
    {
        GameLogicManager glmIns = GameLogicManager.Instance;
        Debug.Log($"result: Money {GetMoney()}, Positive {GetPositiveComment()}, CountDown {GetCountDown()}");
        glmIns.OnMiniGameFinished(glmIns.gameData.money + GetMoney(), glmIns.gameData.positiveComment + GetPositiveComment(), glmIns.gameData.countDown - GameLogicManager.c_StandardGameDuration * GetCountDown());
        
    }

    float GetMoney()
    {
        return Mathf.RoundToInt(Mathf.Lerp(0, 30, GetFactor()));
    }

    float GetPositiveComment()
    {
        return Mathf.RoundToInt(Mathf.Lerp(0, 30, GetFactor()));
    }

    float GetCountDown()
    {
        return Mathf.Lerp(1.2f, 0.7f, GetFactor());
    }

    float GetFactor()
    {
        return resultList[cargoCatchCount];
    }
    
    List<float> resultList = new()
    {
        0, 0.2f, 0.4f, 0.5f, 0.55f, 0.6f, 0.65f, 0.7f, 0.75f, 0.8f, 0.85f, 0.9f, 1f
    };
  
}
