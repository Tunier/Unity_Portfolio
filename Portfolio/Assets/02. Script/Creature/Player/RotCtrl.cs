using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotCtrl : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject cameraArm;

    void Update()
    {
        transform.position = player.transform.position;
        transform.eulerAngles = new Vector3(0, cameraArm.transform.eulerAngles.y, 0);
    }
}
