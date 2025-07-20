using System.Collections.Generic;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

public class TheGame : MonoBehaviour
{

    public bool start = false;
    public int runSeconds = 0;
    public float time;

    public float SpawnCD = 1;


    public GameAssets Assets;

    public GameObject prefab_Enemy;

    public static TheGame Instance {  get; private set; }

    [ShowInInspector]
    public  Dictionary<Vector3Int,Vector3> enemySpawnPoint;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {

      Assets.Init();

    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {

            time += Time.deltaTime;
            if (time >= 1) 
            {
                runSeconds++;
                time = 0;   
            }



            SpawnCD -= Time.deltaTime;
            if(SpawnCD < 0)
            {
                SpawnEnemy();
                SpawnCD = SetSpawnCD();

            }




        }


    }

    [Button]
    private void GetEnemySpawnPoint()
    {
        enemySpawnPoint = new Dictionary<Vector3Int,Vector3>();
        int mapR = HexGridGenerator.Instance.gridRadius;
        
        foreach (Vector3Int pos in HexGridGenerator.Instance.hexMap.Keys)
        {
            if (pos.x==mapR ||pos.y==mapR||pos.z==mapR ) 
            {
                enemySpawnPoint.Add(pos,HexGridGenerator.Instance.hexMap[pos].transform.position);
            }
        }      
    }

    [Button]
    public void SpawnEnemy()
    {
        if (enemySpawnPoint==null)
        {
            GetEnemySpawnPoint();
        }

        Enemy e =  LeanPool.Spawn(prefab_Enemy).GetComponent<Enemy>();

        int index =  UnityEngine.Random.Range(0,enemySpawnPoint.Keys.Count);

        Vector3Int pos = GetRandomSpawnKey();

        e.hp = SetEnemyHp();
        e.Init();
        e.startPoint = pos;
        e.transform.position = enemySpawnPoint[pos];
        e.Move();

        

    }



    private float SetSpawnCD()
    {
        return 1;
    }
    private int SetEnemyHp()
    {
        return 1;
    }


    /// <summary>
    /// 获取一个随机的生成位置
    /// </summary>
    /// <returns></returns>
    public Vector3Int GetRandomSpawnKey()
    {
        if (enemySpawnPoint == null || enemySpawnPoint.Count == 0)
        {
            Debug.LogWarning("enemySpawnPoint为空，返回Vector3Int.zero");
            return Vector3Int.zero;
        }

        // 将字典的键转换为列表
        List<Vector3Int> keysList = new List<Vector3Int>(enemySpawnPoint.Keys);

        // 随机选择一个索引
        int randomIndex = UnityEngine.Random.Range(0, keysList.Count);

        // 返回随机选择的键
        return keysList[randomIndex];
    }





}
