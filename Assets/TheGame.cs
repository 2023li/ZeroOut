using System.Collections.Generic;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;


public class TheGame : MonoBehaviour
{

    [LabelText("实际地图半径")]
    public int MapRadius = 24;
    [LabelText("可用半径")]
    public int UnusableRadius = 12;
    [LabelText("待解锁半径")]
    public int lockRadius = 7;
    [LabelText("已解锁半径")]
    public int unlockRadius = 6;


    [LabelText("开始刷怪")]
    public bool start = false;
    public int runSeconds = 0;
    public float time;

    public float SpawnCD = 1;


    public GameAssets Assets;

    public GameObject prefab_Enemy;

    public static TheGame Instance { get; private set; }

    //
    [ShowInInspector]
    public Dictionary<Vector3Int, Vector3> dicCellPosToWorldPos;


    //最外层的六边形(默认随机刷怪用)
    [ShowInInspector]
    List<Vector3Int> outerRing = new List<Vector3Int>();

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

        TheGameInit();

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
            if (SpawnCD < 0)
            {
                SpawnEnemy(outerRing[UnityEngine.Random.Range(0, outerRing.Count)]);
                SpawnCD = SetSpawnCD();

            }
        }


    }
    [Button]
    public void TheGameInit()
    {
        InitMap();
        CellPosToWroldPos();
        GetOuterRingCoordinates();
    }

    /// <summary>
    /// 建立坐标点和世界坐标的映射
    /// </summary>
    private void CellPosToWroldPos()
    {
        dicCellPosToWorldPos = new Dictionary<Vector3Int, Vector3>();
        int mapR = MapRadius;
        Debug.Log("mapr:" + mapR);
        foreach (Vector3Int pos in HexGridGenerator.Instance.hexMap.Keys)
        {

            dicCellPosToWorldPos.Add(pos, HexGridGenerator.Instance.hexMap[pos].transform.position);

        }
    }





    /// <summary>
    /// 在指定坐标格刷怪
    /// </summary>
    /// <param name="mapCellPos"></param>
    public void SpawnEnemy(Vector3Int mapCellPos)
    {
        if (dicCellPosToWorldPos == null)
        {
            CellPosToWroldPos();
        }

        Enemy e = LeanPool.Spawn(prefab_Enemy).GetComponent<Enemy>();

        Vector3Int pos = mapCellPos;

        e.hp = SetEnemyHp();
        e.Init();
        e.startPoint = pos;
        //pos(0,0,0)
        e.transform.position = dicCellPosToWorldPos[pos];
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
    /// 获取最外层的六边形(用于默认位置刷怪)
    /// </summary>
    /// <returns></returns>
    public List<Vector3Int> GetOuterRingCoordinates()
    {
      
        // 获取当前生成的六边形地图
        var hexMap = HexGridGenerator.Instance.hexMap;

        foreach (Vector3Int pos in hexMap.Keys)
        {
            // 计算六边形到中心的距离
            int distance = Mathf.Max(
                Mathf.Abs(pos.x),
                Mathf.Abs(pos.y),
                Mathf.Abs(pos.z)
            );

            // 如果距离等于地图半径，则属于最外层
            if (distance == MapRadius)
            {
                outerRing.Add(pos);
            }
        }

        return outerRing;
    }





    /*
    public int MapRadius = 24;
    public int UnusableRadius = 12;
    public int lockRadius = 7;
    public int unlockRadius = 6;
     */

    [Button]
    public void InitMap()
    {
        HexGridGenerator.Instance.GenerateHexGrid(MapRadius);
        var hexMap = HexGridGenerator.Instance.hexMap;

        foreach (Vector3Int pos in hexMap.Keys)
        {
            // 计算六边形到中心(0,0,0)的实际距离
            int distance = Mathf.Max(
                Mathf.Abs(pos.x),
                Mathf.Abs(pos.y),
                Mathf.Abs(pos.z)
            );

            MapCell cell = hexMap[pos].GetComponent<MapCell>();

            if (distance > UnusableRadius)
            {
                cell.SetState(MapCellState.不可用);
            }
            else if (distance > lockRadius)
            {
                // 在解锁区但未锁定：红色
                cell.SetState(MapCellState.锁定);
            }
            else
            {
                // 锁定区域：绿色
                cell.SetState(MapCellState.已解锁);
            }
        }
    }



}
