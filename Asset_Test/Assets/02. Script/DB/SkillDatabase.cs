using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

[System.Serializable]
public class Skill
{
    public string UIDCODE;
    public string Name;
    public int Type;
    public int CostType;
    public int Cost;
    public int Value;
    public float ValueFactor;
    public float CoolTime;
    public int MaxSkillLv;
    public int NeedLv;

    [TextArea]
    public string SkillDescription;
    public string SkillImagePath;
}

public class SkillDatabase : MonoBehaviour
{
    public static SkillDatabase instance;

    public List<Skill> AllSkillList = new List<Skill>();
    public Dictionary<string, Skill> AllSkillDic = new Dictionary<string, Skill>();

    Player_SkillIndicator player_SkillIndicator;
    PlayerInfo player;
    PlayerActionCtrl playerAC;

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

        player_SkillIndicator = FindObjectOfType<Player_SkillIndicator>();

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
            AllSkillDic.Add(AllSkillList[i].UIDCODE, AllSkillList[i]);
        }

        player = FindObjectOfType<PlayerInfo>();
        playerAC = FindObjectOfType<PlayerActionCtrl>();

        player.player_Skill_Dic.Add(AllSkillDic["0300000"].UIDCODE, 1); // �ӽ÷� �÷��̾��� ��ų����Ʈ�� ��ų�� �־���.
        player.player_Skill_Dic.Add(AllSkillDic["0300001"].UIDCODE, 0);
        player.player_Skill_Dic.Add(AllSkillDic["0300002"].UIDCODE, 0);
        player.player_Skill_Dic.Add(AllSkillDic["0300003"].UIDCODE, 0);
        player.player_Skill_Dic.Add(AllSkillDic["0300004"].UIDCODE, 0);
        player.player_Skill_Dic.Add(AllSkillDic["0300005"].UIDCODE, 0);
    }

    public Skill NewSkill(string _UIDCODE)
    {
        //���� ������ ���� : Mathf.RoundToInt(AllSkillDic[i].Value + (AllSkillDic[i].Skill_Level - 1) * AllSkillDic[i].ValueFactor);

        var skill = new Skill();

        skill.UIDCODE = AllSkillDic[_UIDCODE].UIDCODE;
        skill.Type = AllSkillDic[_UIDCODE].Type;
        skill.Name = AllSkillDic[_UIDCODE].Name;
        skill.CostType = AllSkillDic[_UIDCODE].CostType;
        skill.Cost = AllSkillDic[_UIDCODE].Cost;
        skill.Value = AllSkillDic[_UIDCODE].Value;
        skill.ValueFactor = AllSkillDic[_UIDCODE].ValueFactor;
        skill.SkillDescription = AllSkillDic[_UIDCODE].SkillDescription;
        skill.SkillImagePath = AllSkillDic[_UIDCODE].SkillImagePath;

        return skill;
    }

    /// <summary>
    /// ��ų, ��ų�����, ��ųŸ��(�⺻�� null)�� �޾Ƽ� ��ų Ÿ�Կ����� ��ų�� ���ǰ���.
    /// </summary>
    /// <param name="_skill"></param>
    /// <param name="_user"></param>
    /// <param name="_target"></param>
    public void UseSkill(Skill _skill, GameObject _user, GameObject _target = null, SkillSlot _skillslot = null)
    {
        PlayerInfo player;

        if (_user.CompareTag("Player"))
        {
            player = _user.GetComponent<PlayerInfo>();

            if (player.player_Skill_Dic[_skill.UIDCODE] == 0)
            {
                Debug.Log("���� ����� ���� ��ų�Դϴ�.");
                return;
            }

            switch (_skill.Type)
            {
                case 0: // �нú�
                    switch (_skill.UIDCODE)
                    {
                        case "0300001": //"Hp����"
                            if (player.player_Skill_Dic[_skill.UIDCODE] > 1)
                            {
                                player.SkillEffectMaxHp += _skill.ValueFactor;
                                player.curHp += _skill.ValueFactor;
                                player.RefeshFinalStats();
                                //Debug.Log(_skill.Name + " (�нú�)��ų ������");
                            }
                            else if (player.player_Skill_Dic[_skill.UIDCODE] == 1)
                            {
                                player.SkillEffectMaxHp += _skill.Value;
                                player.curHp += _skill.Value;
                                player.RefeshFinalStats();
                                //Debug.Log(_skill.Name + " (�нú�)��ų ����");
                            }
                            break;
                    }
                    break;
                case 1: // ��Ÿ����
                    switch (_skill.UIDCODE)
                    {
                        case "0300000":
                            player.curMp -= _skill.Cost;
                            _skillslot.curCooltime = _skill.CoolTime;
                            Vector3 skillPos = player.transform.position + player.transform.forward * 2 + new Vector3(0, 1.9f, 0);
                            var obj = Instantiate(Resources.Load<GameObject>("Skill/Prefebs/FireBall"), skillPos, Quaternion.identity);
                            // ���߿� ������Ʈ Ǯ���ؼ� �̸� �����س��� ������Ʈ Ȱ��ȭ�ؼ� ����ϰ� �����ؾ���
                            obj.transform.forward = player.transform.forward;
                            obj.GetComponent<FireBall>().player = player;
                            break;
                    }
                    break;
                case 2: // Ÿ����
                        //switch (_skill.UIDCODE)
                        //{

                    //}
                    break;
                case 3: // ����
                    //switch (_skill.UIDCODE)
                    //{

                    //}
                    break;
                //case Skill.SkillType.AoEAttack:

                //    break;

                //case Skill.SkillType.Debuff:

                //    break;
                //case Skill.SkillType.Heal:

                //    break;
                default:
                    Debug.LogError("��ųŸ�Կ� ���� ��ų�Դϴ�.");
                    break;
            }
        }


    }

    public void UsePassiveSkillOnLoad(Skill _skill, int _skillLv, GameObject _user)
    {
        if (_user.CompareTag("Player"))
        {
            var player = _user.GetComponent<PlayerInfo>();
            if (_skill.Type == 0)
            {
                switch (_skill.UIDCODE)
                {
                    case "0300001"://"Hp����"
                        if (_skillLv > 0)
                        {
                            player.SkillEffectMaxHp += _skill.Value + (_skillLv - 1) * _skill.ValueFactor;
                            player.RefeshFinalStats();
                            player.curHp = player.finalMaxHp;
                            //Debug.Log(_skill.Name + " (�нú�)��ų ȿ�� �ߵ�");
                        }
                        else
                        {
                            //Debug.Log("���� ����� ���� ��ų�̶� ȿ���ߵ��ȵ�.");
                        }
                        break;
                }
            }
        }
        //else if()
        //{
        //  ���߿� �÷��̾� �̿� ����� �нú� ��ų�� ����ҋ� ��Ȳ.
        //}
    }

    public void PassiveSkillLvDown(Skill _skill, GameObject _user)
    {
        if (_user.CompareTag("Player"))
        {
            var player = _user.GetComponent<PlayerInfo>();
            if (_skill.Type == 0)
            {
                switch (_skill.UIDCODE)
                {
                    case "0300001"://"Hp����"
                        if (player.player_Skill_Dic[_skill.UIDCODE] > 0)
                        {
                            player.SkillEffectMaxHp -= _skill.ValueFactor;
                            player.curHp -= _skill.ValueFactor;
                            player.RefeshFinalStats();
                        }
                        else if (player.player_Skill_Dic[_skill.UIDCODE] <= 0)
                        {
                            player.SkillEffectMaxHp -= _skill.Value;
                            player.curHp -= _skill.Value;
                            player.RefeshFinalStats();
                        }
                        break;
                }
            }
        }
    }
}
