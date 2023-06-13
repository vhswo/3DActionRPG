using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NPCType
{
    None, // 일반 npc
    Merchant, //상인
    Guard, //경비원
}

public class NPCBehaviourController : MonoBehaviour
{
    [Header("플러그앤칩스")]
    public NPCState currentBehaviour;
    public NPCState remainBehaviour;

    [Header("플레이어변수")]
    public NPCVariables variables;
    public Animator ani;

    [Header("UI")]
    public Transform InteractionHUDByPlayer; //플레이와 상호작용하는 UI
    public Text InteractionHUDLabel;
    public InventoryUI InventoryUI;

    [Header("소유중인 아이템")]
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
