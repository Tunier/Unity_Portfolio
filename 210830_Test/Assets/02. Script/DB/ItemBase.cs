public abstract class ItemBase
{
    public enum RARITY
    {
        Common,
        Rare,
        Unique,
        Epic,
    }

    public int UID;

    public string Name;
    public RARITY Rarity;
    public string Discription;

    public int SellCost;
    public int BuyCost;

    public string ItemImagePath;
    public string ItemPrefebPats;

    public int SlotIndex;
}

public class Used : ItemBase // 소비템
{
    /// <summary>
    /// AV = Absolue Value (절대값), Per = Percentage (퍼센트)
    /// </summary>
    //public enum VALUETYPE
    //{
    //    None,
    //    AV_CurHp,
    //    Per_CurHp,
    //    AV_CurMp,
    //    Per_CurMp,
    //    AV_Exp,
    //    Per_Exp,
    //}

    //public float value;

    public int UseEffectType;
}

public class Material : ItemBase // 재료템
{
    
}

public abstract class Equipment : ItemBase // 장비템
{
    /// <summary>
    /// AV = Absolue Value (절대값), Per = Percentage (퍼센트)
    /// </summary>
    public enum VALUETYPE
    { 
        None,
        AV_ATK,
        Per_ATK,
    }

    public float value;
}
