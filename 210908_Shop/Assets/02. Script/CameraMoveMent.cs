using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveMent : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    PlayerMovement playerMvt;
    public Transform cameraArm;
    float camSpeed;
    float mouseX;
    float mouseY;

    CharacterController cController;

    Vector2 mouseDelta;

    private void Awake()
    {
        cController = FindObjectOfType<CharacterController>();
        playerMvt = FindObjectOfType<PlayerMovement>();

        camSpeed = 3.5f;

        cameraArm.position = player.transform.position;
    }

    void CamMove()
    {
        if (Input.GetMouseButton(1) && !playerMvt.wantMove)
        {
            mouseX = Input.GetAxis("Mouse X") * camSpeed;
            mouseY = Input.GetAxis("Mouse Y") * camSpeed;
            mouseDelta = new Vector2(mouseX, mouseY);
            Vector3 camAngle = cameraArm.rotation.eulerAngles;

            float x = camAngle.x - mouseDelta.y;

            if (x < 180f)
                x = Mathf.Clamp(x, -1f, 50f);
            else
                x = Mathf.Clamp(x, 355f, 361f);

            cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseX, camAngle.z);
        }
    }

    private void LateUpdate()
    {
        CamMove();

        cameraArm.position = Vector3.Lerp(cameraArm.position, player.transform.position, Time.deltaTime * 5);
    }
}
