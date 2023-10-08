using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingNPC : MonoBehaviour
{
    Animator animator;
    float nextEye = 5f;
    float accumulateTime = 0;

    [SerializeField]
    Writer writer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        accumulateTime += Time.deltaTime;
        if(accumulateTime > nextEye)
        {
            animator.SetTrigger("Eye");
            accumulateTime = 0;
            nextEye = Random.Range(3, 8);
        }

        if(writer.IsWriting && !writer.IsWaitingForInput)
        {
            animator.SetTrigger("Mouth");
        }
    }
}
