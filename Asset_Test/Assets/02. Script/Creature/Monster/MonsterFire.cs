using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ���Ͱ� ���Ÿ� ������ ��� �ִϸ��̼� �̺�Ʈ���� ȣ��
/// </summary>
public class MonsterFire : MonoBehaviour
{
    MonsterAnim monsterAnim;

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
        //�׽�Ʈ instantiate
        //var _arrow = Instantiate(arrow,transform.position, Quaternion.identity);

        //�� ��ƼŬ ���
        var _arrow = ObjPoolingManager.Instance.GetObjAtPool(ObjPoolingManager.Obj.GoblinHunterArrow);
        _arrow.transform.position = shotPos.position;
        _arrow.transform.rotation = shotPos.rotation;
        _arrow.SetActive(true);
    }
}



