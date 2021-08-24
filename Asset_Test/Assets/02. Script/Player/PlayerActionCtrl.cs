using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionCtrl : MonoBehaviour
{
    PlayerInfo player;

    [SerializeField]
    Player_SkillIndicator skillIndicator;

    [SerializeField]
    GameObject[] skillprefebs;

    SkillDatabase skillDB;

    CharacterController cController;

    [SerializeField]
    GameObject inventoryUI;
    [SerializeField]
    GameObject skilltreeUI;

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
                skillDB.UseSkill(skillDB.AllSkillDic["0300000"], gameObject); // 나중에 스킬 슬롯에 있는 스킬로 변경해야함.
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            skilltreeUI.SetActive(!skilltreeUI.activeSelf);
        }
    }

    void UseQuickSlotSkill(int _slotIndex) // 나중에 스킬퀵슬롯으로 옴김.
    {
        // z 키에 있는 스킬의 종류를 받아서 인디케이터를 킬지 정함.
        // 인디케이터를 킬 필요가 없으면 바로 UseSkill 발동
        //if (quickSkillSlot[0].skill.type == Skill.SkillType.NoneTarget)
        //{
        //    skillIndicator.straightIndicator.SetActive(true);
        //}
        //else
        //{
        //    skillDB.UseSkill(퀵슬롯에 있는 스킬 넣고, gameObject, target);
        //}
        #region 테스트 코드
        skillIndicator.straightIndicator.SetActive(true);
        #endregion
    }
}
