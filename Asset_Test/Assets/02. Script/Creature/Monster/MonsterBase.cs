using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBase : Creature
{
    public int dropGold { get; protected set; }

    public bool isAnger { get; set; } = false;        //선공 : Anger true 후공 : Anger false

    public abstract void DropItem();
}