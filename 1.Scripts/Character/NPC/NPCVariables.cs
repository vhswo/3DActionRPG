using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class NPCVariables
{
    public bool NearPlayerCheckGizmo = true; //����� OnOff
    public float PlayerCheckRadius =3f;   //��ü�κ����� radius
    public NPCType type;
    public LayerMask Target;         // ã�� Ÿ��

}
