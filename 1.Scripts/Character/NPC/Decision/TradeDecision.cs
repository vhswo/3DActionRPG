using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �÷��̾ NPC�� �����Ÿ��� �ִ��� üũ
/// ������ ���� �ɼ� �ִ� UI ����
/// ���ɸ� TradeBehaviour ���·� �Ѿ
/// </summary>
[CreateAssetMenu(menuName = ("PluggableAI/NPC/Decision/Trade"))]
public class TradeDecision : NPCDecision
{
    public override void Setting(NPCBehaviourController controller)
    {

    }

    public override bool Decide(NPCBehaviourController controller)
    {
        return NearPlayer(controller);
    }

    float time = 0f;
    public bool  NearPlayer(NPCBehaviourController controller)
    {
        time += Time.deltaTime;
        Collider[] player = Physics.OverlapSphere(controller.transform.position, controller.variables.PlayerCheckRadius, controller.variables.Target);

        Transform talkHUD = controller.InteractionHUDByPlayer;
        if (player.Length <= 0)
        {
            talkHUD.gameObject.SetActive(false);
            return false;
        }
        //UIâ ���� ���� on off

        if (player[0].TryGetComponent<PlayerInventory>(out PlayerInventory p))
        {
            p.Interaction = true;

            controller.Trading = p.Trading;
        }

        if (controller.Trading) return true;

        talkHUD.gameObject.SetActive(true);
        talkHUD.position = controller.transform.position + Vector3.up - Vector3.forward;
        Vector3 direction = player[0].GetComponent<BehaviourController>().myCam.forward;
        direction.y = 0.0f;
        talkHUD.rotation = Quaternion.LookRotation(direction);
        controller.InteractionHUDLabel.text = "�ŷ��ϱ�";

        if (time > 7.0f) { SoundManager.instance.PlayOneShotEffect((int)SoundList.NormalNpcTalk, controller.transform.position); time = 0f; } //�ӽù���
        return false;
    }

    
}
