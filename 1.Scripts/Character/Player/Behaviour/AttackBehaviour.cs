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
/// 기존 move를 덮어씌워서 새로운 동작을한다
/// 전투상태에선 걷기만 가능하다
/// 무브 -> 어택
/// 달리기 ->  어택
/// </summary>
[CreateAssetMenu(menuName = ("PluggableAI/Behaviour/Attack"))]
public class AttackBehaviour : GenericBehaviour
{ 
    //파워어택  변수
    float PowerAttackDelay = 5f;     // 쿨타임

    //Sword기본공격 변수
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



    //마우스 왼쪽 , 기본 공격
    public void LeftMouse(Animator ani, BehaviourController controller)
    {

        float AniPlayingTime = ani.GetCurrentAnimatorStateInfo(1).normalizedTime; // 가로사이에 1은 애니메이션 파라미터로 2번째에 있는 것을 가르킨다

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
                //공격력, 사운드, 이펙트
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

    //마우스 오른쪽 , 쿨타임 공격,스킬 하나 임시
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

        return true; //기본 공격 가능
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
