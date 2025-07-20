using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;

public class HexGridGenerator : MonoBehaviour
{

    public static HexGridGenerator Instance 
    {
        get;private set;
    }


    public GameObject hexPrefab; // ������Ԥ����
    public int gridRadius = 3;   // ����뾶��������������չ�Ĳ�����
    [LabelText("�����ΰ뾶")]
    public float hexSize = 1f;   // �����δ�С�����ĵ�����ľ��룩
    public bool flatTop = true;  // true=ƽ�������Σ�false=�ⶥ������

    public Dictionary<Vector3Int, GameObject> hexMap = new Dictionary<Vector3Int, GameObject>();


    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else
        {
            Debug.Log("�����ظ��ĵ�ͼʵ��");
            Destroy(this.gameObject);
        }
        
        
    }
    void Start()
    {
        GenerateHexGrid();
    }

    [Button]
    public void GenerateHexGrid()
    {
        // ���������
        ClearGrid();

        // ����������
        for (int x = -gridRadius; x <= gridRadius; x++)
        {
            for (int y = Mathf.Max(-gridRadius, -x - gridRadius); y <= Mathf.Min(gridRadius, -x + gridRadius); y++)
            {
                int z = -x - y;
                if (Mathf.Abs(z) <= gridRadius)
                {
                    CreateHex(new Vector3Int(x, y, z));
                }
            }
        }
    }

    private void ClearGrid()
    {
        // ʹ����ʱ�б�����޸��еļ��ϴ���
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        foreach (GameObject child in children)
        {
            if (Application.isPlaying)
            {
                Destroy(child);
            }
            else
            {
                DestroyImmediate(child);
            }
        }
        hexMap.Clear();
    }

    private void CreateHex(Vector3Int cubeCoords)
    {
        Vector3 position = CalculatePosition(cubeCoords);
        GameObject hex = Instantiate(hexPrefab, position, Quaternion.identity, transform);
        hex.name = $"Hex_{cubeCoords.x}_{cubeCoords.y}_{cubeCoords.z}";

        hex.gameObject.GetComponent<MapCell>().pos = cubeCoords;

        hexMap.Add(cubeCoords, hex);
    }

    private Vector3 CalculatePosition(Vector3Int cubeCoords)
    {
        float xPos, yPos;
        float sqrt3 = Mathf.Sqrt(3f);
        float horizontalSpacing = hexSize * sqrt3;   // ˮƽ���
        float verticalSpacing = hexSize * 1.5f;      // ��ֱ���

        if (flatTop)
        {
            // �������ƽ�����ֹ�ʽ
            xPos = horizontalSpacing * (cubeCoords.x + cubeCoords.z * 0.5f);
            yPos = verticalSpacing * cubeCoords.z;
        }
        else
        {
            // ������ļⶥ���ֹ�ʽ
            xPos = verticalSpacing * cubeCoords.x;
            yPos = horizontalSpacing * (cubeCoords.z + cubeCoords.x * 0.5f);
        }
        return new Vector3(xPos, yPos, 0);
    }

    // �ڱ༭���е�����
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            for (int x = -gridRadius; x <= gridRadius; x++)
            {
                for (int y = Mathf.Max(-gridRadius, -x - gridRadius); y <= Mathf.Min(gridRadius, -x + gridRadius); y++)
                {
                    int z = -x - y;
                    if (Mathf.Abs(z) <= gridRadius)
                    {
                        Vector3 pos = CalculatePosition(new Vector3Int(x, y, z));
                        Gizmos.DrawWireSphere(transform.position + pos, hexSize * 0.2f);
                    }
                }
            }
        }
    }




    public class Path
    {
        // �����η������������������꣩
        static readonly Vector3Int[] Directions =
        {
        new Vector3Int(1, -1, 0),
        new Vector3Int(1, 0, -1),
        new Vector3Int(0, 1, -1),
        new Vector3Int(-1, 1, 0),
        new Vector3Int(-1, 0, 1),
        new Vector3Int(0, -1, 1)
    };
        struct PathNode
        {
            public Vector3Int position;
            public float gScore;
            public float fScore;

            public PathNode(Vector3Int pos, float g, float f)
            {
                position = pos;
                gScore = g;
                fScore = f;
            }
        }
        class NodeComparer : IComparer<PathNode>
        {
            public int Compare(PathNode a, PathNode b)
            {
                return a.fScore.CompareTo(b.fScore);
            }
        }
        class PriorityQueue<T> where T : struct
        {
            private List<T> data;
            private IComparer<T> comparer;

            public int Count => data.Count;

            public PriorityQueue(IComparer<T> comparer)
            {
                this.data = new List<T>();
                this.comparer = comparer;
            }

            public void Enqueue(T item)
            {
                data.Add(item);
                int child = data.Count - 1;
                while (child > 0)
                {
                    int parent = (child - 1) / 2;
                    if (comparer.Compare(data[child], data[parent]) >= 0)
                        break;

                    T tmp = data[child];
                    data[child] = data[parent];
                    data[parent] = tmp;
                    child = parent;
                }
            }

            public T Dequeue()
            {
                int last = data.Count - 1;
                T front = data[0];
                data[0] = data[last];
                data.RemoveAt(last);
                last--;

                int parent = 0;
                while (true)
                {
                    int left = parent * 2 + 1;
                    if (left > last) break;

                    int right = left + 1;
                    int min = left;

                    if (right <= last && comparer.Compare(data[right], data[left]) < 0)
                        min = right;

                    if (comparer.Compare(data[parent], data[min]) <= 0)
                        break;

                    T tmp = data[parent];
                    data[parent] = data[min];
                    data[min] = tmp;
                    parent = min;
                }

                return front;
            }
        }

        public static List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal, HashSet<Vector3Int> obstacles= null)
        {
            // ��֤����Լ��
            if (start.x + start.y + start.z != 0 || goal.x + goal.y + goal.z != 0)
            {
                Debug.LogError("���겻����������Լ��: x + y + z = 0");
                return new List<Vector3Int>();
            }

            // ��ʼ�����ݽṹ
            PriorityQueue<PathNode> openSet = new PriorityQueue<PathNode>(new NodeComparer());
            Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
            Dictionary<Vector3Int, float> gScore = new Dictionary<Vector3Int, float>();
            Dictionary<Vector3Int, float> fScore = new Dictionary<Vector3Int, float>();

            // ����������
            gScore[start] = 0;
            fScore[start] = Heuristic(start, goal);
            openSet.Enqueue(new PathNode(start, 0, fScore[start]));

            while (openSet.Count > 0)
            {
                PathNode current = openSet.Dequeue();

                // ����Ŀ��
                if (current.position == goal)
                {
                    return ReconstructPath(cameFrom, current.position);
                }

                // ������·��
                if (current.gScore > gScore[current.position])
                    continue;

                // ̽���ھ�
                foreach (Vector3Int dir in Directions)
                {
                    Vector3Int neighbor = current.position + dir;

                    // ��֤����������Լ��
                    if (neighbor.x + neighbor.y + neighbor.z != 0)
                        continue;

                    // �����ϰ���
                    if (obstacles!=null&&obstacles.Contains(neighbor))
                        continue;

                    // ������Gֵ���������ƶ��ɱ�Ϊ1��
                    float tentativeG = gScore[current.position] + 1;

                    // ���δ���ʹ��ýڵ㣬���ҵ�����·��
                    if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current.position;
                        gScore[neighbor] = tentativeG;
                        fScore[neighbor] = tentativeG + Heuristic(neighbor, goal);

                        // ʹ���Զ������ȶ���
                        openSet.Enqueue(new PathNode(neighbor, tentativeG, fScore[neighbor]));
                    }
                }
            }

            // δ�ҵ�·��
            return new List<Vector3Int>();
        }

        public static List<Vector3> FindCellPath(Vector3Int start, Vector3Int goal, HashSet<Vector3Int> obstacles = null)
        {
            List<Vector3> rPos = new List<Vector3>();
            var pos = Path.FindPath(start, goal, obstacles);

            foreach (Vector3Int p in pos) 
            {
                if (Instance.hexMap.ContainsKey(p))
                {
                    rPos.Add(Instance.hexMap[p].transform.position);
                }
                else
                {
                    Debug.LogError($"���� {p} ��Ӧ�ĵ�Ԫ�񲻴���");
                }
            }


            return rPos;
        }


        // �����������پ���
        private static float Heuristic(Vector3Int a, Vector3Int b)
        {
            return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z)) / 2f;
        }

        // �ؽ�·��
        private static List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
        {
            List<Vector3Int> path = new List<Vector3Int> { current };

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Insert(0, current);
            }

            return path;
        }

    }



}