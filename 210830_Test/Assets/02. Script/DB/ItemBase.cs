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

public class Used : ItemBase // �Һ���
{
    /// <summary>
    /// AV = Absolue Value (���밪), Per = Percentage (�ۼ�Ʈ)
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

public class Material : ItemBase // �����
{
    
}

public abstract class Equipment : ItemBase // �����
{
    /// <summary>
    /// AV = Absolue Value (���밪), Per = Percentage (�ۼ�Ʈ)
    /// </summary>
    public enum VALUETYPE
    { 
        None,
        AV_ATK,
        Per_ATK,
    }

    public float value;
}
