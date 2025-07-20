using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MapCell : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;

    public Vector3 pos;
    // Start is called before the first frame update


    public bool isLock = true;


 
    [Button]
    public void SetLock(bool isLock)
    {
        if (spriteRenderer==null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        this.isLock = isLock;
        if (isLock)
        {
            spriteRenderer.color = new Color32(114,114,114,255);
            
        }
        else
        {
            spriteRenderer.color = new Color32(255, 255, 255, 255);
        }

    }


}
