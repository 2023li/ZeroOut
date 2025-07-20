using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class EnemyLair : MonoBehaviour
{

    public int hp;

    public float spawnCD;
    public float currentCD;

    public GameObject Enemy;


    private void Update()
    {
        currentCD += Time.deltaTime;
        if (currentCD >= spawnCD) 
        {
            SpawnEnemy();
            currentCD = 0;
        }
    }

    private void SpawnEnemy()
    {
      


    }
}
