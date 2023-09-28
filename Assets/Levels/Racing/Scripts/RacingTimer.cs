using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RacingTimer : MonoBehaviour
{
    [SerializeField]
    TextMeshPro text;

    float time = 0;
    bool counting = false;
    // Start is called before the first frame update
    void Start()
    {
        counting = true;
    }

    private void FixedUpdate()
    {
        if(counting)
        {
            time += Time.deltaTime;
        }

        TimeSpan t = TimeSpan.FromSeconds(time);

        string timeStr = string.Format("{0:D2} {1:D2}",
                        t.Minutes,
                        t.Seconds);

        string spriteTime = "";
        foreach(char c in timeStr)
        {
            if(c == ' ')
            {
                spriteTime += " ";
            }
            else
            {
                spriteTime += String.Format("<sprite={0}>", c);
            }
        }
        text.text = spriteTime;
    }

    public void ResetClock()
    {
        time = 0;
    }

    public int GetTime()
    {
        return (int)time;
    }

    public void PauseCount()
    {
        counting = false;
    }

    public void StartCount()
    {
        counting = true;
    }
}
