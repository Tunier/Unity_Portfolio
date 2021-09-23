using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 몬스터가 원거리 공격일 경우 애니메이션 이벤트에서 호출
/// </summary>
public class MonsterFire : MonoBehaviour
{
    MonsterAnim monsterAnim;

    public Transform shotPos;   //화살 발사 위치

    private void Awake()
    {
        monsterAnim = GetComponent<MonsterAnim>();
    }

    public void MagicFire()
    {

    }

    public void ArrowShot()
    {
        //테스트 instantiate
        //var _arrow = Instantiate(arrow,transform.position, Quaternion.identity);

        //쏠때 파티클 제어문
        var _arrow = ObjPoolingManager.Instance.GetObjAtPool(ObjPoolingManager.Obj.GoblinHunterArrow);

        if (_arrow != null)
        {
            monsterAnim.OnAttack();
            _arrow.transform.position = shotPos.position;
            _arrow.transform.rotation = shotPos.rotation;
            _arrow.SetActive(true);
        }
        else
        {
            monsterAnim.OnMove(false, 0f);
        }
    }
}



