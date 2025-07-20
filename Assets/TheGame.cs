using System.Collections.Generic;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;


public class TheGame : MonoBehaviour
{

    [LabelText("ʵ�ʵ�ͼ�뾶")]
    public int MapRadius = 24;
    [LabelText("���ð뾶")]
    public int UnusableRadius = 12;
    [LabelText("�������뾶")]
    public int lockRadius = 7;
    [LabelText("�ѽ����뾶")]
    public int unlockRadius = 6;


    [LabelText("��ʼˢ��")]
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


    //������������(Ĭ�����ˢ����)
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
    /// �������������������ӳ��
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
    /// ��ָ�������ˢ��
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
    /// ��ȡ������������(����Ĭ��λ��ˢ��)
    /// </summary>
    /// <returns></returns>
    public List<Vector3Int> GetOuterRingCoordinates()
    {
      
        // ��ȡ��ǰ���ɵ������ε�ͼ
        var hexMap = HexGridGenerator.Instance.hexMap;

        foreach (Vector3Int pos in hexMap.Keys)
        {
            // ���������ε����ĵľ���
            int distance = Mathf.Max(
                Mathf.Abs(pos.x),
                Mathf.Abs(pos.y),
                Mathf.Abs(pos.z)
            );

            // ���������ڵ�ͼ�뾶�������������
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
            // ���������ε�����(0,0,0)��ʵ�ʾ���
            int distance = Mathf.Max(
                Mathf.Abs(pos.x),
                Mathf.Abs(pos.y),
                Mathf.Abs(pos.z)
            );

            MapCell cell = hexMap[pos].GetComponent<MapCell>();

            if (distance > UnusableRadius)
            {
                cell.SetState(MapCellState.������);
            }
            else if (distance > lockRadius)
            {
                // �ڽ�������δ��������ɫ
                cell.SetState(MapCellState.����);
            }
            else
            {
                // ����������ɫ
                cell.SetState(MapCellState.�ѽ���);
            }
        }
    }



}
