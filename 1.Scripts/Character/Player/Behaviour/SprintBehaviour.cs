using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무브 -> 달리기
/// </summary>
[CreateAssetMenu(menuName = ("PluggableAI/Behaviour/Sprint"))]
public class SprintBehaviour : GenericBehaviour
{
    private float sprintFOV = 100f;
    public override void Setting(BehaviourController controller)
    {
        //무브에서 달리기로 올때 셋팅해야할것
        controller.moveState = PlayerMoveState.Sprint;
    }

    public override void Action(BehaviourController controller)
    {
        controller.animator.myAnimator.SetBool("Sprint", true);

        controller.MoveValue();

        if(controller.myCamScript)
        {
            controller.myCamScript.SetFOV(sprintFOV);
        }
    }

}
