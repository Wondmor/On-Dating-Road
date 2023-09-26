using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RacingHealth : MonoBehaviour
{
    [SerializeField]
    TextMeshPro text;

    [SerializeField]
    int MAX_HEALTH = 3;

    int health;
    // Start is called before the first frame update
    void Start()
    {
        health = MAX_HEALTH;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = String.Concat(Enumerable.Repeat("<sprite=0>", health));
    }

    public void ResetHealth()
    {
        health = MAX_HEALTH;
    }

    public void AddHealth(int amount)
    {
        health += amount;
        health = Math.Clamp(health, 0, MAX_HEALTH);
    }

    public int GetHealth()
    {
        return health;
    }

    public bool IsDead()
    {
        return health <= 0;
    }
}
