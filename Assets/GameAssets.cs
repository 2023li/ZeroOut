using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameAssets", menuName = "Game/GameAssets", order = 1)]
public class GameAssets : ScriptableObject
{

    [SerializeField] private BulletBase[] AllBulletPerfabs;



    private Dictionary<string, GameObject> dic_bullet;
    public void Init()
    {
        dic_bullet = new Dictionary<string, GameObject>();
        foreach (var bullet in AllBulletPerfabs)
        {
            dic_bullet.Add(bullet.idName, bullet.gameObject);
        }

    }


    public GameObject GetBullet(string id)
    {

        return dic_bullet[id];
    }

}
