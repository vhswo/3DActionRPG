using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EnemyType
{
    Friendly, //��ȣ��
    Warlike,  //ȣ����
}

public class EnemyGenericBehaviour : ScriptableObject
{
    public virtual void Setting(EnemyBehaviourController controller)
    {

    }

    public virtual void Action(EnemyBehaviourController controller)
    {

    }
}

public class EnemyBehaviourController : MonoBehaviour,IDamageable
{
    [Header("�÷��׾�Ĩ��")]
    public EnemyState currentBehaviour;
    public EnemyState remainBehaviour;

    [Header("�÷��̾��")]
    public Animator ani;


    void Start()
    {
        ani = GetComponent<Animator>();
    }

    public void EqualCurrent(EnemyState ChangeState)
    {
        if (remainBehaviour != ChangeState)
        {
            currentBehaviour = ChangeState;
        }
    }


    //�ӽ�
    [NonSerialized] public float HP = 100;
    [NonSerialized] public float amor = 5;
    public void TakeDamage(float damage, SoundList sounds, EffectList effect)
    {
        HP -= (damage - amor); //���� ��� ����

        SoundManager.instance.PlayOneShotEffect((int)sounds, transform.position);
        EffectManager.instance.EffectOneShot((int)effect, transform.position + (Vector3.up * 1.5f));
        ani.SetTrigger("hitMe");

    }


    void Update()
    {
        currentBehaviour.DoAction(this);
        currentBehaviour.CheckTransitions(this);
    }


}
