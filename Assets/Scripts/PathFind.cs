using System.Collections.Generic;
using UnityEngine;

public class PathFind : MonoBehaviour
{
    [Header("�ƶ�����")]
    [Tooltip("�ƶ��ٶ�(��λ/��)")]
    public float moveSpeed = 3f;
    [Tooltip("�������ж�����")]
    public float arrivalThreshold = 0.1f;

    private List<Vector3> currentPath;
    private int currentPointIndex;
    private bool isMoving;

    /// <summary>
    /// ��ʼ��·���ƶ�����
    /// </summary>
    /// <param name="path">�ƶ�·�������б�</param>
    public void MoveAlongPath(List<Vector3> path)
    {
        // ȷ��·����Ч
        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("�ƶ�·��Ϊ��!");
            return;
        }

        // ���õ�ǰ·��
        currentPath = path;
        currentPointIndex = 0;

        // ֱ�����õ����
        if (currentPath.Count > 0)
        {
            transform.position = currentPath[0];
        }

        isMoving = true;
    }

    private void Update()
    {
        // ���û���ƶ������·����Ч��ֱ�ӷ���
        if (!isMoving || currentPath == null || currentPointIndex >= currentPath.Count)
            return;

        // ��ȡ��ǰĿ���
        Vector3 targetPoint = currentPath[currentPointIndex];

        // �����ƶ������ƶ�
        Vector3 moveStep = moveSpeed * Time.deltaTime * (targetPoint - transform.position).normalized;
        transform.position += moveStep;

        // ����Ƿ񵽴ﵱǰĿ���
        if (Vector3.Distance(transform.position, targetPoint) < arrivalThreshold)
        {
            currentPointIndex++;

            // ����Ƿ񵽴��յ�
            if (currentPointIndex >= currentPath.Count)
            {
                FinishMovement();
            }
        }
    }

    /// <summary>
    /// ����ƶ�
    /// </summary>
    private void FinishMovement()
    {
        isMoving = false;
        Debug.Log("�ƶ����!");
    }

    /// <summary>
    /// ֹͣ��ǰ�ƶ�
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