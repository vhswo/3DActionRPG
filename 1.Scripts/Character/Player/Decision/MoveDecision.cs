using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �޸��� -> ����
/// ���� -> ����
/// </summary>
[CreateAssetMenu(menuName = ("PluggableAI/Decision/Move"))]
public class MoveDecision : Decision
{
    public bool sprintToMove;
    public override bool Decide(BehaviourController controller)
    {
        if (sprintToMove) return SprintToMove(controller);
        else return JumpToMove(controller);
    }

    //�޸��� -> ����
    public bool SprintToMove(BehaviourController controller)
    {
        return !Input.GetButton("Run") && controller.moveState == PlayerMoveState.Sprint;
    }

    //���� -> ����
    public bool JumpToMove(BehaviourController controller)
    {
        return controller.moveState == PlayerMoveState.Jump && controller.IsGround() && !controller.variables.Jumpping;
    }
}
