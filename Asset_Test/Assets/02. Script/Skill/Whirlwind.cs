using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlwind : MonoBehaviour
{
    PlayerInfo player;

    PlayerActionCtrl playerAC;

    Vector3 pos;

    public GameObject curHitMob;
    public List<GameObject> mobList = new List<GameObject>();

    private void OnEnable()
    {
        player = FindObjectOfType<PlayerInfo>();
        playerAC = FindObjectOfType<PlayerActionCtrl>();

        StartCoroutine(StopMotion());
        StartCoroutine(ClearMobList());

        Destroy(gameObject, 5.2f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Monster")) //몬스터가 맞으면
        {
            curHitMob = other.gameObject;

            if (!mobList.Contains(curHitMob)) // 맞은 몬스터가 리스트에 없으면
            {
                mobList.Add(curHitMob); // 맞은 몬스터를 리스트에 저장하고

                if (CritcalCalculate()) // 크리티컬이 떴는지 계산해서 Hit를 호출
                {
                    curHitMob.GetComponent<MonsterBase>().Hit((SkillDatabase.instance.AllSkillDic["0300005"].Value + (player.player_Skill_Dic["0300005"] - 1)
                                                                * SkillDatabase.instance.AllSkillDic["0300005"].ValueFactor) * 1.5f);
                    // 데미지 UI 출력하는 구문 작성해야함. 크리티컬이 뜨면 해당 UI Text의 컬러를 바꿔주는 기능도 추가해야함.
                }
                else
                {
                    curHitMob.GetComponent<MonsterBase>().Hit(SkillDatabase.instance.AllSkillDic["0300005"].Value + (player.player_Skill_Dic["0300005"] - 1)
                                                               * SkillDatabase.instance.AllSkillDic["0300005"].ValueFactor);
                }
            }
            else { return; }
        }
    }

    void Update()
    {
        pos = new Vector3(player.transform.position.x, 1.5f, player.transform.position.z);

        transform.position = pos;
    }

    IEnumerator StopMotion()
    {
        yield return new WaitForSeconds(5f);

        playerAC.isWhirlwind = false;
    }

    IEnumerator ClearMobList()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.33f);

            mobList.Clear();
        }
    }

    public bool CritcalCalculate()
    {
        bool isCrit = false;
        int crit;

        crit = Random.Range(0, 10000);

        if (player.finalCriticalChance >= crit)
            isCrit = true;

        return isCrit;
    }
}
