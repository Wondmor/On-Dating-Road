using System.Collections;
using System.Collections.Generic;
using System.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhoneTimer : MonoBehaviour
{
    [Header("总秒数")]
    public float totalTime = 3600.0f;
    public Sprite greenCircle = null;
    public Sprite yellowCircle = null;
    public Sprite redCircle = null;
    TextMeshProUGUI textUI;
    Image circle;
    public bool bStart = false;
    // Start is called before the first frame update
    void Start()
    {
        textUI = transform.Find("Timer").GetComponent<TextMeshProUGUI>();
        circle = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bStart)
        {
            totalTime -= Time.deltaTime;
            totalTime = Mathf.Max(0, totalTime);

            var sprite = totalTime > 4800 ? greenCircle : (totalTime > 1200 ? yellowCircle : redCircle);
            circle.sprite = sprite;

            textUI.text = string.Format("{0:D2}:{1:D2}:{2:D2}", ((int)totalTime / 60 / 60 % 60), ((int)totalTime / 60 % 60), ((int)totalTime % 60));
        }
    }
}
