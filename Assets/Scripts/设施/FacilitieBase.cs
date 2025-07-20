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
            // ��������߼�
        }
        else if (Input.GetMouseButtonDown(1))
        {
            OnMouseButtonDown_1();
            // �Ҽ������߼�
        }
        else if (Input.GetMouseButtonDown(2))
        {
            OnMouseButtonDown_2();
            // �м������߼�
        }
    }


    protected virtual void OnMouseButtonDown_0()
    {

    }
    protected virtual void OnMouseButtonDown_1()
    {
        Debug.Log("��갴��1");
    }
    protected virtual void OnMouseButtonDown_2()
    {
        Debug.Log("��갴��2");
    }
    #endregion

}
