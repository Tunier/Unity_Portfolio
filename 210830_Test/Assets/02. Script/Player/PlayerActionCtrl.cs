using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionCtrl : MonoBehaviour
{
    PlayerInfo player;

    [SerializeField]
    GameObject wayPointUI;
    [SerializeField]
    Player_SkillIndicator skillIndicator;

    [SerializeField]
    GameObject[] skillprefebs;

    SkillDatabase skillDB;

    CharacterController cController;


    public GameObject monster;
    void Awake()
    {
        player = FindObjectOfType<PlayerInfo>();
        skillDB = FindObjectOfType<SkillDatabase>();
        cController = FindObjectOfType<CharacterController>();
    }

    void Update()
    {
        if (cController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                UseQuickSlotSkill(0);
            }

            if (skillIndicator.straightIndicator.activeSelf && Input.GetMouseButtonDown(0))
            {
                skillIndicator.straightIndicator.SetActive(false);
                skillDB.UseSkill(player.skillDic[0], gameObject); // ���߿� ��ų ���Կ� �ִ� ��ų�� �����ؾ���.
            }
        }
        TestAttack();
        TryAction();
    }

    void UseQuickSlotSkill(int _slotIndex) // ���߿� ��ų���������� �ȱ�.
    {
        // z Ű�� �ִ� ��ų�� ������ �޾Ƽ� �ε������͸� ų�� ����.
        // �ε������͸� ų �ʿ䰡 ������ �ٷ� UseSkill �ߵ�
        //if (quickSkillSlot[0].skill.SkillType == Skill.SkillType.NoneTarget)
        //{
        //    skillIndicator.straightIndicator.SetActive(true);
        //}
        //else
        //{
        //    skillDB.UseSkill(�����Կ� �ִ� ��ų �ְ�, gameObject, target);
        //}
        #region �׽�Ʈ �ڵ�
        skillIndicator.straightIndicator.SetActive(true);
        #endregion
    }

    void TestAttack()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            monster.GetComponent<MonsterAction>().Hit(10);
            Debug.Log("������10");
        }
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (UIManager.instance.hotKeyGuid.activeSelf)
            {
                if (UIManager.instance.wayPoints.Contains(UIManager.instance.hotKeyGuidTarget))
                {
                    wayPointUI.SetActive(true);
                }
            }
            else
            {
                wayPointUI.SetActive(false);
            }
            //else if(UIManager.instance.hotKeyGuid.activeSelf && merchants.Contains(UIManager.instance.hotKeyGuidTarget))
            //{
            //����uiŰ�� �κ��丮 Ű��
            //}
        }
    }
}
