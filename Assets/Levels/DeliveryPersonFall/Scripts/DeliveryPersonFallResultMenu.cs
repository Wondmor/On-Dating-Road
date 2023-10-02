using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DeliveryPersonFallResultMenu : MonoBehaviour
{
    [Serializable]
    public class CargoInfo
    {
        public string levelName;
        public float cheapPrice;
        public float expensivePrice;
        [Multiline]
        public string cheapDesc;
        [Multiline]
        public string expensiveDesc;
    }

    public List<CargoInfo> cargoInfoList;
    public InputAction inputAction;
    public Text leftDesc;
    public Text leftSmallPrice;
    public Text leftBigPrice;
    public Text rightDesc;
    public Text rightSmallPrice;
    public Text rightBigPrice;

    public GameObject goodImageGO;
    public GameObject badImageGO;

    public GameObject expensiveSuccessGO;
    public GameObject expensiveFailGO;
    public GameObject cheapFailGO;

    public IObservable<Unit> OnHideFinished;

    Subject<Unit> onHideFinished = new();

    private void Awake()
    {
        inputAction.performed += ctx => 
        {
            gameObject.SetActive(false);
            onHideFinished.OnNext(Unit.Default); 
        };
    }
    public void OnEnable()
    {
        inputAction.Enable();
    }

    public void OnDisable()
    {
        inputAction.Disable();
    }

    public void Show(bool catchCargo, bool catchExpensive, string levelName)
    {
        gameObject.SetActive(true);
        var cargoInfo = cargoInfoList.FirstOrDefault(info => info.levelName == levelName);
        if (cargoInfo == null)
        {
            Debug.LogError($"No cargo info for level {levelName}");
            return;
        }
        goodImageGO.SetActive(catchExpensive);
        badImageGO.SetActive(!catchExpensive);
        expensiveSuccessGO.SetActive(catchExpensive);
        expensiveFailGO.SetActive(!catchExpensive && !catchCargo);
        cheapFailGO.SetActive(!catchExpensive && catchCargo);
        leftDesc.text = cargoInfo.expensiveDesc;
        leftSmallPrice.text = $"��{cargoInfo.expensivePrice:0.00}";
        leftBigPrice.text = $"��{cargoInfo.expensivePrice:0.00}";
        rightDesc.text = cargoInfo.cheapDesc;
        rightSmallPrice.text = $"��{cargoInfo.cheapPrice:0.00}";
        rightBigPrice.text = $"��{cargoInfo.cheapPrice:0.00}";
    }
}
