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
    public GameObject go_Back_Image;

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

    const string defalt_EquipmentSlotBG_Path = "UI/Tooltip/TooltipBackground";
    const string defalt_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_alphaFront";
    const string common_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_white";
    const string rare_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_cyan";
    const string unique_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_pink";
    const string epic_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_yellow";
    const string set_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_green";

    private void Awake()
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

    /// <summary>
    /// ���Կ� �������� �߰��ϰ�, �����ۿ� ���� �ε��� ������ �־��ְ�, ���Կ� �̹����� �߰���.
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="count"></param>
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
        if (gameObject.CompareTag("Equipment"))
        {
            go_Back_Image.SetActive(true);
            slot_BG.sprite = Resources.Load<Sprite>(defalt_EquipmentSlotBG_Path);
        }
        else
        {
            slot_BG.sprite = Resources.Load<Sprite>(defalt_SlotBG_Path);
        }
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
            if (itemCount != 0)
            {
                if (gameObject.CompareTag("Inventory"))
                {
                    if (item.Type != 9 && item.Type != 10)
                        EquipItem(item);
                    else if (item.Type == 9)
                        UseItem(item);
                    else if (item.Type == 10)
                        Debug.Log("����� ��Ŭ�� - ȿ������");
                }
                else if (gameObject.CompareTag("Equipment"))
                {
                    UnEquipItem(item);
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
    /// �巹�� ���۽� �巹�� ������ ���Կ� �ִ� ������ �巹�� ���Կ� ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (itemCount != 0)
            {
                DragSlot.instance.dragSlot = this;
                DragSlot.instance.DragSetImage(itemImage);
                DragSlot.instance.transform.position = eventData.position;
            }
        }
    }

    /// <summary>
    /// �巹�� �ϴ� ���� ������ ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    /// <summary>
    /// �巹�� ������ �巹�� ���� ���İ� 0���� �ٲٰ� �����.<br/>
    /// �巹�� ���� ��ġ�� ����â�̳� �κ�â ���̸� ������ ���â �ҷ���.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (invenBase.gameObject.activeSelf)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(invenBase, DragSlot.instance.transform.position))
            {
                DragSlot.instance.SetColorAlpha(0);
                DragSlot.instance.dragSlot = null;
            }
            else
            {
                //if (DragSlot.instance.dragSlot != null)
                //    if (DragSlot.instance.dragSlot.itemCount <= 1)
                //    {
                //        DragSlot.instance.SetColorAlpha(0);
                //        StartCoroutine(inputNumber.DropItemCoruntine(1));
                //    }
                //    else
                //        inputNumber.Call();
            }
        }
    }

    /// <summary>
    /// ��ӵ� ������ ���������� ����̹ߵ���.<br/>
    /// �κ��丮 ������ ChangeSlot �Լ� �ߵ�. ��� ������ ������� �ߵ�.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            if (gameObject.CompareTag("Inventory"))
            {
                if (DragSlot.instance.dragSlot.CompareTag("Inventory"))
                {
                    ChangeSlot();
                }
                else if (DragSlot.instance.dragSlot.CompareTag("Equipment"))
                {
                    UnEquipItem(DragSlot.instance.dragSlot.item);
                }
            }
            else if (gameObject.CompareTag("Equipment"))
            {
                if (DragSlot.instance.dragSlot.CompareTag("Inventory") && DragSlot.instance.dragSlot.item.Type != 9 && DragSlot.instance.dragSlot.item.Type != 10)
                {
                    if (inven.Equipment_Slots[DragSlot.instance.dragSlot.item.Type].itemCount != 0)
                    {
                        DragSlot.instance.dragSlot.EquipItem(DragSlot.instance.dragSlot.item);
                    }
                    else
                    {
                        DragSlot.instance.dragSlot.EquipItem(DragSlot.instance.dragSlot.item);
                    }
                }
                else if (DragSlot.instance.dragSlot.CompareTag("Equipment"))
                {
                    // ����� ���� ���Ÿ���� ���� ���� ������ �����Ƿ� ����.
                    // ���Ŀ� ����� ���� �ۼ� �ʿ�.
                }
            }
        }

        if (itemCount != 0)
            tooltip.ShowTooltip(item);
    }

    /// <summary>
    /// ���� ��ü�Ҷ� ������ ������ ���� �ȱ�⸸ ����, ���� �ٲ������� �����ؼ� ó������.
    /// </summary>
    private void ChangeSlot()
    {
        if (itemCount != 0)
        {
            if (item.Name == DragSlot.instance.dragSlot.item.Name)
            {
                if (item.Type == 8 || item.Type == 9)
                {
                    SetSlotCount(DragSlot.instance.dragSlot.itemCount);

                    DragSlot.instance.dragSlot.ClearSlot();
                }
            }
            else
            {
                AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

                DragSlot.instance.dragSlot.AddItem(item, itemCount);
            }
        }
        else
        {
            AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

            DragSlot.instance.dragSlot.ClearSlot();
        }
    }

    /// <summary>
    /// ������ ������ �������.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemCount != 0)
            tooltip.ShowTooltip(item);
    }

    /// <summary>
    /// ���� ��������.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }

    /// <summary>
    /// �������� ���Կ��� ���ŵǰ�, ���ĭ�� ������ Ÿ�Կ� ���� ������.
    /// ���ĭ�� �̹� �������� ������� ��ȯ����.
    /// </summary>
    /// <param name="_item"></param>
    public void EquipItem(Item _item)
    {
        SetSlotCount(-1);

        #region ������ Ÿ�Կ����� ��񽽷Կ� �����ϴ� ����.
        switch (_item.Type)
        {
            // 0: �Ѽչ���, 1: �μչ���, 2: ���, 3: ����, 4: ��Ʈ, 5:�尩
            // 6: �Ź�, 7: �����, 8: ����, 9: �Һ�, 10: ���

            case 0:
                if (inven.WeaponSlot.itemCount == 0)
                {
                    inven.WeaponSlot.AddItem(_item);
                    inven.WeaponSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.WeaponSlot.UnEquipItem(inven.WeaponSlot.item, this);
                    inven.WeaponSlot.AddItem(_item);
                    inven.WeaponSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 1:
                if (inven.WeaponSlot.itemCount == 0)
                {
                    inven.WeaponSlot.AddItem(_item);
                    inven.WeaponSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.WeaponSlot.UnEquipItem(inven.WeaponSlot.item, this);
                    inven.WeaponSlot.AddItem(_item);
                    inven.WeaponSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 2:
                if (inven.HelmetSlot.itemCount == 0)
                {
                    inven.HelmetSlot.AddItem(_item);
                    inven.HelmetSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.HelmetSlot.UnEquipItem(inven.HelmetSlot.item, this);
                    inven.HelmetSlot.AddItem(_item);
                    inven.HelmetSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 3:
                if (inven.ArmorSlot.itemCount == 0)
                {
                    inven.ArmorSlot.AddItem(_item);
                    inven.ArmorSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.ArmorSlot.UnEquipItem(inven.ArmorSlot.item, this);
                    inven.ArmorSlot.AddItem(_item);
                    inven.ArmorSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 4:
                if (inven.BeltSlot.itemCount == 0)
                {
                    inven.BeltSlot.AddItem(_item);
                    inven.BeltSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.BeltSlot.UnEquipItem(inven.BeltSlot.item, this);
                    inven.BeltSlot.AddItem(_item);
                    inven.BeltSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 5:
                if (inven.GlovesSlot.itemCount == 0)
                {
                    inven.GlovesSlot.AddItem(_item);
                    inven.GlovesSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.GlovesSlot.UnEquipItem(inven.GlovesSlot.item, this);
                    inven.GlovesSlot.AddItem(_item);
                    inven.GlovesSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 6:
                if (inven.BootsSlot.itemCount == 0)
                {
                    inven.BootsSlot.AddItem(_item);
                    inven.BootsSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.BootsSlot.UnEquipItem(inven.BootsSlot.item, this);
                    inven.BootsSlot.AddItem(_item);
                    inven.BootsSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 7:
                if (inven.NecklaceSlot.itemCount == 0)
                {
                    inven.NecklaceSlot.AddItem(_item);
                    inven.NecklaceSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.NecklaceSlot.UnEquipItem(inven.NecklaceSlot.item, this);
                    inven.NecklaceSlot.AddItem(_item);
                    inven.NecklaceSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 8:
                if (inven.RingSlot.itemCount == 0)
                {
                    inven.RingSlot.AddItem(_item);
                    inven.RingSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.RingSlot.UnEquipItem(inven.RingSlot.item, this);
                    inven.RingSlot.AddItem(_item);
                    inven.RingSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
        }
        #endregion

        #region ������ ȿ���� �޾ƿͼ� ���� ���ݿ� �����ϴ� ����.
        List<int> keys = new List<int>();
        keys.AddRange(_item.itemEffect.ValueDic.Keys);

        for (int i = 0; i < keys.Count; i++)
        {
            // 0: ����, 1: ���� Hp, 2: ���� Mp, 3: �ִ� Hp ������, 4: �ִ� Hp %��, 5: �ִ� Mp ������, 6: �ִ� Mp %��
            // 7: ���ݷ� ������, 8: ���ݷ� %��, 9: ���� ������, 10: ���� %��, 11: �� ������, 12: �� %��
            // 13: ��ø ������, 14: ��ø %��, 15: ���� ������, 16: ���� %��, 17: ���ݽ� ������ ȸ�� ������, 18: ���ݽ� �������� %��ŭ ������ ȸ��

            switch (keys[i])
            {
                case 3:
                    player.ItemEffectMaxHp += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 4:
                    player.ItemEffectMaxHpMultiplier += (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 5:
                    player.ItemEffectMaxMp += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 6:
                    player.ItemEffectMaxMpMultiplier += (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 7:
                    player.ItemEffectAtk += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 8:
                    player.ItemEffectAtkMultiplier += (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 9:
                    player.ItemEffectDef += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 10:
                    player.ItemEffectDefMultiplier += (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 11:
                    player.ItemEffectStr += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 12:
                    player.ItemEffectStrMultiplier += (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 13:
                    player.ItemEffectDex += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 14:
                    player.ItemEffectDexMultiplier += (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 15:
                    player.ItemEffectInt += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 16:
                    player.ItemEffectIntMultiplier += (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 17:
                    player.ItemEffectLifeSteal += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 18:
                    player.ItemEffectLifeStealPercent += _item.itemEffect.ValueDic[keys[i]] * 0.01f;
                    break;
                default:
                    break;
            }
        }
        #endregion

        player.RefeshFinalStats();
    }

    /// <summary>
    /// �κ�â�� �������� �߰��ǰ�, ����â�� �ִ� ��񽽷��� ������ �ټ��� -1 (0�̵Ǽ� ClearSlot�Լ������)
    /// ������ �μ��� �ְԵǸ� �ش罽�Կ� ���Ե�.
    /// </summary>
    /// <param name="_item"></param>
    public void UnEquipItem(Item _item, Slot _slot = null)
    {
        SetSlotCount(-1);

        #region ������ ȿ�� ����(���ҽ�Ű��)�ϴ� �κ�
        List<int> keys = new List<int>();
        keys.AddRange(_item.itemEffect.ValueDic.Keys);

        for (int i = 0; i < keys.Count; i++)
        {
            // 0: ����, 1: ���� Hp, 2: ���� Mp, 3: �ִ� Hp ������, 4: �ִ� Hp %��, 5: �ִ� Mp ������, 6: �ִ� Mp %��
            // 7: ���ݷ� ������, 8: ���ݷ� %��, 9: ���� ������, 10: ���� %��, 11: �� ������, 12: �� %��
            // 13: ��ø ������, 14: ��ø %��, 15: ���� ������, 16: ���� %��, 17: ���ݽ� ������ ȸ�� ������, 18: ���ݽ� �������� %��ŭ ������ ȸ��

            switch (keys[i])
            {
                case 3:
                    player.ItemEffectMaxHp -= _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 4:
                    player.ItemEffectMaxHpMultiplier -= (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 5:
                    player.ItemEffectMaxMp -= _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 6:
                    player.ItemEffectMaxMpMultiplier -= (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 7:
                    player.ItemEffectAtk -= _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 8:
                    player.ItemEffectAtkMultiplier -= (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 9:
                    player.ItemEffectDef -= _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 10:
                    player.ItemEffectDefMultiplier -= (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 11:
                    player.ItemEffectStr -= _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 12:
                    player.ItemEffectStrMultiplier -= (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 13:
                    player.ItemEffectDex -= _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 14:
                    player.ItemEffectDexMultiplier -= (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 15:
                    player.ItemEffectInt -= _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 16:
                    player.ItemEffectIntMultiplier -= (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 17:
                    player.ItemEffectLifeSteal -= _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 18:
                    player.ItemEffectLifeStealPercent -= _item.itemEffect.ValueDic[keys[i]] * 0.01f;
                    break;
                default:
                    break;
            }
        }
        #endregion

        player.RefeshFinalStats();

        if (_slot != null)
            _slot.AddItem(_item);
        else
            inven.GetItem(_item);
    }

    public void UseItem(Item _item)
    {
        SetSlotCount(-1);

        List<int> keys = new List<int>();
        keys.AddRange(_item.itemEffect.ValueDic.Keys);

        for (int i = 0; i < keys.Count; i++)
        {
            // 0: ����, 1: ���� Hp, 2: ���� Mp, 3: �ִ� Hp ������, 4: �ִ� Hp %��, 5: �ִ� Mp ������, 6: �ִ� Mp %��
            // 7: ���ݷ� ������, 8: ���ݷ� %��, 9: ���� ������, 10: ���� %��, 11: �� ������, 12: �� %��
            // 13: ��ø ������, 14: ��ø %��, 15: ���� ������, 16: ���� %��, 17: ���ݽ� ������ ȸ�� ������, 18: ���ݽ� �������� %��ŭ ������ ȸ��

            switch (keys[i])
            {
                case 1:
                    player.curHp += _item.itemEffect.ValueDic[keys[i]];

                    if (player.curHp > player.finalMaxHp)
                        player.curHp = player.finalMaxHp;

                    break;
                case 2:
                    player.curMp += _item.itemEffect.ValueDic[keys[i]];
                    
                    if (player.curMp > player.finalMaxMp)
                        player.curMp = player.finalMaxMp;

                    break;
                default:
                    break;
            }
        }
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
}