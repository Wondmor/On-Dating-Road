using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

    private void Start()
    {
        gameObject.SetActive(false);
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
        leftSmallPrice.text = $"гд{cargoInfo.expensivePrice:0.00}";
        leftBigPrice.text = $"гд{cargoInfo.expensivePrice:0.00}";
        rightDesc.text = cargoInfo.cheapDesc;
        rightSmallPrice.text = $"гд{cargoInfo.cheapPrice:0.00}";
        rightBigPrice.text = $"гд{cargoInfo.cheapPrice:0.00}";
    }
}
