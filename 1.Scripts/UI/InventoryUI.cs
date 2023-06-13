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
    public static InventoryUI SelectInvenUI = null; //선택된 인벤토리
    public static GameObject ClickedSlotNum = null; //클릭된 슬롯
    public static InventoryUI EnterInvenUI = null; //선택된 인벤토리
    public static GameObject EnterSlotNum = null;   // 도착한 슬롯
    public static bool canSlotPickUp = false;

    /// <summary>
    /// 스테틱 반드시 해야함 초기화
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

    //경고창ui
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
        //null 체크
        if (!SelectInven.canSlotPickUp || SelectInven.ClickedSlotNum == null || !copy.activeSelf) return;

        SelectInven.EnterInvenUI = eventData.pointerEnter?.GetComponentInParent<InventoryUI>();
        if (SelectInven.EnterInvenUI != null
            && SelectInven.EnterInvenUI.slotsUI.ContainsKey(eventData.pointerEnter?.transform.parent.gameObject)) { 
            SelectInven.EnterSlotNum = eventData.pointerEnter?.transform.parent.gameObject;
        }


        //event
        switch (SelectInven.SelectInvenUI.inven.type)
        {
            //선택된 인벤토리가 player이다
            case InventoryType.Inventory:
                //들어올수있는 타입은 : ui = null, otherType, slot = null,true

                //들어온인벤ui가 아니다
                if (SelectInven.EnterInvenUI == null)
                {
                    AlertCheck.AskAlert(AskList.IsDrop); //버릴건지 물어봐야함
                    SelectInven.canSlotPickUp = false;
                }
                else
                {
                    //상점으로 들어왔다
                    if(SelectInven.EnterInvenUI.inven.type == InventoryType.Merchant)
                    {
                        AlertCheck.AskAlert(AskList.IsSell);
                        SelectInven.canSlotPickUp = false;
                    }
                    else
                    {
                        //도착한 지점에 슬롯이 있다면 스왑 이벤트
                        if(SelectInven.EnterSlotNum != null)
                            inven.CheckSwap(slotsUI[SelectInven.ClickedSlotNum], SelectInven.EnterInvenUI.slotsUI[SelectInven.EnterSlotNum]);

                        SelectInven.ClearStaticSelect();
                    }
                }
                break;
                //선택된 인벤ui 가 장비이다
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
    /// SelectInven이 null 이 아니고, SelectInven type이 PlayerInventoryUI 여야만 한다.
    /// </summary>
    public void IsBuyItem()
    {
        if (SelectInven.EnterInvenUI == null || SelectInven.EnterInvenUI.inven.type != InventoryType.Inventory)
        {
            SelectInven.ClearStaticSelect();
        }
        else
        {
            //사시겠습니까 물음
            AlertCheck.AskAlert(AskList.IsBuy);
        }
    }

}
