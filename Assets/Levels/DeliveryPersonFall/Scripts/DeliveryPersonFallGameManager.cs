using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.XR;

public class DeliveryPersonFallGameManager : MonoBehaviour
{
    public List<string> levelNameList;

    public DeliveryPersonFallHand hand;
    public DeliveryPersonFallCargoLauncher cargoLauncher;
    public DeliveryPersonFallGround ground;
    public DeliveryPersonFallTutorialMenu tutorialMenu;
    public DeliveryPersonFallResultMenu resultMenu;
    public Camera mainCamera;
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
            hand.SetMovable(false);
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            if (fallCount < 2)
                return;
            resultMenu.Show(catchCargo, catchExpensiveCargo, levelNameList[curLevelIndex]);
        }).AddTo(this);
        tutorialMenu.OnHide.Subscribe(_ =>
        {
            StartLevel();
        }).AddTo(this);
    }

    public void StartGame()
    {
        tutorialMenu.ShowMenu();
    }

    void StartLevel()
    {
        animator.SetTrigger("Start");
    }

    public void Launch()
    {
        //cargoLauncher.LaunchCargo
    }

    public void Reset()
    {
        catchCargo = false;
        catchExpensiveCargo = false;
        cargoFallCountProp.Value = 0;
        hand.Reset();
        cargoLauncher.Reset();
        ground.Reset();
    }
}
