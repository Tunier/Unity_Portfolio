using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerInfo : Creature
{
    public Dictionary<string, int> player_Skill_Dic = new Dictionary<string, int>();

    public float ItemEffectMaxHp;
    public float SkillEffectMaxHp;

    public float finalMaxMp { get; protected set; }
    public float curMp;
    public float mpRegen;
    public float ItemEffectMaxMp;
    public float SkillEffectMaxMp;

    public float finalStr { get; protected set; }
    public float ItemEffectStr;
    public float SkillEffectStr;

    public float finalDex { get; protected set; }
    public float ItemEffectDex;
    public float SkillEffectDex;

    public float finalInt { get; protected set; }
    public float ItemEffectInt;
    public float SkillEffectInt;

    public float ItemEffectAtk;
    public float SkillEffectAtk;

    public float ItemEffectDef;
    public float SkillEffectDef;

    SkillDatabase skillDB;

    readonly string PlayerInfoPath = "/Resources/Data/PlayerInfo.text";

    CharacterController cController;

    void Awake()
    {
        skillDB = FindObjectOfType<SkillDatabase>();
        cController = GetComponent<CharacterController>();

        LoadPlayerInfo();

        RefeshFinalStats();
    }

    private void Start()
    {
        player_Skill_Dic.Add(skillDB.AllSkillDic["0300000"].UIDCODE, 1); // �ӽ÷� �÷��̾��� ��ų����Ʈ�� ���̾ ��ų�� �־���.
        player_Skill_Dic.Add(skillDB.AllSkillDic["0300001"].UIDCODE, 1); // �ӽ÷� �÷��̾��� ��ų����Ʈ�� Hp���� ��ų �־���.

        List<string> keys = new List<string>();
        keys.AddRange(player_Skill_Dic.Keys);
        //foreach (string key in keys)
        //{
        //    Debug.Log(key);
        //}

        foreach (string skill_UID in keys)
        {
            skillDB.UsePassiveSkillOnLoad(skillDB.AllSkillDic[skill_UID], player_Skill_Dic[skill_UID], gameObject);
        }

        // �����Ҷ� Ǯ��, Ǯ������ �������.
        curHp = finalMaxHp;
        curMp = finalMaxMp;
    }

    /// <summary>
    /// ���������� ����� ���ݵ� ������.
    /// </summary>
    public void RefeshFinalStats()
    {
        finalMaxHp = stats.MaxHp + ItemEffectMaxHp + SkillEffectMaxHp;
        finalMaxMp = stats.MaxMp + ItemEffectMaxMp + SkillEffectMaxMp;
        finalAtk = ItemEffectAtk + SkillEffectAtk;
        finalDef = ItemEffectDef + SkillEffectDef;
        finalStr = stats.Str + ItemEffectStr + SkillEffectStr;
        finalDex = stats.Dex + ItemEffectDex + SkillEffectDex;
        finalInt = stats.Int + ItemEffectInt + SkillEffectInt;
    }

    public void LevelUp()
    {
        float ExpFactor = 1f;

        stats.Level++;
        stats.CurExp -= stats.MaxExp;

        if (stats.Level == 1)
            stats.MaxExp = 100f;
        else
        {
            for (int i = 1; i < stats.Level; i++)
            {
                ExpFactor *= 1.1f;
            }
            stats.MaxExp = Mathf.RoundToInt(100f * (stats.Level - 1) + (100 * ExpFactor));
        }

        stats.MaxHp = 100f + (stats.Level - 1) * 15;
        stats.MaxMp = 20f + (stats.Level - 1) * 5;
        stats.Str = 5f + (stats.Level - 1);
        stats.Dex = 5f + (stats.Level - 1);
        stats.Int = 5f + (stats.Level - 1);

        RefeshFinalStats();

        curHp = finalMaxHp;
        curMp = finalMaxMp;
    }

    /// <summary>
    /// �÷��̾ �ش� ��ų�� �������ִ��� �˻�����, �ش罺ų�� ������ _i(�⺻�� 1)�ø���.
    /// </summary>
    /// <param name="_skill"></param>
    public void SkillLvUp(Skill _skill, int _i = 1)
    {
        if (player_Skill_Dic.ContainsKey(_skill.UIDCODE))
        {
            player_Skill_Dic[_skill.UIDCODE] += _i;
            //Debug.Log(_skill.Name + " ��ų�� ��ų������ " + player_Skill_Dic[_skill.UIDCODE] + "�� �Ǿ����ϴ�.");

            if (_skill.Type == 0)//�нú꽺ų�̸�
                skillDB.UseSkill(_skill, gameObject);
        }
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
        {
            Debug.LogWarning("�÷��̾���������� �����ϴ�.\n�÷��̾����� ���� �ʱ�ȭ �����մϴ�.");

            #region �÷��̾� ���� ���� �ʱ�ȭ
            stats.Level = 1;
            stats.CurExp = 0;
            stats.MaxExp = stats.Level * 100;
            stats.MaxHp = 100f + (stats.Level - 1) * 20;
            stats.MaxMp = 20f + (stats.Level - 1) * 5;
            stats.Str = 5f + (stats.Level - 1);
            stats.Dex = 5f + (stats.Level - 1);
            stats.Int = 5f + (stats.Level - 1);
            #endregion
        }
    }

    #region ����׽� ����ϴ� �Լ��� ����
    public void DebugSkillLvUp()
    {
        string Skill_UID = "0300001";

        if (player_Skill_Dic.ContainsKey(Skill_UID))
        {
            player_Skill_Dic[Skill_UID]++;
            Debug.Log(skillDB.AllSkillDic[Skill_UID].Name + " ��ų�� ��ų������ " + player_Skill_Dic[Skill_UID] + "�� �Ǿ����ϴ�.");

            if (skillDB.AllSkillDic[Skill_UID].Type == 0)//�нú꽺ų�̸�
            {
                skillDB.UseSkill(skillDB.AllSkillDic[Skill_UID], gameObject);
                Debug.Log("�нú� ��ų ����");
            }
        }
    }
    #endregion
}
