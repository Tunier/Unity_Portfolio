using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 몬스터가 원거리 공격일 경우 애니메이션 이벤트에서 호출
/// </summary>
public class MonsterFire : MonoBehaviour
{
    MonsterAnim monsterAnim;

    public GameObject arrow;    //화살 프리팹
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
        monsterAnim.OnAttack();

        //쏠때 파티클 제어문

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



