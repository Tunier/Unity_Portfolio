using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IHit
{
    public void Hit(float _damage);
}

[System.Serializable]
public class Stats
{
    public int Level;
    public float MaxHp;
    public float MaxMp;
    public float Str;
    public float Dex;
    public float Int;
    public int Gold;
}

public enum STATE
{ 
    Idle,
    Attacking,
    Jump,
    JumpAndAttack,
    Rolling,
    Die,
}


public abstract class Creature : MonoBehaviour, IHit
{
    public Stats stats = new Stats();

    public STATE state = STATE.Idle;

    public string s_name;

    public float finalMaxHp;
    public float curHp;
    public float hpRegen;

    public float finalAtk;
    public float finalDef;

    public abstract void Hit(float _damage);
}
