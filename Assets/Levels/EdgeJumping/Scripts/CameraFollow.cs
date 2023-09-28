using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 玩家对象的引用
    public float smoothSpeed = 0.125f; // 摄像机移动的平滑度 平滑度越低镜头移动越慢
    public Vector3 offset; // 摄像机与玩家之间的偏移

    private void LateUpdate()
    {
        if (target != null)
        {
            // 获取玩家的当前位置
            Vector3 playerPosition = target.position;

            // 锁定摄像机的Z值
            playerPosition.z = transform.position.z;

            // 计算摄像机的目标位置
            Vector3 desiredPosition = playerPosition + offset;

            // 使用Lerp函数来平滑移动摄像机
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // 更新摄像机的位置
            transform.position = smoothedPosition;

        }
    }
}
