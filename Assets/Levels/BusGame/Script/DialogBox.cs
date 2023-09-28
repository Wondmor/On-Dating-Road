using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DialogBox : MonoBehaviour
{
    public float duration;
    private float startTime;
    public Animator animator;
    void Awake()
    {
        startTime = Time.time;
    }
    void Update()
    {
        float elapsedTime = Time.time - startTime;
        if (elapsedTime >= duration)
        {
            Destroy(gameObject);
        }
    }


}
