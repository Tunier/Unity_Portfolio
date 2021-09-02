using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IHit
{
    public void Hit(float _damage);
}

interface IDie
{
    public void Die();
}

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

public enum STATE
{
    Idle,
    Walk,
    Run,
    Patrol,
    Chase,
    Attacking,
    Backoff,
    Jump,
    JumpAndAttack,
    Rolling,
    Die,
}

public abstract class Creature : MonoBehaviour, IHit, IDie
{
    public STATE state { get; set; } = STATE.Idle;
    public Stats stats { get; set; } = new Stats();

    public float finalMaxHp { get; protected set; }
    public float curHp;
    
    public float finalHpRegen;

    public float finalAtk { get; protected set; }
    public float finalDef { get; protected set; }

    public abstract void Hit(float _damage);

    public abstract void Die();
}