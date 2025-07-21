using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MouseKey
{
    ������,
    ����Ҽ�
}
public interface IOnPlayerClick
{

    public void OnPlayerClick(MouseKey key);


}

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
    private float initialZ; // �洢��ʼZλ��
    private bool isDragging;

    void Start()
    {
        cam = GetComponent<Camera>();
        
        if (cam == null)
        {
            Debug.LogError("CameraController script requires a Camera component!");
            enabled = false;
        }
        
        // ��¼��ʼZ��λ��
        initialZ = transform.position.z;
        
        // ��ʼλ����֤
       // ClampCameraPosition();
    }

    void Update()
    {
        HandleZoomInput();
        HandleDragInput();

        HandleClickInput();




    }



    #region ���߼��

    [Header("Click Settings")]
    [Tooltip("ֻ�����Щ���ϵ�����")]
    public LayerMask clickableLayers = ~0; // Ĭ�ϼ�����в�

    

    [Tooltip("����ģʽ����ʾ���ߺ����е�")]
    public bool debugMode = false;

    private void HandleClickInput()
    {
        
      

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("������߼��");
            HandleRaycast(MouseKey.������);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("�Ҽ������߼��");
            HandleRaycast(MouseKey.����Ҽ�);
        }
    }

    private void HandleRaycast(MouseKey key)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        RaycastHit2D hit2D;

        // 3D�����⣨������ˣ�
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayers))
        {
            if (debugMode)
            {
                Debug.Log($"3D Hit: {hit.collider.name} with {key}");
                Debug.DrawLine(ray.origin, hit.point, Color.green, 1f);
            }
            TryTriggerClickable(hit.collider.gameObject, key);
        }
        // 2D�����⣨������ˣ�
        else if ((hit2D = Physics2D.GetRayIntersection(ray, Mathf.Infinity, clickableLayers)).collider != null)
        {
            if (debugMode)
            {
                Debug.Log($"2D Hit: {hit2D.collider.name} with {key}");
                Debug.DrawLine(ray.origin, hit2D.point, Color.blue, 1f);
            }

            TryTriggerClickable(hit2D.collider.gameObject, key);
        }
        else if (debugMode)
        {
            Debug.Log($"No valid object hit with {key}");
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 1f);
        }
    }

    private void TryTriggerClickable(GameObject target, MouseKey key)
    {
        IOnPlayerClick clickable = target.GetComponent<IOnPlayerClick>();
        Debug.Log($"{target.name}");
        if (clickable != null)
        {
            clickable.OnPlayerClick(key);
            if (debugMode)
                Debug.Log($"Triggered {key} on {target.name}");
        }
        else if (debugMode)
        {
            Debug.Log($"{target.name} has no IOnPlayerClick component");
        }
    }

   
    #endregion





    private void HandleZoomInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        if (scroll != 0)
        {
            // 2D����ͨ��ʹ�����������
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
                // �����͸�������������fieldOfView
                cam.fieldOfView = Mathf.Clamp(
                    cam.fieldOfView - scroll * zoomSpeed,
                    minZoom,
                    maxZoom
                );
            }
            
            // ���ź����¼���߽�
           // ClampCameraPosition();
        }
    }

    private void HandleDragInput()
    {
        if (Input.GetMouseButtonDown(0)) // �Ҽ���ʼ��ק
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0)) // �Ҽ�������ק
        {
          
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dragDelta = dragOrigin - currentPosition;
            transform.position += new Vector3(dragDelta.x, dragDelta.y, 0) * dragSpeed;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        // ��ȡ���λ�ã�����Z�ᣩ
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -initialZ; // ʹ�ø��ĳ�ʼZֵȷ����ȷת��
        
        return cam.ScreenToWorldPoint(mousePosition);
    }

    private void ClampCameraPosition()
    {
        Vector3 pos = transform.position;
        
        // ���㵱ǰ��Ұ�µ���Ч�߽�
        float effectiveZoom = cam.orthographic ? cam.orthographicSize : cam.fieldOfView / 2f;
        float aspectRatio = cam.aspect;
        
        // ����ʵ�ʱ߽磨���ǵ�ǰ��Ұ��С��
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
        
        // ȷ���߽���Ч����Сֵ���������ֵ��
        adjustedMinBoundary.x = Mathf.Min(adjustedMinBoundary.x, adjustedMaxBoundary.x);
        adjustedMinBoundary.y = Mathf.Min(adjustedMinBoundary.y, adjustedMaxBoundary.y);
        
        // ����X��λ��
        pos.x = Mathf.Clamp(pos.x, adjustedMinBoundary.x, adjustedMaxBoundary.x);
        
        // ����Y��λ��
        pos.y = Mathf.Clamp(pos.y, adjustedMinBoundary.y, adjustedMaxBoundary.y);
        
        // ����Z�᲻��
        pos.z = initialZ;
        
        transform.position = pos;
    }

    // �ڳ�����ͼ�л��Ʊ߽�
    void OnDrawGizmos()
    {
        if (!showBoundaryGizmo || !cam) return;
        
        Gizmos.color = Color.green;
        
        // ����߽��
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
        
        // ���Ʊ߽��
        Gizmos.DrawWireCube(center, size);
        
        // ���Ʊ߽�Ǳ��
        float markerSize = 0.5f;
        Gizmos.DrawSphere(new Vector3(minBoundary.x, minBoundary.y, 0), markerSize);
        Gizmos.DrawSphere(new Vector3(minBoundary.x, maxBoundary.y, 0), markerSize);
        Gizmos.DrawSphere(new Vector3(maxBoundary.x, minBoundary.y, 0), markerSize);
        Gizmos.DrawSphere(new Vector3(maxBoundary.x, maxBoundary.y, 0), markerSize);
        
        // ���Ƶ�ǰ��Ч�߽磨������Ұ��
        if (Application.isPlaying)
        {
            Gizmos.color = new Color(1, 0.5f, 0, 0.7f); // ��ɫ��͸��
            
            // ���㵱ǰ��Ч�߽�
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
            
            // ������Ч�߽��
            Vector3 effectiveCenter = (effectiveMin + effectiveMax) / 2;
            Vector3 effectiveSize = effectiveMax - effectiveMin;
            effectiveSize.z = 0.1f;
            
            Gizmos.DrawWireCube(effectiveCenter, effectiveSize);
        }
    }
}
