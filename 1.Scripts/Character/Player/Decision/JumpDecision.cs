using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 무브 -> 점프
/// 달리기 -> 점프
/// </summary>
[CreateAssetMenu(menuName = ("PluggableAI/Decision/Jump"))]
public class JumpDecision : Decision
{
    public bool moveToJump;

    public override bool Decide(BehaviourController controller)
    {
        if (moveToJump) return MoveToJump(controller);
        else return SprintToJump(controller);
    }

    public bool MoveToJump(BehaviourController controller)
    {
        return controller.moveState == PlayerMoveState.Move && Input.GetButtonDown("Jump");
    }

    public bool SprintToJump(BehaviourController controller)
    {
        return controller.moveState == PlayerMoveState.Sprint && Input.GetButton("Jump");
    }
}
