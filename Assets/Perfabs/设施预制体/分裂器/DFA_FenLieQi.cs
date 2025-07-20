using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using System.Drawing;
using UnityEngine;

public class DFA_FenLieQi : DirFaciltie
{

    public Transform point_60;
    public Vector3 EmissionAngle1;
    public Transform point_f60;
    public Vector3 EmissionAngle2;


    private void Start()
    {
        ignoreGO = new HashSet<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag==GameData.tag_Bullet) 
        {
            if (ignoreGO.Contains(collision.gameObject))
            {
                return;
            }



            BulletBase blt =  collision.GetComponent<BulletBase>();
            string id = blt.idName;
            blt.toPool();

            Fission(TheGame.Instance.Assets.GetBullet(id));

        }

    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (ignoreGO.Contains(collider.gameObject))
        {
            ignoreGO.Remove(collider.gameObject);
        }

    }





    HashSet<GameObject> ignoreGO;

    //
    private void Fission(GameObject bullet)
    {

        GameObject blt = LeanPool.Spawn(bullet);
        blt.transform.eulerAngles = new Vector3(0, 0, GetDirAngle()+60);
        blt.transform.position = point_60.transform.position;

        GameObject blt2 = LeanPool.Spawn(bullet);
        blt2.transform.eulerAngles = new Vector3(0, 0, GetDirAngle() -60);
        blt2.transform.position = point_f60.transform.position;


        ignoreGO.Add(blt);
        ignoreGO.Add(blt2);

    }
}




