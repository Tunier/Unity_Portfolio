using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasRotCtrl : MonoBehaviour
{
    GameObject CameraArm;

    private void Awake()
    {
        CameraArm = GameObject.Find("CameraArm");
    }

    void Update()
    {
        Vector3 Rot = new Vector3(0, CameraArm.transform.eulerAngles.y, 0);
        transform.eulerAngles = Rot;
    }
}
