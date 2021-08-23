using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Stats
{
    public string s_Name;
    public int Level;
    public float CurExp;
    public float MaxExp;
    public float MaxHp;
    public float MaxMp;
    public float Str;
    public float Dex;
    public float Int;
    public float Pos_x;
    public float Pos_y;
    public float Pos_z;

    public int Gold;
}


public class PlayerInfo : Creature
{
    public Dictionary<string,int> player_Skill_Dic = new Dictionary<string,int>();

    public Stats stats = new Stats();

    public float ItemEffectMaxHp;
    public float SkillEffectMaxHp;

    public float finalMaxMp;
    public float curMp;
    public float mpRegen;
    public float ItemEffectMaxMp;
    public float SkillEffectMaxMp;

    public float finalStr;
    public float ItemEffectStr;
    public float SkillEffectStr;

    public float finalDex;
    public float ItemEffectDex;
    public float SkillEffectDex;

    public float finalInt;
    public float ItemEffectInt;
    public float SkillEffectInt;

    public float ItemEffectAtk;
    public float SkillEffectAtk;

    SkillDatabase skillDB;

    readonly string PlayerInfoPath = "/Resources/Data/PlayerInfo.text";

    CharacterController cController;

    void Awake()
    {
        skillDB = FindObjectOfType<SkillDatabase>();
        cController = GetComponent<CharacterController>();

        LoadPlayerInfo();

        #region �÷��̾� ���� ���� �ʱ�ȭ(���� ��Ȱ��ȭ)
        //stats.Level = 1;
        //stats.CurExp = 0;
        //stats.MaxExp = stats.Level * 100;
        //stats.MaxHp = 100f + (stats.Level - 1) * 20;
        //stats.MaxMp = 20f + (stats.Level - 1) * 5;
        //stats.Str = 5f + (stats.Level - 1);
        //stats.Dex = 5f + (stats.Level - 1);
        //stats.Int = 5f + (stats.Level - 1);
        #endregion

        RefeshFinalStats();

        curHp = finalMaxHp; // �����Ҷ� Ǯ�Ƿ� �������.
        curMp = finalMaxMp;
    }

    private void Start()
    {
        //player_Skill_Dic.Add(skillDB.AllSkillDic[]); // �ӽ÷� �÷��̾��� ��ų����Ʈ�� ���̾ ��ų�� �־���.
        //skillDic.Add(skillDB.AllSkillDic[1].Index, skillDB.AllSkillDic[1]); // �ӽ÷� �÷��̾��� ��ų����Ʈ�� Hp���� ��ų �־���.
        //skillDic[0].SkillLv = 1;

        //foreach (SkillData skilldata in player_SkillList)
        //{
        //    if (skilldata.skill.skillType == Skill.SkillType.Passive)
        //    {
        //        skillDB.UsePassiveSkillOnLoad(skillDB.AllSkillDic[skilldata.skill_UID], skilldata.skill_Lv, gameObject);
        //    }
        //}
    }

    void Update()
    {

    }

    /// <summary>
    /// ���������� ����� ���ݵ� ������.
    /// </summary>
    public void RefeshFinalStats()
    {
        finalMaxHp = stats.MaxHp + ItemEffectMaxHp + SkillEffectMaxHp;
        finalMaxMp = stats.MaxMp + ItemEffectMaxMp + SkillEffectMaxMp;
        finalAtk = stats.Str + ItemEffectAtk + SkillEffectAtk;
        finalStr = stats.Str + ItemEffectStr + SkillEffectStr;
        finalDex = stats.Dex + ItemEffectDex + SkillEffectDex;
        finalInt = stats.Int + ItemEffectInt + SkillEffectInt;
    }

    public void SkillLvUp(Skill _skill)
    {
        //if (skillDic.ContainsKey(_skill.Index)) // �ش� ��ų�� �÷��̾ ������ ������
        //{
        //    skillDic[_skill.Index].SkillLv++; // �ش� ��ų�� ������ �ø���.
        //    Debug.Log(skillDic[_skill.Index].Name + " ��ų�� ��ų������ " + skillDic[_skill.Index].SkillLv + "�� �Ǿ����ϴ�.");
        //    if (skillDic[_skill.Index].skillType == Skill.SkillType.Passive) // ��ųŸ���� �нú��� ��ų�� ������ ��Ű��
        //    {
        //        skillDB.UseSkill(_skill, gameObject); // �нú� ��ų ȿ�� �ߵ�.
        //    }
        //}
        //else
        //    Debug.LogError("�÷��̾ �ش� ��ų�� ������ ���� �ʽ��ϴ�.");
    }

    public override void Hit(float _damage)
    {
        curHp -= _damage - finalDef;

        if (curHp <= 0)
        {
            state = STATE.Die;
        }
    }

    public override void Die()
    {

    }

    public void SavePlayerInfo()
    {
        stats.Pos_x = transform.position.x;
        stats.Pos_y = transform.position.y;
        stats.Pos_z = transform.position.z;

        string Jdata = JsonConvert.SerializeObject(stats, Formatting.Indented);
        File.WriteAllText(Application.dataPath + PlayerInfoPath, Jdata);

        Debug.Log("�÷��̾���� ���̺� �Ϸ�");
    }

    public void LoadPlayerInfo()
    {
        if (File.Exists(Application.dataPath + PlayerInfoPath))
        {
            string Jdata = File.ReadAllText(Application.dataPath + PlayerInfoPath);
            stats = JsonConvert.DeserializeObject<Stats>(Jdata);
            cController.enabled = false;
            transform.position = new Vector3(stats.Pos_x, stats.Pos_y, stats.Pos_z);
            cController.enabled = true;

            Debug.Log("�÷��̾���� �ε强��.");
        }
        else
            Debug.LogWarning("�÷��̾���������� �����ϴ�.");
    }
}
