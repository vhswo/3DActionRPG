using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyDecision : ScriptableObject
{
    public abstract bool Decide(EnemyBehaviourController controller);
    public virtual void Setting(EnemyBehaviourController controller) { }
}
