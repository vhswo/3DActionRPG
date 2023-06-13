using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPC에 말걸어서 상점이 열린 상태
/// 각 상인마다 들고 있는 것을 로드 해온다
/// 카메라는 상인을 집중 포커스 하며, 일정 거리가 멀어지면 자동으로 창이 닫히고, 플레이로 카메라로 바뀐다
/// 아이템 이미지, 아이템네임, 살때 가격 //마우스 가져다 되면 - > 공격력 or 방어력, 추가 ...
/// </summary>
[CreateAssetMenu(menuName = ("PluggableAI/NPC/Behaviour/Trade"))]
public class TradeBehaviour : NPCGenericBehaviour
{
    public override void Setting(NPCBehaviourController controller)
    {
        controller.InteractionHUDByPlayer.gameObject.SetActive(false); //들어왔으니 더이상 안보이게 끈다
        controller.InventoryUI.gameObject.SetActive(true);
        for(int i = 0; i < controller.haveItems.Length; i++)
        {
            controller.onlyOtherInven.AddItem(ItemManager.instance.GetItemClip(controller.haveItems[i]),1);
        }


    }

    public override void Action(NPCBehaviourController controller)
    {

    }
}
