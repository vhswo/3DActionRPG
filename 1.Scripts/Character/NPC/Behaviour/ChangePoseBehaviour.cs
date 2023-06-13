using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� �پ��ϰ� �ٲ۴�
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

        //Idle ���·� ���� ������ enter�� ������ �߻��Ѱ�
        //�ִϸ��̼��� 0.6�ʰ� ������ �ٽ��ѹ� �����ش�.
        if (OneShotPose && stateInfo.IsName("Idle"))
        {
            rand = Random.Range(0, 4);
            controller.ani.SetFloat("Pose", rand);
            OneShotPose = false;
        }
    }
}
