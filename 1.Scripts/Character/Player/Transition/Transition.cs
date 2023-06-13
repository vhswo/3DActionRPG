using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Transition
{
    public Decision Checkbehaviour;
    public State trueBehaviour;
    public State falseBehaviour;
}
