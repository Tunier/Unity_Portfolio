using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponCtrl : MonoBehaviour
{
    public PlayerInfo player;
    public GameObject curHitMob;
    public List<GameObject> mobList = new List<GameObject>();

    Collider col;

    void Awake()
    {
        col.enabled = false;
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MONSTER")) //���Ͱ� ������
        {
            curHitMob = other.gameObject;

            player.targetMonster = curHitMob; // ���� ���� ������ �÷��̾�� ����

            if (!mobList.Contains(other.gameObject)) // ���� ���Ͱ� ����Ʈ�� ������
            {
                mobList.Add(other.gameObject); // ���� ���͸� ����Ʈ�� �����ϰ�

                if (CritcalCal()) // ũ��Ƽ���� ������ ����ؼ� Hit�� ȣ��
                {
                    curHitMob.GetComponent<MonsterBase>().Hit(player.finalAtk * 1.5f);
                    // ������ UI ����ϴ� ���� �ۼ��ؾ���. ũ��Ƽ���� �߸� �ش� UI Text�� �÷��� �ٲ��ִ� ��ɵ� �߰��ؾ���.
                }
                else
                {
                    curHitMob.GetComponent<MonsterBase>().Hit(player.finalAtk);
                }
            }
            else { return; }
        }
    }

    public bool CritcalCal()
    {
        bool isCrit = false;
        int crit;

        crit = Random.Range(0, 10000);

        if (player.finalCriticalChance >= crit)
            isCrit = true;

        return isCrit;
    }
}
