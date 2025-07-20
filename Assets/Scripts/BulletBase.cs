using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEditor.Purchasing;
using UnityEngine;

public class BulletBase : MonoBehaviour
{


    

    public string idName = "";

    [Header("移动参数")]
    [Tooltip("移动速度 (单位/秒)")]
    public float moveSpeed = 1f;

    public float lifeTime = 10f;

    public int damage = 1;

    

    private void OnEnable()
    {
        lifeTime = 10f;
    }

    void Update()
    {
        // 获取物体自身的Y轴方向（正面方向）
        Vector3 forwardDirection = transform.up;

        // 计算移动向量（速度 * 方向 * 时间）
        Vector3 movement = forwardDirection * moveSpeed * Time.deltaTime;

        // 应用移动
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
