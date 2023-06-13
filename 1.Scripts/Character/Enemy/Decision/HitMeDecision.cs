using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("PluggableAI/Enemy/Decision/HitMe"))]
public class HitMeDecision : EnemyDecision
{
    public override void Setting(EnemyBehaviourController controller)
    {

    }

    public override bool Decide(EnemyBehaviourController controller)
    {
        return HitDecteve(controller) || controller.ani.GetCurrentAnimatorStateInfo(0).IsName("hitMe"); // 맞거나 맞는 중이면 true
    }

    public bool HitDecteve(EnemyBehaviourController controller)
    {
        Collider[] attack = Physics.OverlapSphere(controller.transform.position, 2f, LayerMask.GetMask("PlayerAttack"));

        if (attack.Length <= 0) return false;

        controller.ani.SetTrigger("hitMe");
        return true;
    }
}
