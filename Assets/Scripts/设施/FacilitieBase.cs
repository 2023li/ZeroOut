using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public  abstract class FacilitieBase : MonoBehaviour,IOnPlayerClick
{


    #region

    protected void Awake()
    {
        gameObject.layer = GameData.tagID_Faciltie;
    }


    public void OnPlayerClick(MouseKey key)
    {
        switch (key)
        {
            case MouseKey.������:
                OnMouseButtonDown_0();
                break;
            case MouseKey.����Ҽ�:
                OnMouseButtonDown_1();
                break;
        }
    }

    protected virtual void OnMouseButtonDown_0()
    {
        Debug.Log(gameObject.name +"�����������");
    }
    protected virtual void OnMouseButtonDown_1()
    {
        Debug.Log(gameObject.name + "������Ҽ����");
    }
    protected virtual void OnMouseButtonDown_2()
    {
        Debug.Log("��갴��2");
    }




    #endregion

}
