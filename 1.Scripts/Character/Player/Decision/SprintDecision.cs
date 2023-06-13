using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ���� -> �޸���
/// </summary>
[CreateAssetMenu(menuName = ("PluggableAI/Decision/SprintDecision"))]
public class SprintDecision : Decision
{
    public override bool Decide(BehaviourController controller)
    {
        return Input.GetButton("Run") && controller.moveState == PlayerMoveState.Move;
    }

}
