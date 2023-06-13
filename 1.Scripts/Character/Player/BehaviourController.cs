using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public interface IDamageable
{
    public void TakeDamage(float damage, SoundList sounds, EffectList effect) {}
}
public enum PlayerMoveState
{
    None,
    Move,
    Jump,
    Crouch,
    Sprint,
}
public enum PlayerBehaviourState
{
    None,
    Attack,
    Take, // 채집
}

public class BehaviourController : MonoBehaviour, IDamageable
{
    public PlayerVariables variables;
    public ItemData items;

    public State currentBehaviour;
    public State remainBehaviour;

    public PlayerAnimation animator { get; private set; }
    public Rigidbody rigid;
    public Transform myTransform;
    public Transform myCam;
    public Cam myCamScript;

    public int IsGroundAni;
    private Vector3 ColExtents;
    public float SmoothTurn = 0.06f;
    public PlayerMoveState moveState = PlayerMoveState.None;
    public PlayerBehaviourState behaviourState = PlayerBehaviourState.None;

    public PlayerInventory playerInven;
    public GameObject SKillUI; // 임시
    public GameObject SkillUIParent;
    public LayerDetectiveBoxCol AttackRange;
    //public Skill[] skills; // 스킬 구현할때 쓰일 방식

    public float h { get; private set; }
    public float v { get; private set; }

    public readonly string hKey = "Horizontal";
    public readonly string vKey = "Vertical";
    public int hAni;
    public int vAni;

    private void Awake()
    {
        animator = GetComponent<PlayerAnimation>();
        rigid = GetComponent<Rigidbody>();
        myTransform = transform;
        myCamScript = myCam.GetComponent<Cam>();

        ColExtents = GetComponent<Collider>().bounds.extents;
        IsGroundAni = Animator.StringToHash("IsGround");
        hAni = Animator.StringToHash("H");
        vAni = Animator.StringToHash("V");

        playerInven = GetComponent<PlayerInventory>();
        playerInven.InitPlayer(variables);

    }

    public void MoveValue()
    {
        h = Input.GetAxisRaw(hKey);
        v = Input.GetAxisRaw(vKey);

        animator.myAnimator.SetFloat(hAni, h, 0.1f, Time.deltaTime);
        animator.myAnimator.SetFloat(vAni, v, 0.1f, Time.deltaTime);
    }

    public void EqualCurrent(State ChangeState)
    {
        if (remainBehaviour != ChangeState)
        {
            currentBehaviour = ChangeState;
        }
    }


    public bool IsGround()
    {
        Ray ray = new Ray(myTransform.position + Vector3.up * 2 * ColExtents.x, Vector3.down);

        return Physics.SphereCast(ray, ColExtents.x, ColExtents.x + 0.2f);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.black;
    //    Gizmos.DrawSphere(transform.position + Vector3.up * 2 * ColExtents.x, ColExtents.x + 0.2f);
    //}

    private void Update()
    {
        currentBehaviour.DoAction(this);
        currentBehaviour.CheckTransitions(this);
    }

    private void LateUpdate()
    {
        if (myCamScript)
            myCamScript.MovingCam(playerInven.NowOn());
    }


    //임시
    public void UseSkill(float time)
    {
        StartCoroutine(SkillCoolTime(time));
    }
    public IEnumerator SkillCoolTime(float time)
    {
        float start = 0f;
        SkillUIParent.SetActive(true);
        Image img =  SKillUI.GetComponent<Image>();
        float alpha = 0.5f;
        img.color = new Color(1, 1, 1, alpha);
        float velocity = 0.1f;
        while (start < time)
        {
            yield return new WaitForSeconds(0.1f);
            start += 0.1f;
            alpha = Mathf.SmoothDamp(alpha, 0.5f * ((time - start) / time), ref velocity, 0.1f);
            img.color = new Color(1, 1, 1, alpha);

        }
        SkillUIParent.SetActive(false);
    }

    //임시
    [NonSerialized] public float HP = 100;
    [NonSerialized] public float amor = 5;
    public void TakeDamage(float damage, SoundList sounds, EffectList effect)
    {
        HP -= (damage - amor); //방어력 계수 미정

        SoundManager.instance.PlayOneShotEffect((int)sounds, transform.position);
        EffectManager.instance.EffectOneShot((int)effect, transform.position + (Vector3.up * 1.5f));
        
    }

    //임시 어택 클립
    [NonSerialized] public float damage; //기본 데미지 * 계수
    [NonSerialized] public SoundList sounds; // 스키르이 이펙트소리
    [NonSerialized] public EffectList effectLists; //스킬의 이펙트

    public void SetAttack(float damage, SoundList sounds,EffectList effect)
    {
        this.damage = damage;
        this.sounds = sounds;
        this.effectLists = effect;
    }

    public void OnAttack()
    {
        Collider[] col = AttackRange.LayerDetective(LayerMask.GetMask("Enemy"));

        foreach(Collider collider in col)
        {
            collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(damage,sounds, effectLists);
        }
    }

}
