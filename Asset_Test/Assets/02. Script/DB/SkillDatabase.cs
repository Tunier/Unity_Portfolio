using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

[System.Serializable]
public class Skill
{
    public enum SkillType
    {
        Passive,
        AoEAttack,
        Target,
        NoneTarget,
        Buff,
        Debuff,
        Heal,
    }

    public enum CostType
    {
        None,
        Hp,
        Mp,
    }

    public int Index;
    [JsonConverter(typeof(StringEnumConverter))]
    public SkillType skillType;
    public string Name;
    [JsonConverter(typeof(StringEnumConverter))]
    public CostType costType;
    public int Cost;
    public int Value;
    public float ValueFactor;
    public float CoolTime;
    public int SkillLv;
    public int MaxSkillLv;

    [TextArea]
    public string SkillDescription;
    public string SkillImagePath;
}

public class SkillDatabase : MonoBehaviour
{
    public static SkillDatabase instance;

    Player_SkillIndicator skillIndicator;

    public List<Skill> AllSkillList = new List<Skill>();
    public Dictionary<int, Skill> AllSkillDic = new Dictionary<int, Skill>();

    const string skillDataPath = "/Resources/Data/All_Skill_Data.text";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }

        skillIndicator = FindObjectOfType<Player_SkillIndicator>();

        if (File.Exists(Application.dataPath + skillDataPath))
        {
            string Jdata = File.ReadAllText(Application.dataPath + skillDataPath);
            AllSkillList = JsonConvert.DeserializeObject<List<Skill>>(Jdata);
            Debug.Log("스킬 데이터 로드성공.");
        }
        else
            Debug.LogWarning("스킬 데이터 파일이 없습니다.");

        for (int i = 0; i < AllSkillList.Count; i++)
        {
            AllSkillDic.Add(AllSkillList[i].Index, AllSkillList[i]);

            if (i == AllSkillList.Count - 1)
            {
                Debug.Log("모두 등록 완료");
            }
        }
    }

    public Skill NewSkill(int i)
    {
        //최종 데미지 공식 : Mathf.RoundToInt(AllSkillDic[i].Value + (AllSkillDic[i].Skill_Level - 1) * AllSkillDic[i].ValueFactor);

        var skill = new Skill();

        skill.Index = AllSkillDic[i].Index;
        skill.skillType = AllSkillDic[i].skillType;
        skill.Name = AllSkillDic[i].Name;
        skill.costType = AllSkillDic[i].costType;
        skill.Cost = AllSkillDic[i].Cost;
        skill.Value = AllSkillDic[i].Value;
        skill.ValueFactor = AllSkillDic[i].ValueFactor;
        skill.SkillDescription = AllSkillDic[i].SkillDescription;
        skill.SkillImagePath = AllSkillDic[i].SkillImagePath;

        return skill;
    }

    /// <summary>
    /// 스킬, 스킬타겟, 스킬사용자(기본은 null)를 받아서 스킬 타입에따라 스킬이 사용되게함.
    /// </summary>
    /// <param name="_skill"></param>
    /// <param name="_user"></param>
    /// <param name="_target"></param>
    public void UseSkill(Skill _skill, GameObject _user, GameObject _target = null)
    {
        if (_skill.SkillLv == 0)
        {
            Debug.Log("아직 배우지 않은 스킬입니다.");
            return;
        }

        switch (_skill.skillType)
        {
            case Skill.SkillType.Passive:
                if (_user.CompareTag("Player"))
                {
                    var player = _user.GetComponent<PlayerInfo>();
                    switch (_skill.Index)
                    {
                        case 1://"Hp증가"
                            if (_skill.SkillLv > 1)
                            {
                                player.SkillEffectMaxHp += _skill.ValueFactor;
                                player.RefeshFinalStats();
                                Debug.Log(_skill.Name + " (패시브)스킬 레벨업");
                            }
                            else if (_skill.SkillLv == 1)
                            {
                                player.SkillEffectMaxHp += _skill.Value;
                                player.RefeshFinalStats();
                                Debug.Log(_skill.Name + " (패시브)스킬 습득");
                            }
                            break;
                    }
                }
                // 나중에 사용자가 플레이어 이외일때 작성.
                //else if (_user.GetComponent<>)
                //{ 
                //}
                break;
            case Skill.SkillType.Target:
                if (_user.CompareTag("Player"))
                {
                    var player = _user.GetComponent<PlayerInfo>();
                    //var target = _target.GetComponent<MonsterInfo>();
                    switch (_skill.Index)
                    {
                        //
                    }
                }
                break;
            case Skill.SkillType.NoneTarget:
                if (_user.CompareTag("Player"))
                {
                    var player = _user.GetComponent<PlayerInfo>();
                    switch (_skill.Index)
                    {
                        case 0: // "파이어볼"
                                // 스킬이 쿨타임인지, 마나가 사용에 필요한 마나이상 있는지 체크 코드 작성 필요
                            Vector3 skillPos = player.transform.position + player.transform.forward * 2 + new Vector3(0, 1.5f, 0);
                            var obj = Instantiate(Resources.Load<GameObject>("Skill/Prefebs/FireBall"), skillPos, Quaternion.identity);
                            obj.transform.forward = player.transform.forward;
                            break;
                    }
                }
                break;
            case Skill.SkillType.AoEAttack:

                break;
            case Skill.SkillType.Buff:

                break;
            case Skill.SkillType.Debuff:

                break;
            case Skill.SkillType.Heal:

                break;
            default:
                Debug.LogError("스킬타입에 없는 스킬입니다.");
                break;
        }
    }

    public void UsePassiveSkillOnLoad(Skill _skill, GameObject _user)
    {
        if (_user.CompareTag("Player"))
        {
            var player = _user.GetComponent<PlayerInfo>();
            switch (_skill.Index)
            {
                case 1://"Hp증가"
                    if (_skill.SkillLv > 0)
                    {
                        player.SkillEffectMaxHp += _skill.Value + (_skill.SkillLv - 1) * _skill.ValueFactor;
                        player.RefeshFinalStats();
                        Debug.Log(_skill.Name + " (패시브)스킬 효과 발동");
                    }
                    else
                    {
                        //Debug.Log("아직 배우지 않은 스킬이라 효과발동안됨.");
                    }
                    break;
            }
        }
        //else if()
        //{
        //  나중에 플레이어 이외 대상이 패시브 스킬을 사용할떄 상황.
        //}
    }
}
