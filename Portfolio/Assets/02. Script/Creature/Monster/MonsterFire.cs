using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 몬스터가 원거리 공격일 경우 애니메이션 이벤트에서 호출
/// </summary>
public class MonsterFire : MonoBehaviour
{
    [SerializeField] Transform shotPos;   //화살 발사 위치

    public void SlashAttack()
    {
        var _slash = ObjPoolingManager.Instance.GetObjAtPool(ObjPoolingManager.Obj.GoblinKingSlah);
        _slash.GetComponent<SpecialAttackCtrl>().goblinKing = GetComponent<MonsterGoblinKing>();
        _slash.transform.position = shotPos.position;
        _slash.transform.eulerAngles = new Vector3(90, gameObject.transform.eulerAngles.y, 0);
        _slash.SetActive(true);
    }

    public void ArrowShot()
    {
        var _arrow = ObjPoolingManager.Instance.GetObjAtPool(ObjPoolingManager.Obj.GoblinHunterArrow);
        _arrow.GetComponent<ArrowCtrl>().hunter = GetComponent<MonsterHunter>();
        _arrow.transform.position = shotPos.position;
        _arrow.transform.rotation = shotPos.rotation;
        _arrow.SetActive(true);
    }
}