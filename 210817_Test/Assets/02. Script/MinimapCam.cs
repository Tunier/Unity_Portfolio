using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MinimapCam : MonoBehaviour, IPointerEnterHandler
{
    public float ZoomSpeed = 5f;
    public float MoveSpeed;
    public Transform Target;

    float Distance;

    private Vector3 AxisVec;
    private Vector3 Pos;
    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    void Start()
    {
        
    }

    void Update()
    {
        Zoom();
        Pos = transform.position;
        Pos.y = Target.position.y * 10; 
    }
    void Zoom()
    {
        Distance += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * -1;
        Distance = Mathf.Clamp(Distance, 5f, 15f);

        AxisVec = transform.forward * -1;
        AxisVec *= Distance;
        transform.position = transform.position + AxisVec;
    }
}
