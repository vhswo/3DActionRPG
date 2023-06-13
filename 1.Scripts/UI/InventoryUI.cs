using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum AskList
{
    None,
    IsDrop,
    IsSell,
    IsBuy,
}

public static class SelectInven
{
    public static InventoryUI SelectInvenUI = null; //���õ� �κ��丮
    public static GameObject ClickedSlotNum = null; //Ŭ���� ����
    public static InventoryUI EnterInvenUI = null; //���õ� �κ��丮
    public static GameObject EnterSlotNum = null;   // ������ ����
    public static bool canSlotPickUp = false;

    /// <summary>
    /// ����ƽ �ݵ�� �ؾ��� �ʱ�ȭ
    /// </summary>
    public static void ClearStaticSelect()
    {
       ClickedSlotNum = null;
       SelectInvenUI = null;
       EnterSlotNum = null;
       EnterInvenUI = null;
        canSlotPickUp = true;
    }
}

public abstract class InventoryUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Inventory inven;
    public Dictionary<GameObject, InventorySlot> slotsUI = new();

    GameObject copy;
    Image copyImage;

    //���âui
    public UIManager AlertCheck;

    private void Awake()
    {
        CreateSlotUIs();

        for (int i = 0; i < inven.slots.Length; i++)
        {
            inven.slots[i].parent = inven;
            inven.slots[i].SubScript += Obvserber;
        }

        if (copy == null)
        {
            copy = new();
            RectTransform rectTransform = copy.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(140, 140);
            copyImage = copy.AddComponent<Image>();
            copy.SetActive(false);
        }

    }

    public virtual void Obvserber(InventorySlot slot)
    {

    }


    public abstract void CreateSlotUIs();


    private bool CreateTempSlot(GameObject obj)
    {
        if (!slotsUI.ContainsKey(obj)) return false;
        if (slotsUI[obj].item.ClipName == string.Empty) return false;

        copy.transform.SetParent(transform.parent);
        copy.SetActive(true);

        copyImage.sprite = slotsUI[obj].item.icon;
        copyImage.raycastTarget = false;

        copy.name = "DragImage";

        return true;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        SelectInven.ClickedSlotNum = eventData.pointerEnter?.transform.parent.gameObject;
        if (SelectInven.ClickedSlotNum == null || !CreateTempSlot(SelectInven.ClickedSlotNum)) return;

        SelectInven.SelectInvenUI = this;
        SelectInven.canSlotPickUp = true;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (!SelectInven.canSlotPickUp || SelectInven.ClickedSlotNum == null || !copy.activeSelf) return;

        copy.transform.position = eventData.position;
    }


    public virtual void OnEndDrag(PointerEventData eventData)
    {
        //null üũ
        if (!SelectInven.canSlotPickUp || SelectInven.ClickedSlotNum == null || !copy.activeSelf) return;

        SelectInven.EnterInvenUI = eventData.pointerEnter?.GetComponentInParent<InventoryUI>();
        if (SelectInven.EnterInvenUI != null
            && SelectInven.EnterInvenUI.slotsUI.ContainsKey(eventData.pointerEnter?.transform.parent.gameObject)) { 
            SelectInven.EnterSlotNum = eventData.pointerEnter?.transform.parent.gameObject;
        }


        //event
        switch (SelectInven.SelectInvenUI.inven.type)
        {
            //���õ� �κ��丮�� player�̴�
            case InventoryType.Inventory:
                //���ü��ִ� Ÿ���� : ui = null, otherType, slot = null,true

                //�����κ�ui�� �ƴϴ�
                if (SelectInven.EnterInvenUI == null)
                {
                    AlertCheck.AskAlert(AskList.IsDrop); //�������� ���������
                    SelectInven.canSlotPickUp = false;
                }
                else
                {
                    //�������� ���Դ�
                    if(SelectInven.EnterInvenUI.inven.type == InventoryType.Merchant)
                    {
                        AlertCheck.AskAlert(AskList.IsSell);
                        SelectInven.canSlotPickUp = false;
                    }
                    else
                    {
                        //������ ������ ������ �ִٸ� ���� �̺�Ʈ
                        if(SelectInven.EnterSlotNum != null)
                            inven.CheckSwap(slotsUI[SelectInven.ClickedSlotNum], SelectInven.EnterInvenUI.slotsUI[SelectInven.EnterSlotNum]);

                        SelectInven.ClearStaticSelect();
                    }
                }
                break;
                //���õ� �κ�ui �� ����̴�
            case InventoryType.Equipment:
                if (SelectInven.EnterInvenUI != null &&SelectInven.EnterInvenUI.inven.type == InventoryType.Inventory && SelectInven.EnterSlotNum != null)
                {
                    inven.CheckSwap(slotsUI[SelectInven.ClickedSlotNum], SelectInven.EnterInvenUI.slotsUI[SelectInven.EnterSlotNum]);
                }
                SelectInven.ClearStaticSelect();
                break;
            case InventoryType.Merchant:
                IsBuyItem();
                SelectInven.canSlotPickUp = false;
                break;
            default:
                break;
        }

        copy.SetActive(false);
        copyImage.sprite = null;
        copyImage.raycastTarget = true;
    }

    /// <summary>
    /// SelectInven�� null �� �ƴϰ�, SelectInven type�� PlayerInventoryUI ���߸� �Ѵ�.
    /// </summary>
    public void IsBuyItem()
    {
        if (SelectInven.EnterInvenUI == null || SelectInven.EnterInvenUI.inven.type != InventoryType.Inventory)
        {
            SelectInven.ClearStaticSelect();
        }
        else
        {
            //��ðڽ��ϱ� ����
            AlertCheck.AskAlert(AskList.IsBuy);
        }
    }

}
