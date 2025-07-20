using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;



public enum Faciltie_Dir
{
    ��,
    ��,
    ����,
    ����,
    ����,
    ����,
}

public abstract class DirFaciltie : FacilitieBase
{

    protected static Faciltie_Dir[] dirIndex = new Faciltie_Dir[] {Faciltie_Dir.��,Faciltie_Dir.����,Faciltie_Dir.����,Faciltie_Dir.��,Faciltie_Dir.����,Faciltie_Dir.����};

    protected int currentDirIndex = 0;

    public Faciltie_Dir dir = Faciltie_Dir.��;

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
            case Faciltie_Dir.��:
                return 0f;
            case Faciltie_Dir.��:
                return 180f;
            case Faciltie_Dir.����:
                return 60f;
            case Faciltie_Dir.����:
                return -60f;
            case Faciltie_Dir.����:
                return 120f;
            case Faciltie_Dir.����:
                return -120f;
            default:
                Debug.LogError($"���뷽������ {dir}");
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
