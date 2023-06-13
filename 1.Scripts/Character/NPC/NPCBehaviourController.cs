using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NPCType
{
    None, // �Ϲ� npc
    Merchant, //����
    Guard, //����
}

public class NPCBehaviourController : MonoBehaviour
{
    [Header("�÷��׾�Ĩ��")]
    public NPCState currentBehaviour;
    public NPCState remainBehaviour;

    [Header("�÷��̾��")]
    public NPCVariables variables;
    public Animator ani;

    [Header("UI")]
    public Transform InteractionHUDByPlayer; //�÷��̿� ��ȣ�ۿ��ϴ� UI
    public Text InteractionHUDLabel;
    public InventoryUI InventoryUI;

    [Header("�������� ������")]
    public ItemList[] haveItems = new ItemList[0];
    public bool Trading;

    public Inventory onlyOtherInven;

    private void Awake()
    {
        InteractionHUDByPlayer.gameObject.SetActive(false);
        Trading = false;

    }

    public void EqualCurrent(NPCState ChangeState)
    {
        if (remainBehaviour != ChangeState)
        {
            currentBehaviour = ChangeState;
        }
    }

    private void Update()
    {
        currentBehaviour.DoAction(this);
        currentBehaviour.CheckTransitions(this);
    }

    public void OnDrawGizmos()
    {
        if(variables.NearPlayerCheckGizmo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position + Vector3.up, variables.PlayerCheckRadius);
        }
    }

}
