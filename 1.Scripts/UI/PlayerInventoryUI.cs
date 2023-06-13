using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerInventoryUI : InventoryUI
{
    [SerializeField] protected GameObject goldPrefab;
    [SerializeField] protected GameObject SlotPrefab;
    [SerializeField] protected Vector2 start;
    [SerializeField] protected Vector2 size;
    [SerializeField] protected Vector2 space;
    [Min(1), SerializeField] protected int Horizontal = 4;


    protected Image goldImage;
    protected Text gold;

    private int lastSlot = 0;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (inven.slots[i].image == null)
            {
                inven.slots[i].image = transform.GetChild(i).GetChild(0).GetComponent<Image>();
                inven.slots[i].image.color = new Color(1, 1, 1, 0);
            }
            if (inven.slots[i].Amount == null)
            {
                inven.slots[i].Amount = transform.GetChild(i).GetChild(1).GetComponent<Text>();
                inven.slots[i].Amount.text = string.Empty;
            }
        }
        goldPrefab.transform.SetParent(transform);
        goldPrefab.SetActive(true);

        if (goldImage == null)
        {
            goldImage = goldPrefab.transform.Find("Gold").GetChild(0).GetComponent<Image>();
            goldImage.sprite = Resources.Load<Sprite>("source/IconImage/goldImage");
        }
        if (gold == null)
        {
            gold = goldPrefab.transform.Find("Gold").GetChild(1).GetComponent<Text>();
            gold.text = inven.gold.ToString();
        }
        gameObject.SetActive(false);
    }

    public override void Obvserber(InventorySlot slot)
    {
        slot.image.sprite = slot.item.ClipName != "" ? slot.item.icon : null;
        slot.image.color = slot.item.ClipName != "" ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
        slot.Amount.text = slot.item.Countable ? (slot.amount <= 0 ? string.Empty : slot.amount.ToString()) : string.Empty;
    }

    public override void CreateSlotUIs()
    {
        SlotPrefab.SetActive(true);

        for (int i = lastSlot; i < inven.slots.Length; i++)
        {
            GameObject obj = Instantiate(SlotPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().anchoredPosition = CalculatePosition(i);

            inven.slots[i].slotUI = obj;
            slotsUI.Add(obj, inven.slots[i]);
        }

        lastSlot = inven.slots.Length;
        SlotPrefab.SetActive(false);
    }

    public Vector3 CalculatePosition(int i)
    {
        float x = start.x + ((space.x + size.x) * (i % Horizontal));
        float y = start.y + (-(space.y + size.y) * (i / Horizontal));

        return new Vector3(x, y, 0f);
    }

    public void AddGold(int gold)
    {
        inven.SetGold(inven.gold + gold);
        this.gold.text = inven.gold.ToString();
    }
}
