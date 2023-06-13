using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class NPCTransition
{
    public NPCDecision Checkbehaviour;
    public NPCState trueBehaviour;
    public NPCState falseBehaviour;
}
