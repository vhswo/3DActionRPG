using System;

[Serializable]

public class EnemyTransition
{
    public EnemyDecision Checkbehaviour;
    public EnemyState trueBehaviour;
    public EnemyState falseBehaviour;
}
