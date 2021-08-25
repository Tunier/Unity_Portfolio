using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    GameObject inventory_Slot_Parent;
    [SerializeField]
    GameObject eqipment_Slot_Parent;

    [SerializeField]
    Text goldText;

    [SerializeField]
    PlayerInfo player;

    public List<Slot> inventory_Slots = new List<Slot>();
    List<Slot> Equipment_Slots = new List<Slot>();

    public List<Item> myItems = new List<Item>();

    public bool isFull = false;

    public Slot WeaponSlot { get; private set; }
    public Slot HelmetSlot { get; private set; }
    public Slot ArmorSlot { get; private set; }
    public Slot BeltSlot { get; private set; }
    public Slot BootsSlot { get; private set; }
    public Slot GlovesSlot { get; private set; }
    public Slot NecklaceSlot { get; private set; }
    public Slot RingSlot { get; private set; }


    private void Awake()
    {
        inventory_Slots.AddRange(inventory_Slot_Parent.GetComponentsInChildren<Slot>()); // ĥ�己���� �޾ƿ��� ������ ��Ȱ��ȭ �Ǹ� ���޾ƿ��⶧���� ��Ȱ��ȭ�� ���µ� ã�ƿü��ִ� ĥ�己���� �޾ƿ´�.
        Equipment_Slots.AddRange(eqipment_Slot_Parent.GetComponentsInChildren<Slot>());
    }

    private void Start()
    {
        //rect.localPosition = new Vector3(190, -12); // �����Ѹ� ��ġ �ʱ�ȭ.

        WeaponSlot = Equipment_Slots[0];
        HelmetSlot = Equipment_Slots[1];
        ArmorSlot = Equipment_Slots[2];
        BeltSlot = Equipment_Slots[3];
        BootsSlot = Equipment_Slots[4];
        GlovesSlot = Equipment_Slots[5];
        NecklaceSlot = Equipment_Slots[6];
        RingSlot = Equipment_Slots[7];

        #region �׽�Ʈ �ڵ�
        GetItem(ItemDatabase.instance.newItem("0000003"));
        GetItem(ItemDatabase.instance.newItem("0000002"));
        GetItem(ItemDatabase.instance.newItem("0000004"));
        GetItem(ItemDatabase.instance.newItem("0000005"));
        #endregion
    }

    private void Update()
    {
        DeleteNullSlot();

        for (int i = 0; i < inventory_Slots.Count; i++)
        {
            inventory_Slots[i].index = i;

            if (inventory_Slots[i].itemCount == 0)
            {
                isFull = false;
            }

            if (i == (inventory_Slots.Count - 1))
                if (inventory_Slots[i].itemCount != 0)
                    isFull = true;
        }

        goldText.text = string.Format("{0:N0} <color=#FFF900>G</color>", player.stats.Gold);
    }

    /// <summary>
    /// �κ��丮�� �������� �߰���Ŵ. ��ĥ�� �ִ� �������� ������.
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="count"></param>
    public void GetItem(Item _item, int count = 1)
    {
        if (_item.Type == 9 || _item.Type == 10) // �Һ����̳� ������϶�
        {
            foreach (Slot slot in inventory_Slots) // ��� ������
            {
                if (slot.itemCount != 0) // �������� �ִ� ���Կ�
                {
                    if (slot.item.Name == _item.Name) // ���� �̸��� ���� item�� ������
                    {
                        slot.SetSlotCount(count); // ������ ������Ŵ
                        return;
                    }
                }
            }
        }

        // ��� ������ �������
        foreach (Slot slot in inventory_Slots) // ��� ������
        {
            if (slot.itemCount == 0) // ��� �ִ� ���Կ�
            {
                slot.AddItem(_item, count); // �������߰�
                return;
            }
        }
    }

    public void SaveInven()
    {
        for (int i = 0; i < inventory_Slots.Count; i++)
        {
            if (inventory_Slots[i].itemCount != 0)
            {
                if (!myItems.Contains(inventory_Slots[i].item))
                {
                    myItems.Add(inventory_Slots[i].item);
                }
            }
        }

        string Jdata = JsonConvert.SerializeObject(myItems, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/Resources/Data/MyInvenItems.text", Jdata);

        Debug.Log("�κ� ���̺� �Ϸ�");
    }

    public void LoadInven(List<Item> itemList)
    {
        //for (int i = 0; i < itemList.Count; i++)
        //{
        //    if (itemList[i].Count != 0)
        //    {
        //        slots[itemList[i].SlotIndex].AddItem(itemList[i]);
        //    }
        //    else
        //    {
        //        Debug.Log(i + "��° ������ ����Ʈ�� �������� ����־ �ε����");
        //    }
        //}
    }

    /// <summary>
    /// ���ӵ��߿� ���԰����� �ٲ�� ���� ������ ����Ʈ���� ��������
    /// </summary>
    void DeleteNullSlot()
    {
        for (int i = 0; i < inventory_Slots.Count; ++i)
        {
            if (inventory_Slots[i] == null)
                inventory_Slots.RemoveAt(i);
        }
    }
}
