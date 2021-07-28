using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float walkMoveSpeed = 5f;
    float runMoveSpeed;
    float backMoveSpeed;
    float jumpForce = 9f;
    KeyCode jumpKeyCode = KeyCode.Space;
    float gravity = -9.81f;

    float speed = 0f;

    [SerializeField]
    GameObject cameraArm;

    Vector3 moveDirection;

    CharacterController cController;
    Animator ani;

    KeyCode runKeyCode = KeyCode.LeftShift;
    [SerializeField]
    bool isRun = false;

    readonly int hashSpeed = Animator.StringToHash("Speed_f");
    readonly int hashJump = Animator.StringToHash("Jump_b");

    void Awake()
    {
        cController = GetComponent<CharacterController>();
        ani = GetComponent<Animator>();

        isRun = false;
    }

    void Update()
    {
        runMoveSpeed = walkMoveSpeed * 2f;
        backMoveSpeed = walkMoveSpeed * 0.85f;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (!cController.isGrounded)
            moveDirection.y += gravity * Time.deltaTime;
        else
            ani.SetBool(hashJump, false);

        if (Input.GetKeyDown(jumpKeyCode) && cController.isGrounded)
        { 
            moveDirection.y = jumpForce;
            ani.SetBool(hashJump, true);
        }

        if (Input.GetKeyDown(runKeyCode))
            isRun = !isRun;

        if (x != 0 || z != 0)
        {
            Vector3 camArmRot = new Vector3(0, cameraArm.transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Euler(camArmRot);
        }

        if (x * z == 1)
            transform.eulerAngles += new Vector3(0, 45, 0);
        else if (x * z == -1)
            transform.eulerAngles += new Vector3(0, -45, 0);

        setMoveDir(x, z);

        cController.Move(moveDirection * Time.deltaTime);
    }

    void setMoveDir(float _x, float _z)
    {
        if (_z > 0.25f)
        {
            if (!isRun)
                speed = walkMoveSpeed;
            else
                speed = runMoveSpeed;

            moveDirection = new Vector3(transform.forward.x * speed, moveDirection.y, transform.forward.z * speed);
        }
        else if (_z < -0.25f)
        {
            speed = backMoveSpeed;
            moveDirection = new Vector3(-transform.forward.x * speed, moveDirection.y, -transform.forward.z * speed);
        }
        else if (_x > 0.25f)
        {
            speed = walkMoveSpeed;
            moveDirection = new Vector3(transform.right.x * speed, moveDirection.y, transform.right.z * speed);
        }
        else if (_x < -0.25f)
        {
            speed = walkMoveSpeed;
            moveDirection = new Vector3(-transform.right.x * speed, moveDirection.y, -transform.right.z * speed);
        }
        else if (_x == 0 && _z == 0)
        {
            speed = 0;
            moveDirection = new Vector3(0, moveDirection.y, 0);
        }

        if (speed == runMoveSpeed)
            ani.SetFloat(hashSpeed, 1f);
        else if (speed == walkMoveSpeed)
            ani.SetFloat(hashSpeed, 0.4f);
        else if (speed == backMoveSpeed)
            ani.SetFloat(hashSpeed, 0.4f);
        else
            ani.SetFloat(hashSpeed, 0);
    }
}
