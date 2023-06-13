using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
/// <summary>
/// �κ��丮, 3�� ���ڿ� ��� �ȴ�
/// Icon �� ������ ǥ�� �ȴ�
/// </summary>

[Serializable]
public class InventorySlot
{
    [NonSerialized] public Inventory parent;
    [NonSerialized] public GameObject slotUI;
    [NonSerialized] public Action<InventorySlot> SubScript;

    public ItemType[] allowItemType = new ItemType[0];
    public ItemClip item = new();
    public int amount;

    public Image image = null;
    public Text Amount = null;
    public Text Name = null;
    public Text Price = null;

    public InventorySlot() { }
    public InventorySlot(ItemClip item,int amount) { this.item = item; this.amount = amount; }


    public bool AddItem(ItemClip item, int amount)
    {
        if(this.item.ClipName == string.Empty)
        {
            return UpdateSlot(item, amount);   
        }
        else if(this.item == item && item.Countable)
        {
            return UpdateSlot(item, this.amount + amount);
        }

        return false;
    }

    public bool UpdateSlot(ItemClip item, int amount)
    {
        this.item = item;
        this.amount = amount;
        SubScript?.Invoke(this);

        return true;
    }

    public bool DropItem(int num)
    {
        amount -= num;
        if(amount <= 0)
        {
            item = new();
            amount = 0;
        }

        SubScript?.Invoke(this);

        return true;
    }

    public bool IsSlotTypeSameInputType(ItemType type)
    {

        for (int i = 0; i < allowItemType.Length; i++)
        {
            if (type == allowItemType[i]) return true;
        }

        return false;
    }

    public bool CanSlotInItem(ItemClip otherItem)
    {

        if (allowItemType.Length <= 0 || otherItem.ClipName == string.Empty) 
        {
            //���â�� �ƴϰų� ���� ���� null �̸� true
            return true;
        }

        foreach(ItemType type in allowItemType)
        {
            if(type == otherItem.itemType)
            {
                return true;
            }
        }

        return false;
    }
}
