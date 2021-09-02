using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

interface iPlayerMustHaveFuc
{
    public abstract void LevelUp();

    public abstract void RefeshFinalStats();

    public abstract void SavePlayerInfo();

    public abstract void LoadPlayerInfo();
}

public class PlayerInfo : Creature, iPlayerMustHaveFuc
{
    public Dictionary<string, int> player_Skill_Dic = new Dictionary<string, int>();

    public GameObject targetMonster;

    public float ItemEffectMaxHp;
    public float SkillEffectMaxHp;
    public float ItemEffectMaxHpMultiplier;
    public float SkillEffectMaxHpMultiplier;

    public float ItemEffectHpRegen;
    public float SkillEffectHpRegen;
    public float ItemEffectHpRegenMultiplier;
    public float SkillEffectHpRegenMultiplier;

    public float finalMaxMp { get; protected set; }
    public float curMp;
    public float ItemEffectMaxMp;
    public float SkillEffectMaxMp;
    public float ItemEffectMaxMpMultiplier;
    public float SkillEffectMaxMpMultiplier;

    public float fianlMpRegen { get; protected set; }
    public float ItemEffectMpRegen;
    public float SkillEffectMpRegen;
    public float ItemEffectMpRegenMultiplier;
    public float SkillEffectMpRegenMultiplier;

    public float finalStr { get; protected set; }
    public float ItemEffectStr;
    public float SkillEffectStr;
    public float ItemEffectStrMultiplier;
    public float SkillEffectStrMultiplier;

    public float finalDex { get; protected set; }
    public float ItemEffectDex;
    public float SkillEffectDex;
    public float ItemEffectDexMultiplier;
    public float SkillEffectDexMultiplier;

    public float finalInt { get; protected set; }
    public float ItemEffectInt;
    public float SkillEffectInt;
    public float ItemEffectIntMultiplier;
    public float SkillEffectIntMultiplier;

    public float ItemEffectAtk;
    public float SkillEffectAtk;
    public float ItemEffectAtkMultiplier;
    public float SkillEffectAtkMultiplier;

    public float ItemEffectDef;
    public float SkillEffectDef;
    public float ItemEffectDefMultiplier;
    public float SkillEffectDefMultiplier;

    public float finalLifeSteal { get; protected set; }
    public float ItemEffectLifeSteal;
    public float SkillEffectLifeSteal;

    public float finalLifeStealPercent { get; protected set; }
    public float ItemEffectLifeStealPercent;
    public float SkillEffectLifeStealPercent;

    public int finalCriticalChance { get; protected set; }
    public int ItemEffectCriticalChance;
    public int SkillEffectCriticalChace;

    public float finalCriticalDamageMuliplie { get; protected set; }
    public float ItemEffectCriticalDamageMultiple;
    public float SkillEffectCriticalDamageMultiple;

    SkillDatabase skillDB;

    const string PlayerInfoPath = "/Resources/Data/PlayerInfo.text";

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

    private void Update()
    {
        if (curHp < finalMaxHp)
            curHp += finalHpRegen * Time.deltaTime;

        if (curMp < finalMaxMp)
            curMp += fianlMpRegen * Time.deltaTime;
    }

    /// <summary>
    /// ���������� ����� ���ݵ� ������.
    /// </summary>
    public virtual void RefeshFinalStats()
    {
        finalMaxHp = stats.MaxHp + (ItemEffectMaxHp + SkillEffectMaxHp) * (1 + ItemEffectMaxHpMultiplier + SkillEffectMaxHpMultiplier);
        finalMaxMp = stats.MaxMp + (ItemEffectMaxMp + SkillEffectMaxMp) * (1 + ItemEffectMaxMpMultiplier + SkillEffectMaxMpMultiplier);
        finalAtk = stats.Str + (ItemEffectAtk + SkillEffectAtk) * (1 + ItemEffectAtkMultiplier + SkillEffectAtkMultiplier);
        finalDef = stats.Dex * 0.5f + (ItemEffectDef + SkillEffectDef) * (1 + ItemEffectDefMultiplier + SkillEffectDefMultiplier);
        finalStr = stats.Str + ItemEffectStr + SkillEffectStr * (1 + ItemEffectStrMultiplier + SkillEffectStrMultiplier);
        finalDex = stats.Dex + ItemEffectDex + SkillEffectDex * (1 + ItemEffectDexMultiplier + SkillEffectDexMultiplier);
        finalInt = stats.Int + ItemEffectInt + SkillEffectInt * (1 + ItemEffectIntMultiplier + SkillEffectIntMultiplier);

        finalCriticalChance = Mathf.RoundToInt(finalDex * 50) + ItemEffectCriticalChance + SkillEffectCriticalChace;
        finalCriticalDamageMuliplie = 1.5f + ItemEffectCriticalDamageMultiple + SkillEffectCriticalDamageMultiple;

        finalHpRegen = 0.4f + finalStr * 0.1f + ItemEffectHpRegen + SkillEffectHpRegen;
        fianlMpRegen = 0.4f + finalInt * 0.1f + ItemEffectMpRegen + SkillEffectMpRegen;
    }

    public virtual void LevelUp()
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
    public void SetSkillLv(Skill _skill, int _i = 1)
    {
        if (player_Skill_Dic.ContainsKey(_skill.UIDCODE))
        {
            player_Skill_Dic[_skill.UIDCODE] += _i;
            //Debug.Log(_skill.Name + " ��ų�� ��ų������ " + player_Skill_Dic[_skill.UIDCODE] + "�� �Ǿ����ϴ�.");

            if (_skill.Type == 0)//�нú꽺ų�̸�
            {
                if (_i == 1)
                    skillDB.UseSkill(_skill, gameObject);
                else
                    skillDB.PassiveSkillLvDown(_skill, gameObject);
            }
        }
    }

    public override void Hit(float _damage)
    {
        curHp -= _damage - finalDef;

        if (curHp <= 0)
        {
            state = STATE.Die;
            Die();
        }
    }

    public override void Die()
    {
        print("���");
    }

    public virtual void SavePlayerInfo()
    {
        stats.Pos_x = transform.position.x;
        stats.Pos_y = transform.position.y;
        stats.Pos_z = transform.position.z;

        string Jdata = JsonConvert.SerializeObject(stats, Formatting.Indented);
        File.WriteAllText(Application.dataPath + PlayerInfoPath, Jdata);

        Debug.Log("�÷��̾���� ���̺� �Ϸ�");
    }

    public virtual void LoadPlayerInfo()
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
