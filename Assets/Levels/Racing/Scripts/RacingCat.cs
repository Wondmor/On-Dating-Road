using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Rendering;
using UnityEngine;

public class RacingCat : MonoBehaviour
{
    [SerializeField, Range(3, 8)]
    float MIN_DISTANCE, MAX_DISTANCE;

    [SerializeField, Range(0.1f, 8f)]
    float speed;

    float startDistance = 0;
    bool triggered = false;
    bool hit = false;
    bool flipped = false;
    BoxCollider2D boxCollider;
    Animator animator;

    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!triggered)
        {
            if (startPosition.y - transform.localPosition.y > startDistance)
            {
                triggered = true;
                if(flipped)
                {
                    transform.localPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = Vector3.one;
                }
            }
        }

        if(triggered && !hit)
        {
            transform.localPosition = transform.localPosition + Vector3.left * Time.deltaTime * speed * (flipped ? -1f : 1f);
        }
    }

    public void Hit()
    {
        animator.SetTrigger("hit");
        boxCollider.enabled = false;
        hit = true;
    }

    public void SetupCat()
    {
        startPosition = transform.localPosition;
        startDistance = Random.Range(MIN_DISTANCE, MAX_DISTANCE);
        triggered = false;
        hit = false;

        //flipped = Random.value < 0.5f ? true : false;
        flipped = true;
        GetComponent<BoxCollider2D>().enabled = true;

    }
}
