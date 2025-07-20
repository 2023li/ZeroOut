using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    public int hp;
    public TMP_Text hpText;
    public Vector3Int startPoint;
    [SerializeField] private PathFind pathFind;

    public float MoveSpeed = 1;

    private void OnEnable()
    {

        hp = 1;
    }

    public void Init()
    {
        ChangeHP(hp);
    }
    // Start is called before the first frame update



    [Button]
    public void Move()
    {
        if (pathFind == null) { pathFind = GetComponent<PathFind>(); }
        pathFind.moveSpeed = MoveSpeed;
        pathFind.MoveAlongPath(HexGridGenerator.Path.FindCellPath(startPoint,Vector3Int.zero));

    }

    private void ChangeHP(int v)
    {
        hp = v;
        hpText.text=hp.ToString();

        if (hp <= 0) 
        {
            LeanPool.Despawn(gameObject);
        }

    }

    [Button]
    public void toPool()
    {

        LeanPool.Despawn(this.gameObject);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag==GameData.tag_Bullet) 
        {

            BulletBase blt = collision.gameObject.GetComponent<BulletBase>();

            ChangeHP(hp - blt.damage);
         
            
           blt.toPool();
        }
    }



}
