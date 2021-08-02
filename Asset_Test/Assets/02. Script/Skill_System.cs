using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public enum SkillType
    { 
        Attack,
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

    public Skill(string _Name, SkillType _skillType)
    { 
    }

    public string Name;
    public SkillType skillType;
    public CostType costType;
    public int Cost;
    public int Damage;
    public int Skill_Level;
}

public class Skill_System : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
