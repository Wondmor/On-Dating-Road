using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public float acceleration = 4f; // ��Ҽ��ٶ�
    public float maxSpeed = 10f; // �������ٶ�
    public float jumpForce = 10f; // ��Ծ����
    public float deceleration = 1f; // ���ٶ�
    public Image speedSlider;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    public float currentSpeed = 0.0f; // ��ǰ�ٶ�
    private bool canJump = false;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // ����Ƿ��ڵ�����
        isGrounded = Physics2D.Raycast(transform.position-new Vector3(0,0.5f,0), Vector2.down, 0.1f);
        // ������Ϣ�����ӻ�����
        //Debug.DrawRay(transform.position - new Vector3(0, 0.5f, 0), Vector2.down * 0.1f, Color.red);
        //Debug.Log(isGrounded);

        float moveInput = Input.GetButton("Fire1") ? 1.0f : 0.0f;
        speedSlider.fillAmount = currentSpeed / maxSpeed;
        // �������������м���
        if (moveInput > 0)
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);//�����ٶ���ֵ
            rb.velocity = new Vector2(-currentSpeed, rb.velocity.y);
        }
        else
        {
            // �ɿ����������м���
            if (isGrounded)
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
                rb.velocity = new Vector2(-currentSpeed, rb.velocity.y);
            }
        }

        // ����Ƿ�����ɫ������
        if (canJump && Input.GetButtonDown("Fire2"))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            acceleration = 0f;//��Ծ�����ܽ��м���
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
