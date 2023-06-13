using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� -> �޸���
/// </summary>
[CreateAssetMenu(menuName = ("PluggableAI/Behaviour/Sprint"))]
public class SprintBehaviour : GenericBehaviour
{
    private float sprintFOV = 100f;
    public override void Setting(BehaviourController controller)
    {
        //���꿡�� �޸���� �ö� �����ؾ��Ұ�
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
