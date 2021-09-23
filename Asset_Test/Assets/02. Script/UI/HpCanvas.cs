using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpCanvas : MonoBehaviour
{
    GameObject CameraArm;

    [SerializeField]
    MonsterBase monster;
    [SerializeField]
    Image hpFill;

    private void Awake()
    {
        CameraArm = GameObject.Find("CameraArm");
    }

    void LateUpdate()
    {
        Vector3 Rot = new Vector3(0, CameraArm.transform.eulerAngles.y, 0);
        transform.eulerAngles = Rot;

        hpFill.fillAmount = monster.curHp / monster.finalMaxHp;
    }
}
