using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Item
{
    public enum ItemType
    {
        Weapon,
        Used,
    }

    public enum ItemRarity
    {
        Common,
        Rare,
        Epic,
    }

    public int Index;
    [JsonConverter(typeof(StringEnumConverter))]
    public ItemType Type;
    public string Name;
    [JsonConverter(typeof(StringEnumConverter))]
    public ItemRarity Rarity;
    public string ValueTypes;
    public string Values;
    public Dictionary<int, float> ItemEffect = new Dictionary<int, float>();
    public int BuyCost;
    public int SellCost;
    public int Count;
    public int SlotIndex;

    public string ItemImagePath;
    public string ItemPrefabPath;
}

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance = null;

    public List<Item> AllItemList = new List<Item>();
    public List<Item> LoadItemList = new List<Item>();

    public Dictionary<int, Item> AllItemDic = new Dictionary<int, Item>();

    [SerializeField]
    Inventory inven;

    const string itemDataPath = "/Resources/Data/All_Item_Data.text";
    const string invenSavePath = "/Resources/Data/MyInvenItems.text";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }

        #region txt to json
        //ItemData = Resources.Load<TextAsset>(itemDataPath);

        //string[] line = ItemData.text.Substring(0, ItemData.text.Length - 1).Split('\n');
        //for (int i = 0; i < line.Length; i++)
        //{
        //    Item.ItemType type = new Item.ItemType();
        //    Item.ItemRarity rarity = new Item.ItemRarity();

        //    string[] row = line[i].Split('\t');

        //    if (Enum.IsDefined(typeof(Item.ItemType), row[1]))
        //        type = (Item.ItemType)Enum.Parse(typeof(Item.ItemType), row[1], true);
        //    else
        //        Debug.Log("아이템 타입 오류");

        //    if (Enum.IsDefined(typeof(Item.ItemRarity), row[3]))
        //        rarity = (Item.ItemRarity)Enum.Parse(typeof(Item.ItemRarity), row[3], true);
        //    else
        //        Debug.Log("아이템 레어리티 오류");

        //    row[6] = row[6].Substring(0, row[6].Length - 1); // 마지막 한글자 안잘라주면 경로가 이상하게 잡힘.

        //    AllItemList.Add(new Item(int.Parse(row[0]), type, row[2], rarity, int.Parse(row[4]), int.Parse(row[5]), row[6]));
        //}
        #endregion

        #region Json 아이템 데이터 받아와서 리스트와 딕셔너리에 저장하기.
        if (File.Exists(Application.dataPath + itemDataPath))
        {
            string Jdata = File.ReadAllText(Application.dataPath + itemDataPath);
            AllItemList = JsonConvert.DeserializeObject<List<Item>>(Jdata);
            Debug.Log("아이템데이터 로드성공.");
        }
        else
            Debug.LogWarning("파일이 없습니다.");


        for (int i = 0; i < AllItemList.Count; i++)
        {
            string[] obj = AllItemList[i].ValueTypes.Split('/');
            string[] obj2 = AllItemList[i].Values.Split('/');

            for (int j = 0; j < obj.Length; j++)
            {
                AllItemList[i].ItemEffect.Add(int.Parse(obj[j]), float.Parse(obj2[j]));
            }
        }

        for (int i = 0; i < AllItemList.Count; i++)
        {
            AllItemDic.Add(AllItemList[i].Index, AllItemList[i]);
        }
        #endregion
    }

    public void Save()
    {
        inven.SaveInven();
    }

    public void Load()
    {
        if (File.Exists(Application.dataPath + invenSavePath))
        {
            string Jdata = File.ReadAllText(Application.dataPath + invenSavePath);
            //LoadItemList = JsonConvert.DeserializeObject<List<Item>>(Jdata);
            inven.LoadInven(LoadItemList);

            Debug.Log("인벤토리 로드성공");
        }
        else
            Debug.Log("세이브파일없음");
    }

    //public Item newItem(int i)
    //{
    //    int damage;

    //    var randomItemQuality = UnityEngine.Random.Range(1, 1000);

    //    if (AllItemDic[i].Type == Item.ItemType.Weapon)
    //    {
    //        if (randomItemQuality > 750)
    //            damage = Mathf.RoundToInt(AllItemDic[i].Value * 1.1f);
    //        else if (randomItemQuality > 250)
    //            damage = AllItemDic[i].Value;
    //        else
    //            damage = Mathf.RoundToInt(AllItemDic[i].Value * 0.9f);
    //    }
    //    else
    //    {
    //        damage = AllItemDic[i].Value;
    //    }


    //    var item = new Item(AllItemDic[i].Index,
    //                        AllItemDic[i].Type,
    //                        AllItemDic[i].Name,
    //                        AllItemDic[i].Rarity,
    //                        damage,
    //                        AllItemDic[i].BuyCost,
    //                        AllItemDic[i].SellCost,
    //                        AllItemDic[i].ItemImagePath);

    //    return item;
    //}

    public void UseItem(Item _item)
    {
        switch (_item.Type)
        {
            case Item.ItemType.Used:
                //_item.

                break;
        }
    }
}
