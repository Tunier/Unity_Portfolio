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
        if (other.CompareTag("Monster")) //���Ͱ� ������
        {
            curHitMob = other.gameObject;

            if (!mobList.Contains(curHitMob)) // ���� ���Ͱ� ����Ʈ�� ������
            {
                mobList.Add(curHitMob); // ���� ���͸� ����Ʈ�� �����ϰ�

                if (CritcalCalculate()) // ũ��Ƽ���� ������ ����ؼ� Hit�� ȣ��
                {
                    curHitMob.GetComponent<MonsterBase>().Hit((SkillDatabase.instance.AllSkillDic["0300005"].Value + (player.player_Skill_Dic["0300005"] - 1)
                                                                * SkillDatabase.instance.AllSkillDic["0300005"].ValueFactor) * 1.5f);
                    // ������ UI ����ϴ� ���� �ۼ��ؾ���. ũ��Ƽ���� �߸� �ش� UI Text�� �÷��� �ٲ��ִ� ��ɵ� �߰��ؾ���.
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
