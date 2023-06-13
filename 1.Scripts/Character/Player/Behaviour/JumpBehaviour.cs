using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 달리기 -> 점프
/// 무브 -> 점프
/// </summary>
[CreateAssetMenu(menuName = ("PluggableAI/Behaviour/Jump"))]
public class JumpBehaviour : GenericBehaviour
{
    bool jump;
    /// <summary>
    /// 달리기 -> 점프
    /// 무브 -> 점프
    /// 일때의 세팅
    /// </summary>
    public override void Setting(BehaviourController controller)
    {
        controller.moveState = PlayerMoveState.Jump;
    }

    public override void Action(BehaviourController controller)
    {
        if (controller.variables.Jumpping)
        {
            if(controller.rigid.velocity.y < 0 && controller.IsGround())
            {
                controller.variables.Jumpping = false;
            }
            return;
        }


        if (controller.h <= Mathf.Epsilon && controller.v <= Mathf.Epsilon)
        {
            controller.animator.myAnimator.SetInteger("Jump", 0);
        }
        else
        {
            controller.animator.myAnimator.SetInteger("Jump", 1);
            controller.rigid.AddForce(Vector3.up * Mathf.Sqrt(1.1f * Mathf.Abs(Physics.gravity.y)), ForceMode.VelocityChange);
        }
        controller.animator.myAnimator.SetBool("IsGround", false);

        controller.variables.Jumpping = true;
    }


}
