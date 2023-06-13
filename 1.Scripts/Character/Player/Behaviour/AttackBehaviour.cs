using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("PluggableAI/Skill/Attack"))]
public class  Skill : ScriptableObject
{
    private string Name;
    private int AttackHashCode;

    Skill() { }
    Skill(string name)
    {
        Name = name;
        AttackHashCode = Animator.StringToHash($"AttackMode.{Name}");
    }

    public int GetAttackAniCode(string index)
    {
        if (index == Name)
            return AttackHashCode;
        else
            return -1;
    }
}

/// <summary>
/// ���� move�� ������� ���ο� �������Ѵ�
/// �������¿��� �ȱ⸸ �����ϴ�
/// ���� -> ����
/// �޸��� ->  ����
/// </summary>
[CreateAssetMenu(menuName = ("PluggableAI/Behaviour/Attack"))]
public class AttackBehaviour : GenericBehaviour
{ 
    //�Ŀ�����  ����
    float PowerAttackDelay = 5f;     // ��Ÿ��

    //Sword�⺻���� ����
    int NowCombo;
    string swordAttackAni;



    public override void Setting(BehaviourController controller)
     {
        controller.animator.myAnimator.SetBool("Sprint", false);
        controller.animator.myAnimator.SetBool("Cover", true);
        controller.animator.myAnimator.SetBool("AttackMode", true);

        NowCombo = 0;
        swordAttackAni = "BasicAttack";
    }

    public override void Action(BehaviourController controller)
    {
        BasicSwordAttack(controller);
        Move(controller);

    }
    
     public void Move(BehaviourController controller)
     {
        controller.MoveValue();
     }



    //���콺 ���� , �⺻ ����
    public void LeftMouse(Animator ani, BehaviourController controller)
    {

        float AniPlayingTime = ani.GetCurrentAnimatorStateInfo(1).normalizedTime; // ���λ��̿� 1�� �ִϸ��̼� �Ķ���ͷ� 2��°�� �ִ� ���� ����Ų��

        if (ani.GetCurrentAnimatorStateInfo(1).IsName("OneAttack") && AniPlayingTime > 0.95f)
        {
            ani.SetInteger(swordAttackAni, -1);
            NowCombo = 0;
        }

        if (ani.GetCurrentAnimatorStateInfo(1).IsName("TwoAttack") && AniPlayingTime > 0.95f)
        {
            ani.SetInteger(swordAttackAni, -1);
            NowCombo = 0;
        }

        if (ani.GetCurrentAnimatorStateInfo(1).IsName("ThreeAttack") && AniPlayingTime > 0.95f)
        {
            ani.SetInteger(swordAttackAni, -1);
            NowCombo = 0;
        }


        if (Input.GetMouseButtonDown(0))
        {
            if (NowCombo == 0 && !ani.GetCurrentAnimatorStateInfo(1).IsName("ThreeAttack"))
            {
                ani.SetInteger(swordAttackAni, 0);
                //���ݷ�, ����, ����Ʈ
                SoundManager.instance.PlayEffectSound((int)SoundList.MissSword, controller.transform.position);
                controller.SetAttack(0,SoundList.SwordBasicAttack,EffectList.SwordBasicAttack);
                NowCombo++;
            }

            if (NowCombo == 1 && ani.GetCurrentAnimatorStateInfo(1).IsName("OneAttack") && AniPlayingTime > 0.7f)
            {
                ani.SetInteger(swordAttackAni, 1);
                SoundManager.instance.PlayEffectSound((int)SoundList.MissSword, controller.transform.position);
                controller.SetAttack(0, SoundList.SwordBasicAttack, EffectList.SwordBasicAttack);
                NowCombo++;
            }

            if (NowCombo == 2 && ani.GetCurrentAnimatorStateInfo(1).IsName("TwoAttack") && AniPlayingTime > 0.7f)
            {
                ani.SetInteger(swordAttackAni, 2);
                SoundManager.instance.PlayEffectSound((int)SoundList.MissSword, controller.transform.position);
                controller.SetAttack(0, SoundList.SwordBasicAttack, EffectList.SwordBasicAttack);
                NowCombo = 0;
            }
        }
    }

    //���콺 ������ , ��Ÿ�� ����,��ų �ϳ� �ӽ�
    public bool RightMouse(Animator ani,BehaviourController controller)
    {
        float AniPlayingTime = ani.GetCurrentAnimatorStateInfo(1).normalizedTime;

        if (Input.GetMouseButtonDown(1) && controller.variables.CoolTimeToRightMouse <= Time.time)
        {
            ani.SetTrigger("PowerAttack");
            controller.variables.CoolTimeToRightMouse = Time.time + PowerAttackDelay;
            controller.SetAttack(0, SoundList.SwordBasicAttack, EffectList.SwordBasicAttack);
            controller.UseSkill(5f);
        }

        return true; //�⺻ ���� ����
    }

    public void BasicSwordAttack(BehaviourController controller)
    {
        if (controller.playerInven.NowOn()) return;

        Animator ani = controller.animator.myAnimator;

        if (RightMouse(ani,controller))
        {
            LeftMouse(ani, controller);
        }
    }

}
