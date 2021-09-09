using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponCtrl : MonoBehaviour
{
    public PlayerInfo player;

    Collider col;

    public GameObject curHitMob;
    public List<GameObject> mobList = new List<GameObject>();

    void Awake()
    {
        col = GetComponent<Collider>();
        col.enabled = false;
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster")) //���Ͱ� ������
        {
            curHitMob = other.gameObject;

            if (!mobList.Contains(curHitMob)) // ���� ���Ͱ� ����Ʈ�� ������
            {
                mobList.Add(curHitMob); // ���� ���͸� ����Ʈ�� �����ϰ�

                if (CritcalCalculate()) // ũ��Ƽ���� ������ ����ؼ� Hit�� ȣ��
                {
                    curHitMob.GetComponent<MonsterBase>().Hit(player.finalNormalAtk * 1.5f);
                    // ������ UI ����ϴ� ���� �ۼ��ؾ���. ũ��Ƽ���� �߸� �ش� UI Text�� �÷��� �ٲ��ִ� ��ɵ� �߰��ؾ���.
                }
                else
                {
                    curHitMob.GetComponent<MonsterBase>().Hit(player.finalNormalAtk);
                }
            }
            else { return; }
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
