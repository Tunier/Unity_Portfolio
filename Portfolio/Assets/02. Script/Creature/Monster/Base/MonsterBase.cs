using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBase : Creature
{
    public int dropGold { get; protected set; }
    public float exp { get; protected set; }

    public bool isAnger { get; set; } = false;        // 선공 : Anger true 후공 : Anger false

    public GameObject playerGo;
    public GameObject minimapCube;
    public GameObject hpCanvas;

    public GameObject group;            //몬스터별 무브포인트 기준 파일 넣어주기
    public List<Transform> movePoints;  // 무브포인트
    public MonsterSpawner spawner;      // 본인이 스폰된 스포너

    public abstract void DropItem();
}