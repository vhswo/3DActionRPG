using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class NPCInventoryUI : InventoryUI
{
    [SerializeField] protected GameObject SlotPrefab;
    [SerializeField] protected Vector2 start;
    [SerializeField] protected Vector2 size;
    [SerializeField] protected Vector2 space;
    [Min(1), SerializeField] protected int Horizontal = 1;

    [SerializeField] RectTransform CreateFolder;

    int lastSlot = 0;
    private void Start()
    {
        lastSlot = 0;
        SetSlot(0);
        SlotPrefab.SetActive(false);
        gameObject.SetActive(false);
    }

    public override void CreateSlotUIs()
    {
        int max = inven.slots.Length;
        for (int i = 0; i < max; i++)
        {
            GameObject obj = Instantiate(SlotPrefab, Vector3.zero, Quaternion.identity, CreateFolder);
            obj.GetComponent<RectTransform>().anchoredPosition = CalculatePosition(i);
            inven.slots[i].slotUI = obj;
            slotsUI.Add(obj, inven.slots[i]);
            obj.SetActive(false);
        }
    }
    public Vector3 CalculatePosition(int i)
    {
        float x = start.x + ((space.x + size.x) * (i % Horizontal));
        float y = start.y + (-(space.y + size.y) * (i / Horizontal));

        return new Vector3(x, y, 0f);
    }
    public override void Obvserber(InventorySlot slot)
    {
        foreach (GameObject slots in slotsUI.Keys)
        {
            if (slotsUI[slots] == slot)
            {
                if (!slots.activeSelf)
                {
                    slots.SetActive(true);
                    break;
                }
            }
        }
        slot.image.sprite = slot.item.ClipName != "" ? slot.item.icon : null;
        slot.image.color = slot.item.ClipName != "" ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
        slot.Name.text = slot.item.ClipName != "" ? slot.item.ClipName : null;
        slot.Price.text = slot.item.ClipName != "" ? slot.item.buy.ToString() : null;
    }

    public void ClearInven()
    {
        foreach (GameObject slots in slotsUI.Keys)
        {
            InventorySlot slot = slotsUI[slots];
            slot.DropItem(1);
            slots.SetActive(false);
        }
    }

    public void SetSlot(int num)
    {
        //슬롯추가를 할거면 따로 만들것
        for (; lastSlot < CreateFolder.childCount+ num; lastSlot++)
        {
            if (inven.slots[lastSlot].image == null)
            {
                inven.slots[lastSlot].image = CreateFolder.GetChild(lastSlot).GetChild(0).GetComponent<Image>();
                inven.slots[lastSlot].image.color = new Color(1, 1, 1, 0);
            }

            if (inven.slots[lastSlot].Name == null)
            {
                inven.slots[lastSlot].Name = CreateFolder.GetChild(lastSlot).GetChild(1).GetComponent<Text>();
                inven.slots[lastSlot].Name.text = string.Empty;
            }

            if (inven.slots[lastSlot].Price == null)
            {
                inven.slots[lastSlot].Price = CreateFolder.GetChild(lastSlot).GetChild(2).GetComponent<Text>();
                inven.slots[lastSlot].Price.text = string.Empty;
            }
        }
    }


}

/*
 * npc 대화후 상점 켜고 슬롯 이동 등 구현 하기 팔고 사기
 */
