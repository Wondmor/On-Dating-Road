using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System;

public class DeliveryPersonFallGameManager : MonoBehaviour
{
    public List<string> levelNameList;

    public DeliveryPersonFallHand hand;
    public DeliveryPersonFallCargoLauncher cargoLauncher;
    public DeliveryPersonFallGround ground;
    public DeliveryPersonFallTutorialMenu tutorialMenu;
    public DeliveryPersonFallResultMenu resultMenu;
    public Camera mainCamera;
    public AnimEventHandler animEventHandler;
    public Animator animator;

    bool catchCargo = false;
    bool catchExpensiveCargo = false;
    ReactiveProperty<int> cargoFallCountProp = new(0);
    int curLevelIndex = 0;

    private void Awake()
    {
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
            resultMenu.Show(catchCargo, catchExpensiveCargo, levelNameList[curLevelIndex]);
        }).AddTo(this);
        resultMenu.OnHideFinished.Subscribe(async _ =>
        {
            Reset();
            if (curLevelIndex == levelNameList.Count - 1)
                GameFinished();
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
    }

    private void Start()
    {
        tutorialMenu.gameObject.SetActive(false);
        resultMenu.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        tutorialMenu.ShowMenu();
    }

    async UniTaskVoid StartLevel()
    {
        hand.SetMovable(true);
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        animator.SetTrigger("Play");
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

    }
}
