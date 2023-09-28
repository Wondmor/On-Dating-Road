using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // ��Ҷ��������
    public float smoothSpeed = 0.125f; // ������ƶ���ƽ���� ƽ����Խ�;�ͷ�ƶ�Խ��
    public Vector3 offset; // ����������֮���ƫ��

    private void LateUpdate()
    {
        if (target != null)
        {
            // ��ȡ��ҵĵ�ǰλ��
            Vector3 playerPosition = target.position;

            // �����������Zֵ
            playerPosition.z = transform.position.z;

            // �����������Ŀ��λ��
            Vector3 desiredPosition = playerPosition + offset;

            // ʹ��Lerp������ƽ���ƶ������
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // �����������λ��
            transform.position = smoothedPosition;

        }
    }
}
