using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무브, 런 상태에서 꺼낼수있음
/// true 면 무기 꺼내고 false remain
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
            //무브 or 달리기 -> 전투모드
            controller.animator.myAnimator.SetTrigger("WeaponChange");
            controller.animator.myAnimator.SetInteger("Weapon", 2);      //인벤토리 시스템 만들면 그때 수정
            controller.moveState = PlayerMoveState.None;
            controller.behaviourState = PlayerBehaviourState.Attack;
            return true;
            
        }
        else if((Input.GetKeyDown("x") || !controller.playerInven.playerEquip.InSlotInputItem((int)ItemType.RightWeapon)) && controller.behaviourState == PlayerBehaviourState.Attack)
        {
            //전투모드 -> 무브
            controller.animator.myAnimator.SetInteger("Weapon", 0);
            controller.moveState = PlayerMoveState.Move;
            controller.behaviourState = PlayerBehaviourState.None;
            return true;
        }
        else if(Input.GetKeyDown("x"))
        {
            //controller.playerInven.InventoryUI.GetComponent<PlayerInventoryUI>().ShowAlertWindow("무기가 장착되어있지 않거나 할수없는 동작상태에 있습니다"); //임시방편
        }

        return false;
    }
}
