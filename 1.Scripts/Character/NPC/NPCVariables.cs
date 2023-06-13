using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class NPCVariables
{
    public bool NearPlayerCheckGizmo = true; //기즈모 OnOff
    public float PlayerCheckRadius =3f;   //객체로부터의 radius
    public NPCType type;
    public LayerMask Target;         // 찾을 타겟

}
