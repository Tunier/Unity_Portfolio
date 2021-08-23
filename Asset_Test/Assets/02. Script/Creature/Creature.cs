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

public enum STATE
{
    Idle,
    Attacking,
    Jump,
    JumpAndAttack,
    Rolling,
    Die,
}

public abstract class Creature : MonoBehaviour, IHit, IDie
{
    public STATE state = STATE.Idle;

    public float finalMaxHp;
    public float curHp;
    public float hpRegen;

    public float finalAtk;
    public float finalDef;

    public abstract void Hit(float _damage);

    public abstract void Die();
}