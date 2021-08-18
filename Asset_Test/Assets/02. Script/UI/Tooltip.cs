using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public GameObject go_Tooltip;

    [SerializeField]
    Text NameText;
    [SerializeField]
    Text TypeText;
    [SerializeField]
    Text LevelText;
    [SerializeField]
    Text BaseValueText;
    [SerializeField]
    Text RequireText;
    [SerializeField]
    Text EffectText;
    [SerializeField]
    Text CostText;
    [SerializeField]
    GameObject[] Divider;

    Vector3 RD_Offset;
    Vector3 RU_Offset;

    private void Awake()
    {
        RD_Offset = new Vector3(go_Tooltip.GetComponent<RectTransform>().rect.width * 0.5f, -go_Tooltip.GetComponent<RectTransform>().rect.height * 0.5f); // 오른쪽 아래로 띄우는 오프셋
        RU_Offset = new Vector3(go_Tooltip.GetComponent<RectTransform>().rect.width * 0.5f, go_Tooltip.GetComponent<RectTransform>().rect.height * 0.5f); // 오른쪽 위로 띄우는 오프셋
    }

    void Start()
    {
        CostText.gameObject.SetActive(false);

        #region 아이템 툴팁 체크 (디버그용)
        Item _testItem = ItemDatabase.instance.newItem("0000008");
        #region 아이템 딕셔너리 키 체크 (디버그용)
        //List<int> Keys = new List<int>();
        //Keys.AddRange(_testItem.itemEffect.ValueDic.Keys);
        //Debug.Log(Keys[0]);
        #endregion
        ShowTooltip(_testItem);
        #endregion
    }

    void Update()
    {

    }

    public void ShowTooltip(Item _item)
    {
        #region 아이템 이름
        NameText.text = _item.Name;

        switch (_item.Rarity)
        {
            case 0:
                NameText.color = Color.white;
                break;
            case 1:
                ColorUtility.TryParseHtmlString("#ABA3FF", out Color color_Rare);
                NameText.color = color_Rare;
                break;
            case 2:
                ColorUtility.TryParseHtmlString("#DDD04A", out Color color_Unique);
                NameText.color = color_Unique;
                break;
            case 3:
                ColorUtility.TryParseHtmlString("#C5804F", out Color color_Epic);
                NameText.color = color_Epic;
                break;
            case 4:
                ColorUtility.TryParseHtmlString("#3BC1AF", out Color color_Set);
                NameText.color = color_Set;
                break;
            default:
                //NameText.color = 
                break;
        }
        #endregion

        #region 아이템 타입
        switch (_item.Type)
        {
            case 0:
                TypeText.text = "한손무기";
                break;
            case 1:
                TypeText.text = "두손무기";
                break;
            case 2:
                TypeText.text = "갑옷";
                break;
            case 3:
                TypeText.text = "헬멧";
                break;
            case 4:
                TypeText.text = "벨트";
                break;
            case 5:
                TypeText.text = "장갑";
                break;
            case 6:
                TypeText.text = "신발";
                break;
            case 7:
                TypeText.text = "목걸이";
                break;
            case 8:
                TypeText.text = "반지";
                break;
            case 9:
                TypeText.text = "소비";
                break;
            case 10:
                TypeText.text = "재료";
                break;
            default:
                TypeText.text = "아이템 타입 오류";
                break;
                #region 영문명
                //case 0:
                //    TypeText.text = "OneHandWeapon";
                //    break;
                //case 1:
                //    TypeText.text = "TwoHandWeapon";
                //    break;
                //case 2:
                //    TypeText.text = "Armor";
                //    break;
                //case 3:
                //    TypeText.text = "Helmet";
                //    break;
                //case 4:
                //    TypeText.text = "Belt";
                //    break;
                //case 5:
                //    TypeText.text = "Gloves";
                //    break;
                //case 6:
                //    TypeText.text = "Boots";
                //    break;
                //case 7:
                //    TypeText.text = "Necklace";
                //    break;
                //case 8:
                //    TypeText.text = "Ring";
                //    break;
                //case 9:
                //    TypeText.text = "Used";
                //    break;
                //case 10:
                //    TypeText.text = "Material";
                //    break;
                //default:
                //    TypeText.text = "아이템 타입 오류";
                //    break;
                #endregion
        }
        #endregion

        #region 아이템 베이스 값 텍스트
        switch (_item.Type)
        {
            case 0:
                BaseValueText.text = string.Format("공격력 : <color=#ABA3FF><b>{0} ~ {1}</b></color>", _item.itemEffect.ValueDic[7], _item.itemEffect.ValueDic[7] + 1);
                break;
            case 1:
                BaseValueText.text = string.Format("공격력 : <color=#ABA3FF><b>{0} ~ {1}</b></color>", _item.itemEffect.ValueDic[7], _item.itemEffect.ValueDic[7] + 1);
                break;
            case 2:
                BaseValueText.text = string.Format("방어력 : <color=#ABA3FF><b>{0}</b></color>", _item.itemEffect.ValueDic[9]);
                break;
            case 3:
                //TypeText.text = "헬멧";
                break;
            case 4:
                //TypeText.text = "벨트";
                break;
            case 5:
                //TypeText.text = "장갑";
                break;
            case 6:
                //TypeText.text = "신발";
                break;
            case 7:
                //TypeText.text = "목걸이";
                break;
            case 8:
                //TypeText.text = "반지";
                break;
            case 9:
                //TypeText.text = "소비";
                BaseValueText.gameObject.SetActive(false);
                break;
            case 10:
                //TypeText.text = "재료";
                break;
        }
        #endregion

        #region 아이템 필요조건
        string[] str = new string[64];
        List<int> keys = new List<int>();
        keys.AddRange(_item.itemEffect.RequireValueDic.Keys);

        for (int i = 0; i < _item.itemEffect.RequireValueDic.Count; i++)
        {
            switch (keys[i])
            {
                case 0:
                    RequireText.gameObject.SetActive(false);
                    foreach (GameObject obj in Divider)
                        obj.SetActive(false);
                    break;
                case 1:
                    str[i] = string.Format("레벨 <color><b>{0}</b></color>", _item.itemEffect.RequireValueDic[1]);
                    break;
                case 2:
                    str[i] = string.Format("힘 <color><b>{0}</b></color>", _item.itemEffect.RequireValueDic[2]);
                    break;
                case 3:
                    str[i] = string.Format("민첩 <color><b>{0}</b></color>", _item.itemEffect.RequireValueDic[3]);
                    break;
                case 4:
                    str[i] = string.Format("지능 <color><b>{0}</b></color>", _item.itemEffect.RequireValueDic[4]);
                    break;
                case 5:
                    str[i] = string.Format("HpMax <color><b>{0}</b></color>", _item.itemEffect.RequireValueDic[5]);
                    break;
                case 6:
                    str[i] = string.Format("MpMax <color><b>{0}</b></color>", _item.itemEffect.RequireValueDic[6]);
                    break;
                default:
                    break;
            }

            if (i != 0)
                RequireText.text += ", " + str[i];
            else
                RequireText.text = "요구 " + str[i];
        }
        #endregion

        #region 아이템 효과
        string[] str2 = new string[64];
        List<int> keys2 = new List<int>();
        keys2.AddRange(_item.itemEffect.ValueDic.Keys);
        int mainStart = 1;

        if (_item.Type == 9 || _item.Type == 10)
            mainStart = 0;

        for (int i = mainStart; i < _item.itemEffect.ValueDic.Count; i++)
        {
            switch (keys2[i])
            {
                case 1:
                    str2[i] = string.Format("Hp <color><b>{0}</b></color> 회복", _item.itemEffect.ValueDic[1]);
                    break;
                case 2:
                    str2[i] = string.Format("Mp <color><b>{0}</b></color> 회복", _item.itemEffect.ValueDic[2]);
                    break;
                case 3:
                    str2[i] = string.Format("Hp최대치 <color><b>{0}</b></color> 증가", _item.itemEffect.ValueDic[3]);
                    break;
                case 4:
                    str2[i] = string.Format("Hp최대치 <color><b>{0}%</b></color> 증가", _item.itemEffect.ValueDic[4]);
                    break;
                case 5:
                    str2[i] = string.Format("Mp최대치 <color><b>{0}</b></color> 증가", _item.itemEffect.ValueDic[5]);
                    break;
                case 6:
                    str2[i] = string.Format("Mp최대치 <color><b>{0}%</b></color> 증가", _item.itemEffect.ValueDic[6]);
                    break;
                case 7:
                    str2[i] = string.Format("공격력 <color><b>{0}</b></color> 증가", _item.itemEffect.ValueDic[7]);
                    break;
                case 8:
                    str2[i] = string.Format("공격력 <color><b>{0}%</b></color> 증가", _item.itemEffect.ValueDic[8]);
                    break;
                case 9:
                    str2[i] = string.Format("방어력 <color><b>{0}</b></color> 증가", _item.itemEffect.ValueDic[9]);
                    break;
                case 10:
                    str2[i] = string.Format("방어력 <color><b>{0}%</b></color> 증가", _item.itemEffect.ValueDic[10]);
                    break;
                case 11:
                    str2[i] = string.Format("힘 <color><b>{0}</b></color> 증가", _item.itemEffect.ValueDic[11]);
                    break;
                case 12:
                    str2[i] = string.Format("힘 <color><b>{0}</b>%</color> 증가", _item.itemEffect.ValueDic[12]);
                    break;
                case 13:
                    str2[i] = string.Format("민첩 <color><b>{0}</b></color> 증가", _item.itemEffect.ValueDic[13]);
                    break;
                case 14:
                    str2[i] = string.Format("민첩 <color><b>{0}%</b></color> 증가", _item.itemEffect.ValueDic[14]);
                    break;
                case 15:
                    str2[i] = string.Format("지능 <color><b>{0}</b></color> 증가", _item.itemEffect.ValueDic[15]);
                    break;
                case 16:
                    str2[i] = string.Format("지능 <color><b>{0}%</b></color> 증가", _item.itemEffect.ValueDic[16]);
                    break;
                case 17:
                    str2[i] = string.Format("공격시 생명력 <color><b>{0}</b></color> Hp회복", _item.itemEffect.ValueDic[17]);
                    break;
                case 18:
                    str2[i] = string.Format("공격시 데미지의 <color><b>{0}%</b></color> Hp회복", _item.itemEffect.ValueDic[18]);
                    break;
                default:
                    break;
            }

            if (i != mainStart)
                EffectText.text += "\n" + str2[i];
            else
                EffectText.text = str2[i];
        }
        #endregion
    }

    public void ShowTooltip(Skill _skill)
    {

    }
}
