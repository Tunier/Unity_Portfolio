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

        slots.AddRange(slotsParent.GetComponentsInChildren<Slot>()); // 칠드런으로 받아오는 이유는 비활성화 되면 못받아오기때문에 비활성화된 상태도 찾아올수있는 칠드런으로 받아온다.
    }

    private void Start()
    {
        rect.localPosition = new Vector3(190, -12); // 게임켜면 위치 초기화.
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
    /// 인벤토리에 아이템을 추가시킴. 겹칠수 있는 아이템은 겹쳐줌.
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="count"></param>
    public void GetItem(Item _item, int count = 1)
    {
        if (_item.Type == Item.ItemType.Used) // 소비탬일때
        {
            foreach (Slot slot in slots) // 모든 슬롯중
            {
                if (slot.haveItem) // 아이템이 있는 슬롯에
                {
                    if (slot.item.Name == _item.Name) // 같은 이름을 가진 item이 있으면
                    {
                        slot.SetSlotCount(count); // 갯수를 증가시킴
                        return;
                    }
                }
            }
        }

        // 장비 종류에 상관없이
        foreach (Slot slot in slots) // 모든 슬롯중
        {
            if (slot.item == null) // 비어 있는 슬롯에
            {
                slot.AddItem(_item, count); // 아이템추가
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

        Debug.Log("인벤 세이브 완료");
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
                    Debug.Log(itemList[i].Name + "을 " + itemList[i].SlotIndex + "에 로드했음");
                }
                else
                {
                    Debug.Log(itemList[i].Name + "을 " + itemList[i].Count + "개 " + itemList[i].SlotIndex + "에 로드했음");
                }
            }
            else
            {
                Debug.Log(i + "번째 아이템 리스트의 아이템은 비어있어서 로드안함");
            }
        }
    }

    /// <summary>
    /// 게임도중에 슬롯갯수가 바뀌면 없는 슬롯을 리스트에서 제거해줌
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
