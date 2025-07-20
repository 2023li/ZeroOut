using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;



public enum Faciltie_Dir
{
    上,
    下,
    左上,
    右上,
    左下,
    右下,
}

public abstract class DirFaciltie : FacilitieBase
{

    protected static Faciltie_Dir[] dirIndex = new Faciltie_Dir[] {Faciltie_Dir.上,Faciltie_Dir.右上,Faciltie_Dir.右下,Faciltie_Dir.下,Faciltie_Dir.左下,Faciltie_Dir.左上};

    protected int currentDirIndex = 0;

    public Faciltie_Dir dir = Faciltie_Dir.上;

    public Transform rotateGO;

    protected override void OnMouseButtonDown_0()
    {
        OnClickChangeDir();

    }

    protected void OnClickChangeDir()
    {
        currentDirIndex++;
        if (currentDirIndex>= DirFaciltie.dirIndex.Length)
        {
            currentDirIndex = 0;
        }

        Rotate(dirIndex[currentDirIndex]);
    }

    public float GetDirAngle()
    {
        switch (this.dir)
        {
            case Faciltie_Dir.上:
                return 0f;
            case Faciltie_Dir.下:
                return 180f;
            case Faciltie_Dir.左上:
                return 60f;
            case Faciltie_Dir.右上:
                return -60f;
            case Faciltie_Dir.左下:
                return 120f;
            case Faciltie_Dir.右下:
                return -120f;
            default:
                Debug.LogError($"传入方向有误 {dir}");
                return 0f;
        }
    }
    [Button]
    public virtual void Rotate(Faciltie_Dir dir)
    {
        this.dir = dir;

        rotateGO.eulerAngles = new Vector3(0, 0, GetDirAngle());

        
    }

   

}
