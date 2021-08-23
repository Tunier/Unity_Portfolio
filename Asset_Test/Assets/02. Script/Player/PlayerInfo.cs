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

        #region 플레이어 정보 강제 초기화(현재 비활성화)
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

        curHp = finalMaxHp; // 시작할때 풀피로 만들어줌.
        curMp = finalMaxMp;
    }

    private void Start()
    {
        //player_Skill_Dic.Add(skillDB.AllSkillDic[]); // 임시로 플레이어의 스킬리스트에 파이어볼 스킬을 넣어줌.
        //skillDic.Add(skillDB.AllSkillDic[1].Index, skillDB.AllSkillDic[1]); // 임시로 플레이어의 스킬리스트에 Hp증가 스킬 넣어줌.
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
    /// 최종적으로 사용할 스텟들 정리함.
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
        //if (skillDic.ContainsKey(_skill.Index)) // 해당 스킬을 플레이어가 가지고 있을때
        //{
        //    skillDic[_skill.Index].SkillLv++; // 해당 스킬의 레벨을 올린다.
        //    Debug.Log(skillDic[_skill.Index].Name + " 스킬의 스킬레벨이 " + skillDic[_skill.Index].SkillLv + "가 되었습니다.");
        //    if (skillDic[_skill.Index].skillType == Skill.SkillType.Passive) // 스킬타입이 패시브인 스킬을 레벨업 시키면
        //    {
        //        skillDB.UseSkill(_skill, gameObject); // 패시브 스킬 효과 발동.
        //    }
        //}
        //else
        //    Debug.LogError("플레이어가 해당 스킬을 가지고 있지 않습니다.");
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

        Debug.Log("플레이어데이터 세이브 완료");
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

            Debug.Log("플레이어데이터 로드성공.");
        }
        else
            Debug.LogWarning("플레이어데이터파일이 없습니다.");
    }
}
