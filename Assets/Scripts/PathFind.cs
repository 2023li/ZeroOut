using System.Collections.Generic;
using UnityEngine;

public class PathFind : MonoBehaviour
{
    [Header("移动设置")]
    [Tooltip("移动速度(单位/秒)")]
    public float moveSpeed = 3f;
    [Tooltip("到达点的判定距离")]
    public float arrivalThreshold = 0.1f;

    private List<Vector3> currentPath;
    private int currentPointIndex;
    private bool isMoving;

    /// <summary>
    /// 开始沿路径移动物体
    /// </summary>
    /// <param name="path">移动路径坐标列表</param>
    public void MoveAlongPath(List<Vector3> path)
    {
        // 确保路径有效
        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("移动路径为空!");
            return;
        }

        // 设置当前路径
        currentPath = path;
        currentPointIndex = 0;

        // 直接设置到起点
        if (currentPath.Count > 0)
        {
            transform.position = currentPath[0];
        }

        isMoving = true;
    }

    private void Update()
    {
        // 如果没有移动任务或路径无效，直接返回
        if (!isMoving || currentPath == null || currentPointIndex >= currentPath.Count)
            return;

        // 获取当前目标点
        Vector3 targetPoint = currentPath[currentPointIndex];

        // 计算移动方向并移动
        Vector3 moveStep = moveSpeed * Time.deltaTime * (targetPoint - transform.position).normalized;
        transform.position += moveStep;

        // 检查是否到达当前目标点
        if (Vector3.Distance(transform.position, targetPoint) < arrivalThreshold)
        {
            currentPointIndex++;

            // 检查是否到达终点
            if (currentPointIndex >= currentPath.Count)
            {
                FinishMovement();
            }
        }
    }

    /// <summary>
    /// 完成移动
    /// </summary>
    private void FinishMovement()
    {
        isMoving = false;
        Debug.Log("移动完成!");
    }

    /// <summary>
    /// 停止当前移动
    /// </summary>
    public void StopMovement()
    {
        isMoving = false;
        currentPath = null;
    }

    private void OnDisable()
    {
        StopMovement();
    }
}