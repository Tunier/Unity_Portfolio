using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBase : Creature
{
    int dropGold;

    public abstract void DropItem();
}