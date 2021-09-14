using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerActionCtrl : MonoBehaviour
{
    public Collider weaponCol;
    PlayerWeaponCtrl playerWP;
    [SerializeField]
    GameObject AttackEffect1;
    [SerializeField]
    GameObject AttackEffect2;

    PlayerInfo player;
    SkillDatabase skillDB;
    Tooltip tooltip;
    [SerializeField]
    GameObject cameraArm;

    public RectTransform inventoryRect;
    public RectTransform skillTreeRect;

    [SerializeField]
    Player_SkillIndicator skillIndicator;

    [SerializeField]
    GameObject wayPointUI;
    [SerializeField]
    GameObject inventoryUI;
    [SerializeField]
    GameObject skilltreeUI;
    [SerializeField]
    GameObject statsUI;

    public GameObject QuickSkillSlotParents;
    public GameObject QuickPotionSlotParents;

    CharacterController cController;
    Animator ani;

    public List<SkillSlot> skillSlot = new List<SkillSlot>();
    public Dictionary<string, float> curSkillCooltime = new Dictionary<string, float>();
    List<string> keys = new List<string>();

    public List<Slot> potionSlot = new List<Slot>();

    SkillSlot readySkillSlot = null;
    Skill readySkill = null;

    public bool isUsingSkill = false;

    public bool isWhirlwind = false;
    public bool isSwordSkill2 = false;
    readonly int hashWhirlwind = Animator.StringToHash("IsWhirlwind");
    readonly int hashIsAttack = Animator.StringToHash("IsAttack");
    readonly int hashSpeed = Animator.StringToHash("Speed_f");
    readonly int hashJump = Animator.StringToHash("Jump_b");
    readonly int hashSwordSkill2 = Animator.StringToHash("UseSwordSkill2");

    void Awake()
    {
        playerWP = FindObjectOfType<PlayerWeaponCtrl>();
        player = FindObjectOfType<PlayerInfo>();
        skillDB = FindObjectOfType<SkillDatabase>();
        tooltip = FindObjectOfType<Tooltip>();

        cController = FindObjectOfType<CharacterController>();
        ani = GetComponent<Animator>();

        skillSlot.AddRange(QuickSkillSlotParents.GetComponentsInChildren<SkillSlot>());
        potionSlot.AddRange(QuickPotionSlotParents.GetComponentsInChildren<Slot>());

        StartCoroutine(StateCheck());
    }

    private void Start()
    {
        weaponCol.enabled = false;

        keys.AddRange(player.player_Skill_Dic.Keys);

        foreach (var key in keys)
        {
            curSkillCooltime.Add(key, 0f);
        }

    }

    void Update()
    {
        if (cController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                UseQuickSlotSkill(0);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                UseQuickSlotSkill(1);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                UseQuickSlotSkill(2);
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                UseQuickSlotSkill(3);
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                UseQuickSlotSkill(4);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                UseQuickSlotSkill(5);
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                UseQuickSlotSkill(6);
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                UseQuickSlotSkill(7);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                potionSlot[0].UseItem(potionSlot[0].item);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                potionSlot[1].UseItem(potionSlot[1].item);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                potionSlot[2].UseItem(potionSlot[2].item);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                potionSlot[3].UseItem(potionSlot[3].item);
            }
            else if (skillIndicator.straightIndicator.activeSelf && Input.GetMouseButtonDown(0))
            {
                skillIndicator.straightIndicator.SetActive(false);
                skillDB.UseSkill(readySkill, gameObject, null, readySkillSlot);
                readySkillSlot = null;
                readySkill = null;
                isUsingSkill = false;
            }
            else if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                ani.SetBool(hashIsAttack, false);
            }
            else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 camArmRot = new Vector3(0, cameraArm.transform.eulerAngles.y, 0);
                transform.rotation = Quaternion.Euler(camArmRot);

                player.state = STATE.Attacking;
                ani.SetBool(hashIsAttack, true);
            }

        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            if (!inventoryUI.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, Input.mousePosition))
            {
                tooltip.HideTooltip();
            }
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            skilltreeUI.SetActive(!skilltreeUI.activeSelf);
            if (statsUI.activeSelf)
                statsUI.SetActive(false);

            if (!skilltreeUI.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(skillTreeRect, Input.mousePosition))
            {
                tooltip.HideTooltip();
            }
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            statsUI.SetActive(!statsUI.activeSelf);
            if (skilltreeUI.activeSelf)
                skilltreeUI.SetActive(false);
        }
        

        ani.SetBool(hashWhirlwind, isWhirlwind);

        if (isWhirlwind)
        {
            ani.SetFloat("Speed_f", 0);
            isUsingSkill = true;
        }

        if (isSwordSkill2)
        {
            ani.SetTrigger(hashSwordSkill2);
            player.state = STATE.Attacking;
            isSwordSkill2 = false;
            isUsingSkill = true;
        }

        for (int i = 0; i < curSkillCooltime.Count; i++)
        {
            if (curSkillCooltime[keys[i]] > 0)
                curSkillCooltime[keys[i]] -= Time.deltaTime;
            else
                curSkillCooltime[keys[i]] = 0;
        }

        TryAction();
    }

    void UseQuickSlotSkill(int _slotIndex) // 나중에 스킬퀵슬롯으로 옴김.
    {
        Skill _skill = skillSlot[_slotIndex].skill;
        SkillSlot _skillSlot = skillSlot[_slotIndex];
        if (_skillSlot.haveSkill)
        {
            if (isUsingSkill == false && player.state != STATE.Attacking)
            {
                if (_skillSlot.curCooltime <= 0)
                {
                    switch (_skill.CostType)
                    {
                        case 0:
                            // z 키에 있는 스킬의 종류를 받아서 인디케이터를 킬지 정함.
                            // 인디케이터를 킬 필요가 없으면 바로 useskill 발동
                            if (_skill.Type == 1)
                            {
                                skillIndicator.straightIndicator.SetActive(true);
                                readySkillSlot = _skillSlot;
                                readySkill = _skillSlot.skill;
                                isUsingSkill = true;
                            }
                            else
                            {
                                SkillDatabase.instance.UseSkill(_skill, gameObject);
                            }
                            break;

                        case 1:
                            if (player.curHp >= _skill.Cost)
                            {
                                if (_skill.Type == 1)
                                {
                                    skillIndicator.straightIndicator.SetActive(true);
                                    readySkillSlot = _skillSlot;
                                    readySkill = _skillSlot.skill;
                                    isUsingSkill = true;
                                }
                                else
                                {
                                    SkillDatabase.instance.UseSkill(_skill, gameObject, null, _skillSlot);
                                }
                            }
                            else
                            {
                                print("마나가 부족합니다.");
                            }
                            break;

                        case 2:
                            if (player.curMp >= _skill.Cost)
                            {
                                if (_skill.Type == 1)
                                {
                                    skillIndicator.straightIndicator.SetActive(true);
                                    readySkillSlot = _skillSlot;
                                    readySkill = _skillSlot.skill;
                                    isUsingSkill = true;
                                }
                                else
                                {
                                    SkillDatabase.instance.UseSkill(_skill, gameObject, null, _skillSlot);
                                }
                            }
                            else
                            {
                                print("마나가 부족합니다.");
                            }
                            break;
                    }
                }
                else
                {
                    print("스킬이 쿨타임입니다.");
                }
                #region 테스트 코드
                //skillIndicator.straightIndicator.SetActive(true);
                #endregion
            }
            else
            {
                print("다른 동작중입니다.");
            }
        }
    }

    IEnumerator StateCheck()
    {
        while (true)
        {
            switch (player.state)
            {
                case STATE.Idle:
                    ani.SetBool(hashIsAttack, false);
                    ani.SetBool(hashJump, false);
                    ani.SetFloat(hashSpeed, 0);
                    break;
                case STATE.Attacking:
                    //ani.SetBool(hashIsAttack, true);
                    ani.SetBool(hashJump, false);
                    ani.SetFloat(hashSpeed, 0);
                    break;
                case STATE.Walk:
                    ani.SetFloat(hashSpeed, 0.5f);
                    ani.SetBool(hashIsAttack, false);
                    ani.SetBool(hashJump, false);
                    break;
                case STATE.Run:
                    ani.SetFloat(hashSpeed, 1f);
                    ani.SetBool(hashIsAttack, false);
                    ani.SetBool(hashJump, false);
                    break;
                case STATE.Backoff:
                    ani.SetFloat(hashSpeed, 0.5f);
                    ani.SetBool(hashIsAttack, false);
                    ani.SetBool(hashJump, false);
                    break;
                case STATE.Jump:
                    ani.SetBool(hashJump, true);
                    ani.SetBool(hashIsAttack, false);
                    break;
                case STATE.Hit:
                    // 히트모션 동작하게 해야함.
                    break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (UIManager.Instance.hotKeyGuid.activeSelf)
            {
                if (UIManager.Instance.wayPoints.Contains(UIManager.Instance.hotKeyGuidTarget))
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
            //상점ui키고 인벤토리 키기
            //}
        }
    }

    public void EventEndAttack()
    {
        ClearMobList();

        if (!Input.GetMouseButton(0))
            player.state = STATE.Idle;
    }

    public void WeaponColCtrl()
    {
        weaponCol.enabled = !weaponCol.enabled;
    }

    public void WeaponRotCtrl1()
    {
        weaponCol.gameObject.transform.localEulerAngles = new Vector3(90, 90, 90);
    }

    public void WeaponRotCtrl2()
    {
        weaponCol.gameObject.transform.localEulerAngles = new Vector3(45, 90, 90);
    }

    public void ClearMobList()
    {
        playerWP.mobList.Clear();
    }

    public void OnOffAttackEffect()
    {
        AttackEffect1.SetActive(!AttackEffect1.activeSelf);
    }

    public void OnOffAttackEffect2()
    {
        AttackEffect2.SetActive(!AttackEffect2.activeSelf);
    }

    public void SwordSkill2End()
    {
        player.state = STATE.Idle;
        isUsingSkill = false;
    }
}
