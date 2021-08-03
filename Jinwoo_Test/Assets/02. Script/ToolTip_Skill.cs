using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip_Skill : MonoBehaviour
{
    public GameObject baseImage;

    [SerializeField]
    Text NameAndLvTxt;
    [SerializeField]
    Text coolTimeTxt;
    [SerializeField]
    Text DescTxt;
    [SerializeField]
    Text CostTxt;

    [SerializeField]
    RectTransform quickSkillSlot;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void ShowToolTip(Skill _skill,Vector3 _pos)
    {
        baseImage.SetActive(true);
        _pos += new Vector3(baseImage.GetComponent<RectTransform>().rect.width * 0.5f,
                            -baseImage.GetComponent<RectTransform>().rect.height * 0.5f,
                            0);
        baseImage.transform.position = _pos;

        NameAndLvTxt.text = _skill.Name + "(1레벨)";
        coolTimeTxt.text = "쿨타임 : " + _skill.coolTime;
        DescTxt.text = _skill.Desc;
        CostTxt.text = "마나 : " + _skill.mpCost;
    }
    public void HideToolTip()
    {
        baseImage.SetActive(false);
    }
}
