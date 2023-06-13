using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����, �� ���¿��� ����������
/// true �� ���� ������ false remain
/// </summary>
[CreateAssetMenu(menuName = ("PluggableAI/Decision/ChangeWeaponDecision"))]
public class QuickDecision : Decision
{
    public override bool Decide(BehaviourController controller)
    {
        return CheckEquip(controller);
    }

    public bool CheckEquip(BehaviourController controller)
    {
        if(Input.GetKeyDown("x") && controller.moveState != PlayerMoveState.Jump && controller.playerInven.playerEquip.InSlotInputItem((int)ItemType.RightWeapon))
        {
            //���� or �޸��� -> �������
            controller.animator.myAnimator.SetTrigger("WeaponChange");
            controller.animator.myAnimator.SetInteger("Weapon", 2);      //�κ��丮 �ý��� ����� �׶� ����
            controller.moveState = PlayerMoveState.None;
            controller.behaviourState = PlayerBehaviourState.Attack;
            return true;
            
        }
        else if((Input.GetKeyDown("x") || !controller.playerInven.playerEquip.InSlotInputItem((int)ItemType.RightWeapon)) && controller.behaviourState == PlayerBehaviourState.Attack)
        {
            //������� -> ����
            controller.animator.myAnimator.SetInteger("Weapon", 0);
            controller.moveState = PlayerMoveState.Move;
            controller.behaviourState = PlayerBehaviourState.None;
            return true;
        }
        else if(Input.GetKeyDown("x"))
        {
            //controller.playerInven.InventoryUI.GetComponent<PlayerInventoryUI>().ShowAlertWindow("���Ⱑ �����Ǿ����� �ʰų� �Ҽ����� ���ۻ��¿� �ֽ��ϴ�"); //�ӽù���
        }

        return false;
    }
}
