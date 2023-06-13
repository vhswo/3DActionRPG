using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� -> ����
/// ���� -> �޸���
/// </summary>
[CreateAssetMenu(menuName = ("PluggableAI/Behaviour/Move"))]
public class MoveBehaviour : GenericBehaviour
{
    //���� -> ����
    //�޸��� -> ����
    //�� �������� ����
    public override void Setting(BehaviourController controller)
    {
        controller.moveState = PlayerMoveState.Move;

        if (controller.myCamScript) controller.myCamScript.ResetFOV();
        controller.animator.myAnimator.SetBool("Sprint", false);
        controller.animator.myAnimator.SetBool("IsGround", true);
        controller.animator.myAnimator.SetBool("Cover", false);
        controller.animator.myAnimator.SetBool("AttackMode", false);
        controller.animator.myAnimator.SetInteger("Jump", -1);
    }
    public override void Action(BehaviourController controller)
    {
        controller.MoveValue();
    }

}
