using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCube : MonoBehaviour
{
    public Transform target;
   

    void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, this.transform.position.y, target.transform.position.z);
    }
}
