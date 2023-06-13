using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 포즈를 다양하게 바꾼다
/// </summary>
[CreateAssetMenu(menuName = ("PluggableAI/NPC/Behaviour/Idle"))]
public class ChangePoseBehaviour : NPCGenericBehaviour
{
    int rand;
    bool OneShotPose;
    public override void Setting(NPCBehaviourController controller)
    {
        OneShotPose = true;
        (controller.InventoryUI as NPCInventoryUI).ClearInven();
        controller.InventoryUI.gameObject.SetActive(false);
        controller.Trading = false;
        controller.InteractionHUDByPlayer.gameObject.SetActive(true);
        

    }

    public override void Action(NPCBehaviourController controller)
    {
        AnimatorStateInfo stateInfo = controller.ani.GetCurrentAnimatorStateInfo(0); // Base Layer
        if (stateInfo.IsName("Pose")) OneShotPose = true;

        //Idle 상태로 오래 있으면 enter의 에러가 발생한거
        //애니메이션이 0.6초가 지나면 다시한번 돌려준다.
        if (OneShotPose && stateInfo.IsName("Idle"))
        {
            rand = Random.Range(0, 4);
            controller.ani.SetFloat("Pose", rand);
            OneShotPose = false;
        }
    }
}
