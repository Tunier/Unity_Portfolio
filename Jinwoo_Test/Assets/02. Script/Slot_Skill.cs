using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot_Skill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Skill skill;
    public float coolTime;
    public float curCoolTime;
    public Image skillImage;
    
    [SerializeField]
    GameObject cooldownImage;
    [SerializeField]
    Text cooltimeTxt;
    [SerializeField]
    RectTransform quickSlot;
    [SerializeField]
    ToolTip_Skill toolTip;

    Image cooldown;
    void Awake()
    {
        cooldown = GetComponent<Image>();
    }

    void Update()
    {
        SetCoolDown();
    }
    //색의 알파값을 받아서 변경 (0 or0.6)
    void SetColorAlpha(float alpha)
    {
        Color color = skillImage.color;
        color.a = alpha;
        skillImage.color = color;
    }
    /// <summary>
    /// 스킬 슬롯에 스킬을 넣어주고, 이미지를 스킬 이미지로 바꿔주고, 해당 스킬쿨타임과 현재 쿨타임을 넘겨줌.
    /// </summary>
    /// <param name="_skill"></param>
    /// <param name="_curCooltime"></param>
    public void AddSkillicon(Skill _skill,float _curCooltime)
    {
        skill = _skill;
        skillImage.sprite = _skill.skillImage;
        coolTime = _skill.coolTime;
        curCoolTime = _curCooltime;

        //해당 스킬이 쿨타임 이라면 쿨타임 이미지를 active 시키고
        //쿨타임을 표시 되게 하는 기능 넣어야함.

        SetColorAlpha(0.6f);
    }
    //슬롯에 스킬을 옮기거나 빈칸으로 만들시에
    void ClearSlot()
    {
        skill = null;
        coolTime = 0f;
        curCoolTime = 0f;
        skillImage.sprite = null;
        SetColorAlpha(0);

        cooltimeTxt.text = "0";
        cooldownImage.SetActive(false);
    }
    void SetCoolDown()
    {
        if (curCoolTime >= 0f)
        {
            cooldownImage.SetActive(true);
            curCoolTime -= Time.deltaTime;
        }
        else if (curCoolTime < 0f)
        {
            cooldownImage.SetActive(false);
            curCoolTime = 0f;
        }
        cooltimeTxt.text = Mathf.Ceil(curCoolTime).ToString();
        cooldown.fillAmount = curCoolTime / coolTime;
    }
    //마우스 드래그를 시작했을때 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
    }
    //마우스 드래그 중인 동안 계속 호출
    public void OnDrag(PointerEventData eventData)
    {
    }
    //드롭 된 무언가가 있을 때 호출 *OnDrop이 OnEndDrag보다 먼저 실행된다.
    public void OnDrop(PointerEventData eventData)
    {
    }
    //마우스 드래그 하는 것을 끝냈을 때 호출
    public void OnEndDrag(PointerEventData eventData)
    {
    }
    //마우스포인터가 충돌범위안에 들어 올때 들어오는 이벤트
    public void OnPointerEnter(PointerEventData eventData)
    {
    }
    //마우스의 포인터가 충돌범위밖으로 나갈 때 들어오는 이벤트
    public void OnPointerExit(PointerEventData eventData)
    {
    }

}
