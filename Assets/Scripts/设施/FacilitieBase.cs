using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public  abstract class FacilitieBase : MonoBehaviour
{


    #region
    protected void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseButtonDown_0();
            // 左键处理逻辑
        }
        else if (Input.GetMouseButtonDown(1))
        {
            OnMouseButtonDown_1();
            // 右键处理逻辑
        }
        else if (Input.GetMouseButtonDown(2))
        {
            OnMouseButtonDown_2();
            // 中键处理逻辑
        }
    }


    protected virtual void OnMouseButtonDown_0()
    {

    }
    protected virtual void OnMouseButtonDown_1()
    {
        Debug.Log("鼠标按下1");
    }
    protected virtual void OnMouseButtonDown_2()
    {
        Debug.Log("鼠标按下2");
    }
    #endregion

}
