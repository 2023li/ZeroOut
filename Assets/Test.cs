using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{
    public HexGridGenerator map;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    [Button]
    public void T(Vector3Int t)
    {
        var pos =  HexGridGenerator.Path.FindPath(Vector3Int.zero, t,null);

        foreach (var p in pos) 
        {
            if (map.hexMap.ContainsKey(p))
            {
                map.hexMap[p].GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }
}
