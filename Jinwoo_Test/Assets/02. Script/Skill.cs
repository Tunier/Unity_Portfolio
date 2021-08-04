using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "New Skill/skill")]
public class Skill : ScriptableObject
{
    public enum SkillType
    {
        Active,
        Passive,
    }
    public string Name;
    public SkillType Type;
    [TextArea]
    public string Desc;
    public Sprite skillImage;
    public GameObject skillPrefab;
    public float coolTime;
    public float mpCost;
}
