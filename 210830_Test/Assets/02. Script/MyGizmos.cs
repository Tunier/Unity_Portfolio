using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public enum Type {NORMAL,MONSTER,PLAYER,WAYPOINT,RESPAWN}
    public Type type = Type.NORMAL;

    public Color _color = Color.yellow;
    public float _radius = 0.1f;

    private void OnDrawGizmos()
    {
        switch (type)
        {
            case Type.NORMAL:
                Gizmos.color = _color;
                Gizmos.DrawSphere(transform.position, _radius);
                break;
        }
    }










}


