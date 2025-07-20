using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
     [Header("Zoom Settings")]
    public float minZoom = 2f;
    public float maxZoom = 12f;
    public float zoomSpeed = 5f;

    [Header("Drag Settings")]
    public float dragSpeed = 2f;
    public bool invertDrag = false;

    [Header("Movement Boundaries")]
    public Vector2 minBoundary = new Vector2(-10, -10);
    public Vector2 maxBoundary = new Vector2(10, 10);
    public bool showBoundaryGizmo = true;

    private Camera cam;
    private Vector3 dragOrigin;
    private float initialZ; // 存储初始Z位置
    private bool isDragging;

    void Start()
    {
        cam = GetComponent<Camera>();
        
        if (cam == null)
        {
            Debug.LogError("CameraController script requires a Camera component!");
            enabled = false;
        }
        
        // 记录初始Z轴位置
        initialZ = transform.position.z;
        
        // 初始位置验证
       // ClampCameraPosition();
    }

    void Update()
    {
        HandleZoomInput();
        //HandleDragInput();
    }

    void LateUpdate()
    {
       // // 确保摄像机在边界内并保持Z轴稳定
       //// ClampCameraPosition();
        
       // // 始终固定Z轴位置
       // Vector3 pos = transform.position;
       // pos.z = initialZ;
       // transform.position = pos;
    }

    private void HandleZoomInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        if (scroll != 0)
        {
            // 2D场景通常使用正交摄像机
            if (cam.orthographic)
            {
                cam.orthographicSize = Mathf.Clamp(
                    cam.orthographicSize - scroll * zoomSpeed,
                    minZoom,
                    maxZoom
                );
            }
            else
            {
                // 如果是透视摄像机，调整fieldOfView
                cam.fieldOfView = Mathf.Clamp(
                    cam.fieldOfView - scroll * zoomSpeed,
                    minZoom,
                    maxZoom
                );
            }
            
            // 缩放后重新计算边界
           // ClampCameraPosition();
        }
    }

    private void HandleDragInput()
    {
        // 鼠标左键按下开始拖拽
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = GetMouseWorldPosition();
            isDragging = true;
        }

        // 鼠标左键抬起停止拖拽
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 currentMousePos = GetMouseWorldPosition();
            Vector3 difference = dragOrigin - currentMousePos;
            
            // 忽略Z轴差异
            difference.z = 0;
            
            // 应用拖拽移动
            if (invertDrag)
            {
                transform.position += difference * dragSpeed;
            }
            else
            {
                transform.position -= difference * dragSpeed;
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        // 获取鼠标位置（忽略Z轴）
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -initialZ; // 使用负的初始Z值确保正确转换
        
        return cam.ScreenToWorldPoint(mousePosition);
    }

    private void ClampCameraPosition()
    {
        Vector3 pos = transform.position;
        
        // 计算当前视野下的有效边界
        float effectiveZoom = cam.orthographic ? cam.orthographicSize : cam.fieldOfView / 2f;
        float aspectRatio = cam.aspect;
        
        // 计算实际边界（考虑当前视野大小）
        float horizontalExtent = effectiveZoom * aspectRatio;
        float verticalExtent = effectiveZoom;
        
        Vector2 adjustedMinBoundary = new Vector2(
            minBoundary.x + horizontalExtent,
            minBoundary.y + verticalExtent
        );
        
        Vector2 adjustedMaxBoundary = new Vector2(
            maxBoundary.x - horizontalExtent,
            maxBoundary.y - verticalExtent
        );
        
        // 确保边界有效（最小值不大于最大值）
        adjustedMinBoundary.x = Mathf.Min(adjustedMinBoundary.x, adjustedMaxBoundary.x);
        adjustedMinBoundary.y = Mathf.Min(adjustedMinBoundary.y, adjustedMaxBoundary.y);
        
        // 限制X轴位置
        pos.x = Mathf.Clamp(pos.x, adjustedMinBoundary.x, adjustedMaxBoundary.x);
        
        // 限制Y轴位置
        pos.y = Mathf.Clamp(pos.y, adjustedMinBoundary.y, adjustedMaxBoundary.y);
        
        // 保持Z轴不变
        pos.z = initialZ;
        
        transform.position = pos;
    }

    // 在场景视图中绘制边界
    void OnDrawGizmos()
    {
        if (!showBoundaryGizmo || !cam) return;
        
        Gizmos.color = Color.green;
        
        // 计算边界框
        Vector3 center = new Vector3(
            (minBoundary.x + maxBoundary.x) / 2,
            (minBoundary.y + maxBoundary.y) / 2,
            0
        );
        
        Vector3 size = new Vector3(
            maxBoundary.x - minBoundary.x,
            maxBoundary.y - minBoundary.y,
            0.1f
        );
        
        // 绘制边界框
        Gizmos.DrawWireCube(center, size);
        
        // 绘制边界角标记
        float markerSize = 0.5f;
        Gizmos.DrawSphere(new Vector3(minBoundary.x, minBoundary.y, 0), markerSize);
        Gizmos.DrawSphere(new Vector3(minBoundary.x, maxBoundary.y, 0), markerSize);
        Gizmos.DrawSphere(new Vector3(maxBoundary.x, minBoundary.y, 0), markerSize);
        Gizmos.DrawSphere(new Vector3(maxBoundary.x, maxBoundary.y, 0), markerSize);
        
        // 绘制当前有效边界（考虑视野）
        if (Application.isPlaying)
        {
            Gizmos.color = new Color(1, 0.5f, 0, 0.7f); // 橙色半透明
            
            // 计算当前有效边界
            float effectiveZoom = cam.orthographic ? cam.orthographicSize : cam.fieldOfView / 2f;
            float aspectRatio = cam.aspect;
            float horizontalExtent = effectiveZoom * aspectRatio;
            float verticalExtent = effectiveZoom;
            
            Vector3 effectiveMin = new Vector3(
                minBoundary.x + horizontalExtent,
                minBoundary.y + verticalExtent,
                0
            );
            
            Vector3 effectiveMax = new Vector3(
                maxBoundary.x - horizontalExtent,
                maxBoundary.y - verticalExtent,
                0
            );
            
            // 绘制有效边界框
            Vector3 effectiveCenter = (effectiveMin + effectiveMax) / 2;
            Vector3 effectiveSize = effectiveMax - effectiveMin;
            effectiveSize.z = 0.1f;
            
            Gizmos.DrawWireCube(effectiveCenter, effectiveSize);
        }
    }
}
