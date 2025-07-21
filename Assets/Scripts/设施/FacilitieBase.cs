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
            case MouseKey.鼠标左键:
                OnMouseButtonDown_0();
                break;
            case MouseKey.鼠标右键:
                OnMouseButtonDown_1();
                break;
        }
    }

    protected virtual void OnMouseButtonDown_0()
    {
        Debug.Log(gameObject.name +"被鼠标左键点击");
    }
    protected virtual void OnMouseButtonDown_1()
    {
        Debug.Log(gameObject.name + "被鼠标右键点击");
    }
    protected virtual void OnMouseButtonDown_2()
    {
        Debug.Log("鼠标按下2");
    }




    #endregion

}
