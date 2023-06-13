using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchDecision : Decision
{
    public string CrouchKey = "LC";
    public int CrouchHashCode;

    //�ִϸ��̼� �� �ӵ��� �ٲ�
    public override bool Decide(BehaviourController behaviour)
    {
        if(Input.GetButtonDown(CrouchKey))
        {
            switch(behaviour.moveState)
            {
                case PlayerMoveState.Jump:
                    return false;
                case PlayerMoveState.Move:
                    behaviour.moveState = PlayerMoveState.Crouch;
                    return true;
                case PlayerMoveState.Sprint:
                    return false;
                case PlayerMoveState.Crouch:
                    behaviour.moveState = PlayerMoveState.Move;
                    return true;
                default:
                    return false;
            }
        }

        return false;
    }

}
