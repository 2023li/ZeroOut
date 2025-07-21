using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum MapCellState
{
    不可用,
    锁定,
    已解锁,
}

public class MapCell : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteRenderer;

    public Vector3Int pos;
    // Start is called before the first frame update


    public bool isLock = true;

    public MapCellState state;

    public void SetState(MapCellState newState)
    {
        state = newState; 

        switch (state)
        {
            case MapCellState.不可用:
                spriteRenderer.color = new Color32(114, 114, 114, 114);
                break;
            case MapCellState.锁定:
                spriteRenderer.color = new Color32(134, 71, 71, 255);

                break;
            case MapCellState.已解锁:

                spriteRenderer.color = new Color32(175, 201, 220, 255);
                break;
            default:
                break;
        }
    }


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


    private void Start()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = -1;
    }



  


}
