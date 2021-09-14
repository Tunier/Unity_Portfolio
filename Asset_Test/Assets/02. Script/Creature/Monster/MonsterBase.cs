using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBase : Creature
{
    public int dropGold { get; protected set; }

    public abstract void DropItem();
}