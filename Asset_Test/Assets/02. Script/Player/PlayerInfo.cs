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
        player_Skill_Dic.Add(skillDB.AllSkillDic["0300000"].UIDCODE, 1); // 임시로 플레이어의 스킬리스트에 파이어볼 스킬을 넣어줌.
        player_Skill_Dic.Add(skillDB.AllSkillDic["0300001"].UIDCODE, 1); // 임시로 플레이어의 스킬리스트에 Hp증가 스킬 넣어줌.

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

        // 시작할때 풀피, 풀마나로 만들어줌.
        curHp = finalMaxHp;
        curMp = finalMaxMp;
    }

    /// <summary>
    /// 최종적으로 사용할 스텟들 정리함.
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
    /// 플레이어가 해당 스킬을 가지고있는지 검사한후, 해당스킬의 레벨을 _i(기본값 1)올린다.
    /// </summary>
    /// <param name="_skill"></param>
    public void SkillLvUp(Skill _skill, int _i = 1)
    {
        if (player_Skill_Dic.ContainsKey(_skill.UIDCODE))
        {
            player_Skill_Dic[_skill.UIDCODE] += _i;
            //Debug.Log(_skill.Name + " 스킬의 스킬레벨이 " + player_Skill_Dic[_skill.UIDCODE] + "가 되었습니다.");

            if (_skill.Type == 0)//패시브스킬이면
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
        {
            Debug.LogWarning("플레이어데이터파일이 없습니다.\n플레이어정보 강제 초기화 진행합니다.");

            #region 플레이어 정보 강제 초기화
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

    #region 디버그시 사용하는 함수들 모음
    public void DebugSkillLvUp()
    {
        string Skill_UID = "0300001";

        if (player_Skill_Dic.ContainsKey(Skill_UID))
        {
            player_Skill_Dic[Skill_UID]++;
            Debug.Log(skillDB.AllSkillDic[Skill_UID].Name + " 스킬의 스킬레벨이 " + player_Skill_Dic[Skill_UID] + "가 되었습니다.");

            if (skillDB.AllSkillDic[Skill_UID].Type == 0)//패시브스킬이면
            {
                skillDB.UseSkill(skillDB.AllSkillDic[Skill_UID], gameObject);
                Debug.Log("패시브 스킬 사용됨");
            }
        }
    }
    #endregion
}
