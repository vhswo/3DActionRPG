using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCDecision : ScriptableObject
{
    public abstract bool Decide(NPCBehaviourController controller);
    public virtual void Setting(NPCBehaviourController controller) { }
}
