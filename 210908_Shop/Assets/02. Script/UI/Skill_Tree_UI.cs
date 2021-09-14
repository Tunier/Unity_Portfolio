using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Tree_UI : MonoBehaviour
{
    PlayerInfo playerinfo;
    Tooltip tooltip;

    [SerializeField]
    Button SkillTree_BG;

    public GameObject skilltree_Slot_Parent;
    public List<SkillSlot> slots = new List<SkillSlot>();

    public List<Skill> player_skill = new List<Skill>();

    public GameObject skillOptionPanel;

    public SkillSlot curSlot = null;

    private void Awake()
    {
        playerinfo = FindObjectOfType<PlayerInfo>();
        tooltip = FindObjectOfType<Tooltip>();

        slots.AddRange(skilltree_Slot_Parent.GetComponentsInChildren<SkillSlot>());
    }

    void Start()
    {
        List<string> keys = new List<string>();

        keys.AddRange(playerinfo.player_Skill_Dic.Keys);

        foreach (string key in keys)
        {
            player_skill.Add(SkillDatabase.instance.AllSkillDic[key]);
        }

        for (int i = 0; i < keys.Count; i++)
        {
            slots[i].AddSkill(player_skill[i]);
        }

        //foreach (Skill skill in player_skill)
        //{
        //    switch (skill.NeedLv)
        //    {
        //        case 1:

        //            break;
        //    }
        //}
    }


    void Update()
    {
        if (!SkillTree_BG.gameObject.activeSelf)
        {
            skillOptionPanel.SetActive(false);
        }
    }

    public void BG_Click()
    {
        skillOptionPanel.SetActive(false);
    }

    public void PlusBtnClick()
    {
        if (playerinfo.player_Skill_Dic[curSlot.skill.UIDCODE] < curSlot.skill.MaxSkillLv)
        {
            playerinfo.SetSkillLv(SkillDatabase.instance.AllSkillDic[curSlot.skill.UIDCODE]);
            tooltip.ShowTooltip(curSlot.skill);
        }
    }

    public void MinusBtnClick()
    {
        if (playerinfo.player_Skill_Dic[curSlot.skill.UIDCODE] > 0)
        {
            playerinfo.SetSkillLv(SkillDatabase.instance.AllSkillDic[curSlot.skill.UIDCODE], -1);
            tooltip.ShowTooltip(curSlot.skill);
        }
    }
}