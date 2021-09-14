using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonsterBase
{
    public override void Die()
    {
        Debug.Log("»ç¸Á");
    }

    public override void DropItem()
    {
        throw new System.NotImplementedException();
    }

    public override void Hit(float _damage)
    {
        curHp -= _damage;
        print(_damage);

        if (curHp <= 0)
            Die();
    }

    void Start()
    {
        finalMaxHp = 50;
        curHp = finalMaxHp; 
    }

    void Update()
    {
        
    }
}
