using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    //필요한 이미지
    [SerializeField]
    private Image[] images_Gauge;
    //각상태를 대표하는 인덱스
    private const int HP = 0, MP = 1, EXP = 2;

    int curHp, curMp, curExp;
    int maxHp;
    int maxMp;

    public int level = 1;
    public int nextExp = 100; //일단은 100으로 설정 나중에 레벨별 경험치로 변경
    public Text expTxt;

    private void Start()
    {
        maxHp = 100;
        maxMp = 100;
        curHp = maxHp;
        curMp = maxMp;
        curExp = 0;
    }

    private void Update()
    {
        GuageUpdate();
        GetExp();
    }

    private void GuageUpdate()
    {
        images_Gauge[HP].fillAmount = (float)curHp / maxHp;
        images_Gauge[MP].fillAmount = (float)curMp / maxMp;
        images_Gauge[EXP].fillAmount = (float)curExp / nextExp;
        expTxt.text = "EXP " + curExp + " / " + nextExp;
    }

    //포션효과 
    //아이템이나 스킬타입이 생기면 파라메터에 타입을 넣어서 increse별 decrease별로 합치기
    public void IncreaseHp(int _count)
    {
        if (curHp + _count < maxHp)
            curHp += _count;
        else
            curHp = maxHp;
    }

    public void IncreaseMp(int _count)
    {
        if (curMp + _count < maxMp)
            curMp += _count;
        else
            curMp = maxMp;

    }

    //경험치 얻기 (test)
    public void GetExp()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            IncreaseExp(10);
            Debug.Log("경험치 10 얻었습니다.");
        }
    }

    public void IncreaseExp(int _count)
    {
        if (curExp + _count <= nextExp)
            curExp += _count;
        if (curExp + _count > nextExp)
        {
            //레벨업 시스템을 넣으면 레벨업 함수를 넣고 curExp를 초기화해주고 next레벨을 높여줌
            LevelUp();
            Debug.Log("레벨업");//레벨업 함수
        }
    }

    //데미지를 받는 경우(공격,독 등)에 호출
    public void DecreaseHp(int _count)
    {
        //방어력이 있을경우 그냥 빼지않고 방어력만큼 차감한 후 데미지를 준다.
        curHp -= _count;
        Debug.Log("캐릭터의 체력이 10감소 했습니다.");

        //죽을경우
        if (curHp <= 0)
            Debug.Log("캐릭터의 체력이 0이 되었습니다.");
        //죽는애니메이션 재생, 게임종료 문구-UI출력,적들 상태기본으로 변경
    }

    public void DecreaseMp(int _count)
    {
        curMp -= _count;
        Debug.Log("캐릭터의 마력이 10 감소 되었습니다.");
        if (curMp <= 0)
            Debug.Log("캐릭터의 마력이 0이 되엇습니다.");
    }
    void LevelUp()
    {
        curExp -= nextExp;
        nextExp = level * 100 + 100;
        level++;
        maxHp += 10;//임시 수치
        maxMp += 10;
        curHp = maxHp;
        curMp = maxMp;

    }


}
