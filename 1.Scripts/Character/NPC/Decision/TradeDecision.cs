using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 플레이어가 NPC와 일정거리에 있는지 체크
/// 있으면 말을 걸수 있는 UI 띄우기
/// 말걸면 TradeBehaviour 상태로 넘어감
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
        //UI창 띄우기 상점 on off

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
        controller.InteractionHUDLabel.text = "거래하기";

        if (time > 7.0f) { SoundManager.instance.PlayOneShotEffect((int)SoundList.NormalNpcTalk, controller.transform.position); time = 0f; } //임시방편
        return false;
    }

    
}
