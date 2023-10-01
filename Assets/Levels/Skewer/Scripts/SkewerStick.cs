using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SkewerStick : MonoBehaviour
{
    public enum StickType
    {
        NORMAL,
        CHOPSTICK,
        PENCIL,
        INCENSE,
        STRAW
    }

    [SerializeField]
    Sprite[] sprites;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    GameObject[] meats;

    StickType type = StickType.NORMAL;

    bool isMoving = false;
    int currentMeat = 0;

    public StickType Type { get => type; set => type = value; }
    public int CurrentMeat { get => currentMeat; private set => currentMeat = value; }
    public void Awake()
    {
        SetupStick(StickType.NORMAL);
    }

    public void SetupStick(StickType type)
    {
        this.type = type;

        spriteRenderer.sprite = sprites[(int)type];

        for (int i = 0; i < meats.Length; i++)
        {
            meats[i].transform.localPosition = new Vector3(0, 5, 0);
            meats[i].SetActive(false);
        }

        isMoving = false;
        currentMeat = 0;
    }

    public void ShowMeat()
    {
        if (isMoving || currentMeat >= meats.Length)
            return;

        isMoving = true;

        meats[currentMeat].SetActive(true);
        iTween.MoveTo(meats[currentMeat], iTween.Hash(
            "position", new Vector3(Random.Range(-0.02f, 0.02f), -1 + currentMeat ,0),
            "time", 0.5f,
            "easetype", iTween.EaseType.linear,
            "oncomplete", "OnComplete",
            "oncompletetarget", gameObject));

        currentMeat++;
    }

    void OnComplete()
    {
        isMoving = false;
    }

}
