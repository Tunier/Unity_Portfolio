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
            Debug.Log("��ų ������ �ε强��.");
        }
        else
            Debug.LogWarning("��ų ������ ������ �����ϴ�.");

        for (int i = 0; i < AllSkillList.Count; i++)
        {
            AllSkillDic.Add(AllSkillList[i].Index, AllSkillList[i]);

            if (i == AllSkillList.Count - 1)
            {
                Debug.Log("��� ��� �Ϸ�");
            }
        }
    }

    public Skill NewSkill(int i)
    {
        //���� ������ ���� : Mathf.RoundToInt(AllSkillDic[i].Value + (AllSkillDic[i].Skill_Level - 1) * AllSkillDic[i].ValueFactor);

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
    /// ��ų, ��ųŸ��, ��ų�����(�⺻�� null)�� �޾Ƽ� ��ų Ÿ�Կ����� ��ų�� ���ǰ���.
    /// </summary>
    /// <param name="_skill"></param>
    /// <param name="_user"></param>
    /// <param name="_target"></param>
    public void UseSkill(Skill _skill, GameObject _user, GameObject _target = null)
    {
        if (_skill.SkillLv == 0)
        {
            Debug.Log("���� ����� ���� ��ų�Դϴ�.");
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
                        case 1://"Hp����"
                            if (_skill.SkillLv > 1)
                            {
                                player.SkillEffectMaxHp += _skill.ValueFactor;
                                player.RefeshFinalStats();
                                Debug.Log(_skill.Name + " (�нú�)��ų ������");
                            }
                            else if (_skill.SkillLv == 1)
                            {
                                player.SkillEffectMaxHp += _skill.Value;
                                player.RefeshFinalStats();
                                Debug.Log(_skill.Name + " (�нú�)��ų ����");
                            }
                            break;
                    }
                }
                // ���߿� ����ڰ� �÷��̾� �̿��϶� �ۼ�.
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
                        case 0: // "���̾"
                                // ��ų�� ��Ÿ������, ������ ��뿡 �ʿ��� �����̻� �ִ��� üũ �ڵ� �ۼ� �ʿ�
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
                Debug.LogError("��ųŸ�Կ� ���� ��ų�Դϴ�.");
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
                case 1://"Hp����"
                    if (_skill.SkillLv > 0)
                    {
                        player.SkillEffectMaxHp += _skill.Value + (_skill.SkillLv - 1) * _skill.ValueFactor;
                        player.RefeshFinalStats();
                        Debug.Log(_skill.Name + " (�нú�)��ų ȿ�� �ߵ�");
                    }
                    else
                    {
                        //Debug.Log("���� ����� ���� ��ų�̶� ȿ���ߵ��ȵ�.");
                    }
                    break;
            }
        }
        //else if()
        //{
        //  ���߿� �÷��̾� �̿� ����� �нú� ��ų�� ����ҋ� ��Ȳ.
        //}
    }
}
