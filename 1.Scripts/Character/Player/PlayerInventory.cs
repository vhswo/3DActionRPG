using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Inventory playerInven;
    public Inventory playerEquip;
    public GameObject InventoryUI;
    public GameObject equipmentUI;

    readonly string InvenKey = "inventoryKey";
    readonly string EquipKey = "equipmentKey";
    readonly string PickupKey = "PickupKey";
    readonly string InteractionKey = "Interaction";
    readonly string Cancel = "Cancel";

    bool callInven, callEquip;
    public bool PickUp, Interaction;
    public bool Trading = false;

    public void InitPlayer(PlayerVariables variables)
    {
        playerInven.SetGold(variables.gold); 
    }

    private void Start()
    {
        callInven = false;
        callEquip = false;
        PickUp = false;
        Interaction = false;
        InventoryUI.SetActive(true);
        equipmentUI.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetButtonDown(InvenKey))
        {
            callInven = !callInven;
            InventoryUI.SetActive(callInven);
            SoundManager.instance.PlayEffectSound((int)SoundList.OnOffUI, transform.position);
        }
        else if(Input.GetButtonDown(EquipKey))
        {
            callEquip = !callEquip;
            equipmentUI.SetActive(callEquip);
            SoundManager.instance.PlayEffectSound((int)SoundList.OnOffUI, transform.position);
        }

        if(PickUp && Input.GetButtonDown(PickupKey))
        {
            //아이템 줍기
        }

        if(Interaction && Input.GetButtonDown(InteractionKey))
        {
            Trading = true;
            callInven = true;
            InventoryUI.SetActive(callInven);
            SoundManager.instance.PlayEffectSound((int)SoundList.OnOffUI, transform.position);
        }

        if(Trading) CheckMerchantNPC();

        if(Input.GetButtonDown(Cancel))
        {
            callInven = false;
            InventoryUI.SetActive(callInven);
            callEquip = false;
            equipmentUI.SetActive(callEquip);
            Trading = false;
            Interaction = false;
        }
    }

    public void CheckMerchantNPC()
    {
        Collider[] npc = Physics.OverlapSphere(transform.position, 3f, LayerMask.GetMask("MerchantNPC"));

        if (npc.Length <= 0)
        {
            callInven = false;
            InventoryUI.SetActive(callInven);
            Trading = false;
            Interaction = false;
            SoundManager.instance.PlayEffectSound((int)SoundList.OnOffUI, transform.position);

        }
    }

    public bool NowOn()
    {
        return callInven || callEquip;
    }
}
