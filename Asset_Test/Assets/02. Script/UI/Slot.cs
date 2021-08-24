using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public int index = 0;
    public Item item;
    public Image itemImage;
    public Image slot_BG;

    public int itemCount = 0;

    [SerializeField]
    Text countText;

    ItemDatabase ItemDB;
    InputNumberUI inputNumber;
    Inventory inven;
    Shop shop;
    Tooltip tooltip;

    PlayerInfo player;

    [SerializeField]
    RectTransform invenBase;
    [SerializeField]
    RectTransform quickSlotBase;
    [SerializeField]
    RectTransform shopBase;

    const string common_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_white";
    const string rare_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_cyan";
    const string unique_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_pink";
    const string epic_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_yellow";
    const string set_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_green";

    private void Start()
    {
        ItemDB = FindObjectOfType<ItemDatabase>();
        inputNumber = FindObjectOfType<InputNumberUI>();
        inven = FindObjectOfType<Inventory>();
        shop = FindObjectOfType<Shop>();
        tooltip = FindObjectOfType<Tooltip>();
        player = FindObjectOfType<PlayerInfo>();
    }

    void SetColorAlpha(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    public void AddItem(Item _item, int count = 1)
    {
        item = _item;
        itemCount = count;
        item.SlotIndex = index;
        itemImage.sprite = Resources.Load<Sprite>(_item.ItemImagePath);
        SetColorAlpha(0.82f);

        if (item.Type == 9 || item.Type == 10)
        {
            countText.gameObject.SetActive(true);
            countText.text = itemCount.ToString();
        }
        else
        {
            countText.text = "0";
            countText.gameObject.SetActive(false);
        }

        switch (_item.Rarity)
        {
            case 0: // �Ϲ�
                slot_BG.sprite = Resources.Load<Sprite>(common_SlotBG_Path);
                break;
            case 1: // ����
                slot_BG.sprite = Resources.Load<Sprite>(rare_SlotBG_Path);
                break;
            case 2: // ����ũ
                slot_BG.sprite = Resources.Load<Sprite>(unique_SlotBG_Path);
                break;
            case 3: // ����
                slot_BG.sprite = Resources.Load<Sprite>(epic_SlotBG_Path);
                break;
            case 4: // ��Ʈ
                slot_BG.sprite = Resources.Load<Sprite>(set_SlotBG_Path);
                break;
        }
    }

    /// <summary>
    /// �ش罽���� ������ ī��Ʈ�� �μ��� ��ŭ ������. ������ ī��Ʈ�� 0�̵Ǹ� ClearSlot �Լ� �ߵ�.
    /// </summary>
    /// <param name="count"></param>
    public void SetSlotCount(int count)
    {
        itemCount += count;
        countText.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
            tooltip.HideTooltip();
        }
    }

    /// <summary>
    /// �ش�ĭ�� �����. (item = null, itemCount = 0, ��������Ʈ = null, ���İ� 0����)
    /// </summary>
    void ClearSlot()
    {
        item = null;
        itemImage.sprite = null;
        SetColorAlpha(0);
        itemCount = 0;

        countText.text = "0";
        countText.gameObject.SetActive(false);
    }

    /// <summary>
    /// ���� ��Ŭ���� ���Կ� �ִ� ������ Ÿ���� ���� �Ҹ�ǰ�̸� ������ ���,
    /// ���� �������ͽ�â�� �ش� ���Ÿ��ĭ�� ����.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (gameObject.CompareTag("Inventory"))
            {
                switch (item.Type)
                {
                    case 0:
                        Item _item = item;
                        SetSlotCount(-1);
                        inven.eqipment_Slots[0].AddItem(_item);
                        break;
                    // ���Ÿ�Կ� ���� ����ġ ĳ�̽��� �����ϴ� ����.
                    default:
                        break;
                }
            }
        }


        //if (eventData.button == PointerEventData.InputButton.Right)
        //{
        //    if (shopBase.gameObject.activeSelf)
        //    {
        //        if (transform.CompareTag("INVENTORY")) // �κ��丮�� �ִ� ����.
        //            if (item != null)
        //            {
        //                SellItem(item);
        //            }
        //    }
        //    else
        //    {
        //        if (transform.CompareTag("INVENTORY")) // �κ��丮�� �ִ� ����.
        //            if (item != null)
        //            {
        //                if (item.Type == Item.ItemType.Used)
        //                {
        //                    ItemDB.UseItem(item);
        //                    SetSlotCount(-1);
        //                }
        //                else if (item.Type == Item.ItemType.Equipment)
        //                {
        //                    Item _item = item;
        //                    SetSlotCount(-1);
        //                    ItemDB.EquipItem(_item);
        //                }
        //            }

        //        if (transform.CompareTag("STATUS")) // �������ͽ� â�� �ִ� ����.
        //            if (item != null)
        //            {
        //                UnEquipItem(item);
        //            }

        //        if (transform.CompareTag("QUICKSLOT"))
        //        {
        //            if (item != null)
        //            {
        //                if (item.itemType == Item.ItemType.Equipment)
        //                {
        //                    if (item.EquipmentType == "Weapon")
        //                    {
        //                        if (status.weaponSlot.item == null)
        //                        {
        //                            EquipItem(item);
        //                            return;
        //                        }
        //                        else
        //                            return;
        //                    }
        //                }

        //                ItemDB.UseItem(item);

        //                if (item.itemType == Item.ItemType.Used)
        //                    SetSlotCount(-1);
        //            }
        //        }
        //    }
        //}
        //else if (eventData.button == PointerEventData.InputButton.Left)
        //{
        //    if (shopBase.gameObject.activeSelf)
        //    {
        //        if (shop.isSelling)
        //        {
        //            if (transform.CompareTag("INVENTORY")) // �κ��丮�� �ִ� ����.
        //            {
        //                if (item != null)
        //                {
        //                    SellItem(item);
        //                }
        //            }
        //        }
        //    }
        //}

        //if (item != null)
        //{
        //    ItemDB.ShowToolTip(item);
        //    ItemDB.SetItemCostText(item.sellCost);
        //}
    }

    /// <summary>
    /// �������� ���ŵǰ�, ����â�� �������� ������ �°� ������.
    /// </summary>
    /// <param name="_item"></param>
    public void EquipItem(Item _item)
    {
        //ItemDB.EquipItem(_item);

        //SetSlotCount(-1);
    }

    /// <summary>
    /// �κ�â�� �������� �����ǰ�, ����â�� �ִ� ��񽽷��� ������ �ټ��� -1 (0�̵Ǽ� ClearSlot�Լ������)
    /// </summary>
    /// <param name="_item"></param>
    public void UnEquipItem(Item _item)
    {
        //inven.GetItem(_item);

        //ItemDB.UnEquipItem(_item);

        //SetSlotCount(-1);
    }

    /// <summary>
    /// ������ ����, ����ĭ�� �������� �߰��ǰ�, �÷��̾��� ��尡 �������� �ǸŰ���ŭ ������.
    /// </summary>
    /// <param name="_item"></param>
    void SellItem(Item _item)
    {
        ClearSlot();
        shop.GetItem(_item);
        player.stats.Gold += _item.SellCost;
    }

    // �巹�� ���۽� �巹�� ������ ���Կ� �ִ� ������ �巹�� ���Կ� ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                DragSlot.instance.dragSlot = this;
                DragSlot.instance.DragSetImage(itemImage);
                DragSlot.instance.transform.position = eventData.position;
            }
        }
    }

    // �巹�� �ϴ� ���� ������ ����
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    // �巹�� ������ �巹�� ���� ���İ� 0���� �ٲٰ� �����
    // �巹�� ���� ��ġ�� ����â�̳� �κ�â ���̸� ������ ���â �ҷ���.
    public void OnEndDrag(PointerEventData eventData)
    {
        //// Ȱ��ȭ�� â�� ���ǰ� if�� �־���.
        //if (invenBase.gameObject.activeSelf && statusBase.gameObject.activeSelf && status.gameObject.activeSelf)
        //{
        //    if (RectTransformUtility.RectangleContainsScreenPoint(invenBase, DragSlot.instance.transform.position)
        //       || RectTransformUtility.RectangleContainsScreenPoint(statusBase, DragSlot.instance.transform.position)
        //       || RectTransformUtility.RectangleContainsScreenPoint(shopBase, DragSlot.instance.transform.position)
        //       || RectTransformUtility.RectangleContainsScreenPoint(quickSlotBase, DragSlot.instance.transform.position))
        //    {
        //        DragSlot.instance.SetColorAlpha(0);
        //        DragSlot.instance.dragSlot = null;
        //    }
        //    else
        //    {
        //        if (DragSlot.instance.dragSlot != null)
        //            if (DragSlot.instance.dragSlot.itemCount <= 1)
        //            {
        //                DragSlot.instance.SetColorAlpha(0);
        //                StartCoroutine(inputNumber.DropItemCoruntine(1));
        //            }
        //            else
        //                inputNumber.Call();
        //    }
        //}
        //else if (invenBase.gameObject.activeSelf && statusBase.gameObject.activeSelf)
        //{
        //    if (RectTransformUtility.RectangleContainsScreenPoint(invenBase, DragSlot.instance.transform.position)
        //        || RectTransformUtility.RectangleContainsScreenPoint(statusBase, DragSlot.instance.transform.position)
        //        || RectTransformUtility.RectangleContainsScreenPoint(quickSlotBase, DragSlot.instance.transform.position))
        //    {
        //        DragSlot.instance.SetColorAlpha(0);
        //        DragSlot.instance.dragSlot = null;
        //    }
        //    else
        //    {
        //        if (DragSlot.instance.dragSlot != null)
        //            if (DragSlot.instance.dragSlot.itemCount <= 1)
        //            {
        //                DragSlot.instance.SetColorAlpha(0);
        //                StartCoroutine(inputNumber.DropItemCoruntine(1));
        //            }
        //            else
        //                inputNumber.Call();
        //    }
        //}
        //else if (invenBase.gameObject.activeSelf && shopBase.gameObject.activeSelf)
        //{
        //    if (RectTransformUtility.RectangleContainsScreenPoint(invenBase, DragSlot.instance.transform.position)
        //        || RectTransformUtility.RectangleContainsScreenPoint(shopBase, DragSlot.instance.transform.position)
        //        || RectTransformUtility.RectangleContainsScreenPoint(quickSlotBase, DragSlot.instance.transform.position))
        //    {
        //        DragSlot.instance.SetColorAlpha(0);
        //        DragSlot.instance.dragSlot = null;
        //    }
        //    else
        //    {
        //        if (DragSlot.instance.dragSlot != null)
        //            if (DragSlot.instance.dragSlot.itemCount <= 1)
        //            {
        //                DragSlot.instance.SetColorAlpha(0);
        //                StartCoroutine(inputNumber.DropItemCoruntine(1));
        //            }
        //            else
        //                inputNumber.Call();
        //    }
        //}
        //else if (invenBase.gameObject.activeSelf)
        //{
        //    if (RectTransformUtility.RectangleContainsScreenPoint(invenBase, DragSlot.instance.transform.position)
        //        || RectTransformUtility.RectangleContainsScreenPoint(quickSlotBase, DragSlot.instance.transform.position))
        //    {
        //        DragSlot.instance.SetColorAlpha(0);
        //        DragSlot.instance.dragSlot = null;
        //    }
        //    else
        //    {
        //        if (DragSlot.instance.dragSlot != null)
        //            if (DragSlot.instance.dragSlot.itemCount <= 1)
        //            {
        //                DragSlot.instance.SetColorAlpha(0);
        //                StartCoroutine(inputNumber.DropItemCoruntine(1));
        //            }
        //            else
        //                inputNumber.Call();
        //    }
        //}
        //else if (statusBase.gameObject.activeSelf)
        //{
        //    if (RectTransformUtility.RectangleContainsScreenPoint(statusBase, DragSlot.instance.transform.position)
        //        || RectTransformUtility.RectangleContainsScreenPoint(quickSlotBase, DragSlot.instance.transform.position))
        //    {
        //        DragSlot.instance.SetColorAlpha(0);
        //        DragSlot.instance.dragSlot = null;
        //    }
        //    else
        //    {
        //        if (DragSlot.instance.dragSlot != null)
        //            if (DragSlot.instance.dragSlot.itemCount <= 1)
        //            {
        //                DragSlot.instance.SetColorAlpha(0);
        //                StartCoroutine(inputNumber.DropItemCoruntine(1));
        //            }
        //            else
        //                inputNumber.Call();
        //    }
        //}
        //else
        //{
        //    if (RectTransformUtility.RectangleContainsScreenPoint(quickSlotBase, DragSlot.instance.transform.position))
        //    {
        //        DragSlot.instance.SetColorAlpha(0);
        //        DragSlot.instance.dragSlot = null;
        //    }
        //    else
        //    {
        //        if (DragSlot.instance.dragSlot != null)
        //            if (DragSlot.instance.dragSlot.itemCount <= 1)
        //            {
        //                DragSlot.instance.SetColorAlpha(0);
        //                StartCoroutine(inputNumber.DropItemCoruntine(1));
        //            }
        //            else
        //                inputNumber.Call();
        //    }
        //}
    }

    public void OnDrop(PointerEventData eventData)
    {
        //    if (transform.parent.CompareTag("INVENTORY"))
        //        if (DragSlot.instance.dragSlot != null)
        //            if (DragSlot.instance.dragSlot.transform.parent.CompareTag("STATUS"))
        //            {   // ����â���� �κ�â���� �Ѿ�ð�� ���� ������ ���� ��ü���ְ�, �ƴϸ� ������ ȹ��.
        //                Item _item = DragSlot.instance.dragSlot.item;

        //                ItemDB.UnEquipItem(DragSlot.instance.dragSlot.item);
        //                DragSlot.instance.dragSlot.SetSlotCount(-1);

        //                if (item != null)
        //                {
        //                    if (item.EquipmentType == _item.EquipmentType)
        //                    {
        //                        EquipItem(item);
        //                    }
        //                }

        //                inven.GetItem(_item);
        //            }
        //            else // �κ� �������� ��ȯ����.
        //            {
        //                ChangeSlot();
        //            }

        //    if (transform.parent.CompareTag("STATUS"))
        //        if (DragSlot.instance.dragSlot != null)
        //            if (DragSlot.instance.dragSlot.transform.parent.CompareTag("INVENTORY"))
        //            {   // �κ�â���� ����â���� �Ѿ��� ���ĭ�� ��������� �´� ���Ÿ��ĭ�� ����, ��� ������ ���� ���Ÿ���ϰ�� ��ü.
        //                if (DragSlot.instance.dragSlot.item.itemType == Item.ItemType.Equipment)
        //                {
        //                    Item _item = item;

        //                    if (item != null)
        //                    {
        //                        if (item.EquipmentType == DragSlot.instance.dragSlot.item.EquipmentType)
        //                        {
        //                            ItemDB.UnEquipItem(_item);
        //                            SetSlotCount(-1);
        //                            ItemDB.EquipItem(DragSlot.instance.dragSlot.item);
        //                            DragSlot.instance.dragSlot.SetSlotCount(-1);
        //                            inven.GetItem(_item);
        //                        }
        //                    }
        //                    else if (item == null)
        //                    {
        //                        ItemDB.EquipItem(DragSlot.instance.dragSlot.item);
        //                        DragSlot.instance.dragSlot.SetSlotCount(-1);
        //                    }
        //                }
        //            }

        //    if (transform.parent.CompareTag("QUICKSLOT"))
        //        if (DragSlot.instance.dragSlot != null)
        //            ChangeSlot();

        //    if (item != null)
        //    {
        //        ItemDB.ShowToolTip(item);
        //        ItemDB.SetItemCostText(item.sellCost);
        //    }
        //}

        //private void ChangeSlot()
        //{
        //    Item _item = item;
        //    int _itemCount = itemCount;

        //    if (_item != null)
        //    {
        //        if (_item.itemName == DragSlot.instance.dragSlot.item.itemName)
        //        {
        //            if (_item.itemType == Item.ItemType.Used)
        //            {
        //                SetSlotCount(DragSlot.instance.dragSlot.itemCount);

        //                DragSlot.instance.dragSlot.ClearSlot();
        //            }
        //        }
        //        else
        //        {
        //            AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        //            DragSlot.instance.dragSlot.AddItem(_item, _itemCount);
        //        }
        //    }
        //    else
        //    {
        //        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        //        DragSlot.instance.dragSlot.ClearSlot();
        //    }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemCount != 0)
            tooltip.ShowTooltip(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
}
