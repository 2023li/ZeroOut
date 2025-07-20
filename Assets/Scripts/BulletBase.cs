using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEditor.Purchasing;
using UnityEngine;

public class BulletBase : MonoBehaviour
{


    

    public string idName = "";

    [Header("�ƶ�����")]
    [Tooltip("�ƶ��ٶ� (��λ/��)")]
    public float moveSpeed = 1f;

    public float lifeTime = 10f;

    public int damage = 1;

    

    private void OnEnable()
    {
        lifeTime = 10f;
    }

    void Update()
    {
        // ��ȡ���������Y�᷽�����淽��
        Vector3 forwardDirection = transform.up;

        // �����ƶ��������ٶ� * ���� * ʱ�䣩
        Vector3 movement = forwardDirection * moveSpeed * Time.deltaTime;

        // Ӧ���ƶ�
        transform.position += movement;




        lifeTime -= Time.deltaTime;
        if (lifeTime<=0)
        {
            LeanPool.Despawn(this.gameObject);
        }
    }


    public void toPool()
    {
        if (this.gameObject.activeSelf)
        {
            LeanPool.Despawn(this.gameObject);
        }

        
    }
}
