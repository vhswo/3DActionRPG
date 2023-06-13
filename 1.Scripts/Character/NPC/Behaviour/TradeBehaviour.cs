using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPC�� ���ɾ ������ ���� ����
/// �� ���θ��� ��� �ִ� ���� �ε� �ؿ´�
/// ī�޶�� ������ ���� ��Ŀ�� �ϸ�, ���� �Ÿ��� �־����� �ڵ����� â�� ������, �÷��̷� ī�޶�� �ٲ��
/// ������ �̹���, �����۳���, �춧 ���� //���콺 ������ �Ǹ� - > ���ݷ� or ����, �߰� ...
/// </summary>
[CreateAssetMenu(menuName = ("PluggableAI/NPC/Behaviour/Trade"))]
public class TradeBehaviour : NPCGenericBehaviour
{
    public override void Setting(NPCBehaviourController controller)
    {
        controller.InteractionHUDByPlayer.gameObject.SetActive(false); //�������� ���̻� �Ⱥ��̰� ����
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
