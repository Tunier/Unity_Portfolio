using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPoolingManager : MonoSingletone<ObjPoolingManager>
{
    public enum Monster
    {
        Pig,
        GolinHunter,
        SkeltonWarrior,
    }

    public GameObject go_monsterPig;
    public GameObject go_monsterGoblinHunter;
    public GameObject go_monsterSkeletonWarrior;

    int maxcount = 40;

    public List<GameObject> pigList = new List<GameObject>();
    public List<GameObject> goblinHunterList = new List<GameObject>();
    public List<GameObject> skeletonWarriorList = new List<GameObject>();

    private void Awake()
    {
        CreateMonsterPool(go_monsterPig, maxcount);
        CreateMonsterPool(go_monsterGoblinHunter, maxcount);
        CreateMonsterPool(go_monsterSkeletonWarrior, maxcount);
    }

    void CreateMonsterPool(GameObject _monster, int _count)
    {
        var monsterPool = new GameObject(_monster.name + "Pool");

        for (int i = 0; i < _count; i++)
        {
            var obj = Instantiate(_monster);
            obj.SetActive(false);
            obj.name = _monster.name + " " + (i + 1).ToString("00");
            obj.transform.SetParent(monsterPool.transform);

            if (obj.name.Contains("Pig"))
            {
                pigList.Add(obj);
            }
            else if (obj.name.Contains("Goblin_Hunter"))
            {
                goblinHunterList.Add(obj);
            }
            else if (obj.name.Contains("Skeleton_Warrior"))
            {
                skeletonWarriorList.Add(obj);
            }
        }
    }

    public GameObject GetMonsterAtPool(Monster _monster)
    {
        switch (_monster)
        {
            case Monster.Pig:
                foreach (var obj in pigList)
                {
                    if (!obj.activeSelf)
                        return obj;
                }
                return null;
            case Monster.GolinHunter:
                foreach (var obj in goblinHunterList)
                {
                    if (!obj.activeSelf)
                        return obj;
                }
                return null;
            case Monster.SkeltonWarrior:
                foreach (var obj in skeletonWarriorList)
                {
                    if (!obj.activeSelf)
                        return obj;
                }
                return null;
            default:
                Debug.LogError("없는몬스터를 받아가려고함.");
                return null;
        }
    }
}