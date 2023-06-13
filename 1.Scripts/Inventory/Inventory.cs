using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum InventoryType
{
    Inventory,
    Equipment,
    QuickSlot,
    Box,
    Merchant,
}

[CreateAssetMenu(menuName ="Inventory",fileName ="playerInventory")]
public class Inventory : ScriptableObject
{
    public InventoryType type;
    public int MaximumBag = 16;
    public InventorySlot[] slots = new InventorySlot[16];
    public int gold { get; private set; }

    //슬롯이 늘어난다
    public void AddSlots(int num)
    {
        for(int i = 0; i < num; i++)
        {
            slots = ArrayHelper.helperAdd(new InventorySlot(), slots);
        }
    }

    //슬롯에 넣으면 슬롯에 구현되어있는 타입비교, 빈공간, 개수까지 확인한다
    public void AddItem(ItemClip item, int amount)
    {
        foreach(InventorySlot slot in slots)
        {
           if(slot.AddItem(item, amount)) return;
        }
    }
    public void DropItem(ItemClip item, int amount) {}


    /// <summary>
    /// slotA가 slotB로 향했다
    /// </summary>
    public void CheckSwap(InventorySlot slotA, InventorySlot slotB)
    {
        //같은 슬롯을 지목했다
        if (slotA == slotB) return;

        if(slotA.CanSlotInItem(slotB.item) && slotB.CanSlotInItem(slotA.item))
        {
            if(slotA.item == slotB.item && slotA.item.Countable)
            {
                //합치기
                slotB.AddItem(slotB.item, slotA.amount);
                slotA.UpdateSlot(new ItemClip(), 0);
            }
            else
            {
                InventorySlot tempSlot = new InventorySlot(slotB.item,slotB.amount);
                //스왑
                slotB.UpdateSlot(slotA.item, slotA.amount);
                slotA.UpdateSlot(tempSlot.item, tempSlot.amount);
            }
        }
    }

    public void SetGold(int gold)
    {
        this.gold = gold;
    }

    public bool InSlotInputItem(int num)
    {
        if (slots.Length <= num || slots.Length < 0) return false;

        if (slots[num].item.ClipName != string.Empty) return true;

        return false;
    }

}
