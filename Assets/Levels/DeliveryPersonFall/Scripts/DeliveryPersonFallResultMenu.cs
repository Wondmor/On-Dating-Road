using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;

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
        public GameObject cheapGO;
        public GameObject expensiveGO;
    }

    enum State
    {
        AnimatingShowBox,
        WaitingShowBox,
        AnimatingShowResult,
        WaitingResult,
        AnimatingHeartResult,
    }

    public List<CargoInfo> cargoInfoList;
    public InputActionAsset inputActionAsset;
    public TextMeshProUGUI leftDesc;
    public TextMeshProUGUI leftSmallPrice;
    public TextMeshProUGUI leftBigPrice;
    public TextMeshProUGUI rightDesc;
    public TextMeshProUGUI rightSmallPrice;
    public TextMeshProUGUI rightBigPrice;

    public GameObject goodImageGO;
    public GameObject badImageGO;

    public GameObject expensiveSuccessGO;
    public GameObject expensiveFailGO;
    public GameObject cheapFailGO;

    public Transform leftTicketTrans;
    public Transform rightTicketTrans;
    public Transform leftTicketStartPos;
    public Transform leftTicketEndPos;
    public Transform rightTicketStartPos;
    public Transform rightTicketEndPos;

    public GameObject leftItemGOGroup;
    public GameObject rightItemGOGroup;
    public List<DeliveryPersonFallYellowBlue> deliveryPersonFallYellowBlues;
    public CanvasGroup heartLoseCanvasGroup;
    public List<GameObject> heartList;

    public AudioSource goodEndAS;
    public AudioSource badEndAS;
    public AudioSource goodEndVoiceAS;
    public AudioSource badEndVoiceAS;
    public AudioSource paperAS;

    public IObservable<Unit> OnHideFinished => onHideFinished;

    private InputAction continueAction;
    Subject<Unit> onHideFinished = new();
    State state = State.AnimatingShowBox;
    bool catchExpensive;
    bool catchCargo;
    int heartCount;
    private AudioSource resultAudioVoiceSource;
    private AudioSource resultAudioSource;

    private void Awake()
    {
        continueAction = inputActionAsset.FindAction("Continue");
    }

    private void OnContinuePressed(InputAction.CallbackContext _)
    {
        switch (state)
        {
            case State.WaitingShowBox:
                Sequence sequence = DOTween.Sequence();
                leftItemGOGroup.SetActive(false);
                rightItemGOGroup.SetActive(false);
                state = State.AnimatingShowResult;
                var centerResultTrans = catchExpensive ? goodImageGO.transform : badImageGO.transform;
                sequence.Append(centerResultTrans.transform.DOScale(Vector3.one, 1.5f));
                sequence.Join(centerResultTrans.transform.DOLocalRotate(new Vector3(0f, 0f, 3600f), 1.5f, RotateMode.FastBeyond360));
                sequence.AppendCallback(() =>
                {
                    resultAudioVoiceSource = catchExpensive ? goodEndVoiceAS : badEndVoiceAS;
                    resultAudioVoiceSource.Play();
                    state = State.WaitingResult;
                });
                break;
            case State.WaitingResult:
                if (catchExpensive)
                {
                    HideAndContinue();
                }
                else
                {
                    state = State.AnimatingHeartResult;
                    int origHeartCount = heartCount + 1;
                    for (int i = 0; i < heartList.Count; ++i)
                    {
                        heartList[i].SetActive(i < origHeartCount);
                    }
                    Sequence heartSequence = DOTween.Sequence();
                    heartSequence.Append(heartLoseCanvasGroup.DOFade(1f, 0.5f))
                    .Append(heartList[heartCount].transform.DOShakeRotation(1f, new Vector3(0f, 0f, 90f)))
                    .Insert(0.5f, heartList[heartCount].GetComponent<Image>().DOFade(0f, 0.5f))
                    .AppendInterval(1f)
                    .Append(heartLoseCanvasGroup.DOFade(0f, 0.5f))
                    .AppendCallback(() =>
                    {
                        HideAndContinue();
                    });
                }
                break;
            default:
                return;
        }
    }

    private void HideAndContinue()
    {
        gameObject.SetActive(false);
        onHideFinished.OnNext(Unit.Default);
    }

    public void OnEnable()
    {
        continueAction.Enable();
        continueAction.performed += OnContinuePressed;
    }

    public void OnDisable()
    {
        continueAction.Disable();
        resultAudioVoiceSource?.Stop();
        resultAudioSource?.Stop();
        continueAction.performed -= OnContinuePressed;
    }

    public void Show(bool _catchCargo, bool _catchExpensive, string levelName, bool yellow, int _heartCount)
    {
        foreach(var yellowBlue in deliveryPersonFallYellowBlues)
        {
            yellowBlue.yellow.SetActive(yellow);
            yellowBlue.blue.SetActive(!yellow);
        } 
        state = State.AnimatingShowBox;
        gameObject.SetActive(true);
        cargoInfoList.ForEach(info =>
        {
            info.cheapGO.SetActive(false);
            info.expensiveGO.SetActive(false);
        });
        var cargoInfo = cargoInfoList.FirstOrDefault(info => info.levelName == levelName);
        if (cargoInfo == null)
        {
            Debug.LogError($"No cargo info for level {levelName}");
            return;
        }
        cargoInfo.cheapGO.SetActive(true);
        cargoInfo.expensiveGO.SetActive(true);
        catchExpensive = _catchExpensive;
        catchCargo = _catchCargo;
        goodImageGO.SetActive(catchExpensive);
        badImageGO.SetActive(!catchExpensive);
        leftItemGOGroup.SetActive(true);
        rightItemGOGroup.SetActive(true);
        expensiveSuccessGO.SetActive(false);
        expensiveFailGO.SetActive(false);
        cheapFailGO.SetActive(false);
        leftDesc.text = cargoInfo.expensiveDesc;
        leftSmallPrice.text = $"{cargoInfo.expensivePrice:0.00}";
        leftBigPrice.text = cargoInfo.expensivePrice == 0 ? "ÔùÆ·" : $"{cargoInfo.expensivePrice:0.00}";
        rightDesc.text = cargoInfo.cheapDesc;
        rightSmallPrice.text = $"{cargoInfo.cheapPrice:0.00}";
        rightBigPrice.text = cargoInfo.cheapPrice == 0 ? "ÔùÆ·": $"{cargoInfo.cheapPrice:0.00}";
        leftTicketTrans.localPosition = leftTicketStartPos.localPosition;
        rightTicketTrans.localPosition = rightTicketStartPos.localPosition;
        goodImageGO.transform.localScale = Vector3.zero;
        badImageGO.transform.localScale = Vector3.zero;
        goodImageGO.transform.eulerAngles = Vector3.zero;
        badImageGO.transform.eulerAngles = Vector3.zero;
        paperAS.Play();
        resultAudioSource = catchExpensive ? goodEndAS : badEndAS;
        resultAudioSource.Play();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(leftTicketTrans.DOLocalMove(leftTicketEndPos.localPosition, 0.5f).SetEase(Ease.Linear));
        sequence.Join(rightTicketTrans.DOLocalMove(rightTicketEndPos.localPosition, 0.5f).SetEase(Ease.Linear));
        sequence.AppendInterval(1f);
        sequence.AppendCallback(() =>
        {
            expensiveSuccessGO.SetActive(catchExpensive);
            expensiveFailGO.SetActive(!catchExpensive && !catchCargo);
            cheapFailGO.SetActive(!catchExpensive || !catchCargo);
            state = State.WaitingShowBox;
        });
        heartLoseCanvasGroup.alpha = 0f;
        heartCount = _heartCount;
        heartList.ForEach(heart => heart.GetComponent<Image>().color = Color.white);
    }
}
