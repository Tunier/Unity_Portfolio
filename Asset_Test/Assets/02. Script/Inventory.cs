using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    GameObject slotsParent;

    [SerializeField]
    Text goldText;

    [SerializeField]
    PlayerInfo player;

    RectTransform rect;

    public List<Slot> slots;

    public List<Item> myItems;

    public bool isFull = false;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        
        slots = new List<Slot>();

        slots.AddRange(slotsParent.GetComponentsInChildren<Slot>()); // ĥ�己���� �޾ƿ��� ������ ��Ȱ��ȭ �Ǹ� ���޾ƿ��⶧���� ��Ȱ��ȭ�� ���µ� ã�ƿü��ִ� ĥ�己���� �޾ƿ´�.
    }

    private void Start()
    {
        rect.localPosition = new Vector3(190, -12); // �����Ѹ� ��ġ �ʱ�ȭ.
    }

    private void Update()
    {
        DeleteNullSlot();

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].index = i;

            if (slots[i].item == null)
            {
                isFull = false;
            }

            if (i == (slots.Count - 1))
                if (slots[i].item != null)
                    isFull = true;
        }

        goldText.text = string.Format("{0:N0}", player.stats.Gold);
    }

    /// <summary>
    /// �κ��丮�� �������� �߰���Ŵ. ��ĥ�� �ִ� �������� ������.
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="count"></param>
    public void GetItem(Item _item, int count = 1)
    {
        if (_item.Type == Item.ItemType.Used) // �Һ����϶�
        {
            foreach (Slot slot in slots) // ��� ������
            {
                if (slot.haveItem) // �������� �ִ� ���Կ�
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
        foreach (Slot slot in slots) // ��� ������
        {
            if (slot.item == null) // ��� �ִ� ���Կ�
            {
                slot.AddItem(_item, count); // �������߰�
                return;
            }
        }
    }

    public void SaveInven()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].haveItem)
            {
                if (!myItems.Contains(slots[i].item))
                {
                    myItems.Add(slots[i].item);
                }
            }
        }

        string Jdata = JsonConvert.SerializeObject(myItems, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/Resources/Data/MyInvenItems.text", Jdata);

        Debug.Log("�κ� ���̺� �Ϸ�");
    }

    public void LoadInven(List<Item> itemList)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].Count != 0)
            {
                slots[itemList[i].SlotIndex].AddItem(itemList[i]);
                if (itemList[i].Type == Item.ItemType.Weapon)
                {
                    Debug.Log(itemList[i].Name + "�� " + itemList[i].SlotIndex + "�� �ε�����");
                }
                else
                {
                    Debug.Log(itemList[i].Name + "�� " + itemList[i].Count + "�� " + itemList[i].SlotIndex + "�� �ε�����");
                }
            }
            else
            {
                Debug.Log(i + "��° ������ ����Ʈ�� �������� ����־ �ε����");
            }
        }
    }

    /// <summary>
    /// ���ӵ��߿� ���԰����� �ٲ�� ���� ������ ����Ʈ���� ��������
    /// </summary>
    void DeleteNullSlot()
    {
        for (int i = 0; i < slots.Count; ++i)
        {
            if (slots[i] == null)
                slots.RemoveAt(i);
        }
    }
}
