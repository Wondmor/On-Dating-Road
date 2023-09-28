using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public float acceleration = 4f; // 玩家加速度
    public float maxSpeed = 10f; // 玩家最大速度
    public float jumpForce = 10f; // 跳跃力量
    public float deceleration = 1f; // 减速度
    public Image speedSlider;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    public float currentSpeed = 0.0f; // 当前速度
    private bool canJump = false;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 检测是否在地面上
        isGrounded = Physics2D.Raycast(transform.position-new Vector3(0,0.5f,0), Vector2.down, 0.1f);
        // 调试信息：可视化射线
        //Debug.DrawRay(transform.position - new Vector3(0, 0.5f, 0), Vector2.down * 0.1f, Color.red);
        //Debug.Log(isGrounded);

        float moveInput = Input.GetButton("Fire1") ? 1.0f : 0.0f;
        speedSlider.fillAmount = currentSpeed / maxSpeed;
        // 长按鼠标左键进行加速
        if (moveInput > 0)
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);//设置速度最值
            rb.velocity = new Vector2(-currentSpeed, rb.velocity.y);
        }
        else
        {
            // 松开鼠标左键进行减速
            if (isGrounded)
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
                rb.velocity = new Vector2(-currentSpeed, rb.velocity.y);
            }
        }

        // 检测是否在蓝色区域内
        if (canJump && Input.GetButtonDown("Fire2"))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            acceleration = 0f;//跳跃后不再能进行加速
            deceleration = 10f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("JumpPlatform"))
        {
            canJump = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("JumpPlatform"))
        {
            canJump = false;
        }
    }
}
