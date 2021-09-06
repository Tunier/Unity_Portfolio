using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ���Ͱ� ���Ÿ� ������ ��� �ִϸ��̼� �̺�Ʈ���� ȣ��
/// </summary>
public class MonsterFire : MonoBehaviour
{
    MonsterAnim monsterAnim;

    public GameObject arrow;    //ȭ�� ������
    public Transform shotPos;   //ȭ�� �߻� ��ġ

    private void Awake()
    {
        monsterAnim = GetComponent<MonsterAnim>();
    }

    public void MagicFire()
    {

    }

    public void ArrowShot()
    {
        monsterAnim.OnAttack();

        //�� ��ƼŬ ���

        //var _arrow = WeaponManager.instance.GetArrow();
        var _arrow = Instantiate(arrow,transform.position, Quaternion.identity);
        if(_arrow != null)
        {
            _arrow.transform.position = shotPos.position;
            _arrow.transform.rotation = shotPos.rotation;
            _arrow.SetActive(true);
        }

    }
}



