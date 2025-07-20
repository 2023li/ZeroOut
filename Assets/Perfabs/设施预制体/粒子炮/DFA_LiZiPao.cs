using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

public class DFA_LiZiPao : DirFaciltie
{


    [SerializeField] private GameObject blt;

    [SerializeField] private Transform point;


    public static float AttackCD = 1f;

    public float currentAttackCD;


    private void OnEnable()
    {
        currentAttackCD = AttackCD; 
    }

    

   
    void Update()
    {
        currentAttackCD-=Time.deltaTime;
        if (currentAttackCD<=0)
        {
            Attack();
            currentAttackCD = AttackCD;
        }
    }



  
    [Button]
    public void Attack()
    {
        GameObject blt = LeanPool.Spawn(this.blt);
        blt.transform.eulerAngles =new Vector3(0,0,GetDirAngle());
        blt.transform.position = point.transform.position;
    }


}
