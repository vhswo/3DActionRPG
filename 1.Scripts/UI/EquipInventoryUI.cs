using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipInventoryUI : InventoryUI
{
    [SerializeField] protected GameObject[] SlotPrefab;
    public Transform Weapons;
    public string exRightWeapon, exLeftWeapon;

    private void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (inven.slots[i].image == null)
            {
                inven.slots[i].image = transform.GetChild(i).GetChild(0).GetComponent<Image>();
                inven.slots[i].image.color = new Color(1, 1, 1, 0);
            }
        }

        gameObject.SetActive(false);
    }

    public override void CreateSlotUIs()
    {
        for (int i =0; i < inven.slots.Length; i++)
        {
            inven.slots[i].slotUI = SlotPrefab[i];
            slotsUI.Add(SlotPrefab[i], inven.slots[i]);
        }
    }

    public override void Obvserber(InventorySlot slot)
    {
        slot.image.sprite = slot.item.ClipName != "" ? slot.item.icon : null;
        slot.image.color = slot.item.ClipName != "" ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);

        switch (slot.allowItemType[0])
        {
            case ItemType.RightWeapon:
                FindWeapon(slot.item.ClipName != "" ? slot.item.ClipName : exRightWeapon);
                exRightWeapon = slot.item.ClipName;
                break;
            case ItemType.LeftWeapon:
                FindWeapon(slot.item.ClipName != "" ? slot.item.ClipName : exLeftWeapon);
                exLeftWeapon = slot.item.ClipName;
                break;
            default:
                break;
        }
    }

    public void FindWeapon(string name)
    {
        Transform find = Weapons?.Find(name);

        if (find == null) return;

        find.gameObject.SetActive(!find.gameObject.activeSelf);

    }

}
