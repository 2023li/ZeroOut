using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class FacilitieBase : MonoBehaviour
{


  


  

  



   

    protected void OnMouseDown()
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

    }
    protected virtual void OnMouseButtonDown_2()
    {

    }

}
