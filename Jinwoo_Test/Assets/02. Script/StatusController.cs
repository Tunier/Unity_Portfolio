using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    //�ʿ��� �̹���
    [SerializeField]
    private Image[] images_Gauge;
    //�����¸� ��ǥ�ϴ� �ε���
    private const int HP = 0, MP = 1, EXP = 2;

    int curHp, curMp, curExp;
    int maxHp;
    int maxMp;

    public int level = 1;
    public int nextExp = 100; //�ϴ��� 100���� ���� ���߿� ������ ����ġ�� ����
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

    //����ȿ�� 
    //�������̳� ��ųŸ���� ����� �Ķ���Ϳ� Ÿ���� �־ increse�� decrease���� ��ġ��
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

    //����ġ ��� (test)
    public void GetExp()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            IncreaseExp(10);
            Debug.Log("����ġ 10 ������ϴ�.");
        }
    }

    public void IncreaseExp(int _count)
    {
        if (curExp + _count <= nextExp)
            curExp += _count;
        if (curExp + _count > nextExp)
        {
            //������ �ý����� ������ ������ �Լ��� �ְ� curExp�� �ʱ�ȭ���ְ� next������ ������
            LevelUp();
            Debug.Log("������");//������ �Լ�
        }
    }

    //�������� �޴� ���(����,�� ��)�� ȣ��
    public void DecreaseHp(int _count)
    {
        //������ ������� �׳� �����ʰ� ���¸�ŭ ������ �� �������� �ش�.
        curHp -= _count;
        Debug.Log("ĳ������ ü���� 10���� �߽��ϴ�.");

        //�������
        if (curHp <= 0)
            Debug.Log("ĳ������ ü���� 0�� �Ǿ����ϴ�.");
        //�״¾ִϸ��̼� ���, �������� ����-UI���,���� ���±⺻���� ����
    }

    public void DecreaseMp(int _count)
    {
        curMp -= _count;
        Debug.Log("ĳ������ ������ 10 ���� �Ǿ����ϴ�.");
        if (curMp <= 0)
            Debug.Log("ĳ������ ������ 0�� �Ǿ����ϴ�.");
    }
    void LevelUp()
    {
        curExp -= nextExp;
        nextExp = level * 100 + 100;
        level++;
        maxHp += 10;//�ӽ� ��ġ
        maxMp += 10;
        curHp = maxHp;
        curMp = maxMp;

    }


}
