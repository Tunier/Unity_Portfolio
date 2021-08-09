using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    GameObject cameraArm;
    Player_SkillIndicator pSkillIndicator;
    ClickEffect clickEffect;

    float walkMoveSpeed = 5f;
    float runMoveSpeed;
    float backMoveSpeed;
    float jumpForce = 9f;
    KeyCode jumpKeyCode = KeyCode.Space;
    float gravity = -9.81f;

    float speed = 0f;

    float x;
    float z;

    Vector3 moveDirection;

    CharacterController cController;
    NavMeshAgent nav;
    Animator ani;

    KeyCode runKeyCode = KeyCode.LeftShift;
    [SerializeField]
    bool isRun = false;
    public bool isMove = false;

    float pushTime = 0;

    readonly int hashSpeed = Animator.StringToHash("Speed_f");
    readonly int hashJump = Animator.StringToHash("Jump_b");

    Ray ray;
    public RaycastHit hit;

    void Awake()
    {
        pSkillIndicator = FindObjectOfType<Player_SkillIndicator>();
        clickEffect = FindObjectOfType<ClickEffect>();

        cController = GetComponent<CharacterController>();
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();

        isRun = false;
    }

    private void Start()
    {
        nav.enabled = false;
    }

    void Update()
    {
        #region 키보드로 제어하는 움직임 부분
        runMoveSpeed = walkMoveSpeed * 2f;
        backMoveSpeed = walkMoveSpeed * 0.85f;

        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

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

        if (!pSkillIndicator.straightIndicator.activeSelf)
        {
            if (x != 0 || z != 0)
            {
                Vector3 camArmRot = new Vector3(0, cameraArm.transform.eulerAngles.y, 0);
                transform.rotation = Quaternion.Euler(camArmRot);
            }

            if (x * z == 1)
                transform.eulerAngles += new Vector3(0, 45, 0);
            else if (x * z == -1)
                transform.eulerAngles += new Vector3(0, -45, 0);
        }

        if (cController.enabled)
        {
            setMoveDir(x, z);
            cController.Move(moveDirection * Time.deltaTime);
        }
        #endregion

        #region 캐릭터 제어권을 가지는 컴포넌트 변경 부분
        if (x != 0 || z != 0)
        {
            if (nav.enabled)
            {
                nav.isStopped = true;
                nav.ResetPath();

                nav.enabled = false;
                cController.enabled = true;
            }
        }
        #endregion

        #region 마우스 우클릭을 했을때 누르는 시간 체크
        if (Input.GetMouseButton(1))
        {
            pushTime += Time.deltaTime;

            if (pushTime >= 0.1f)
                isMove = false;
            else
                isMove = true;
        }
        #endregion

        if (Input.GetMouseButtonUp(1))
        {
            pushTime = 0;
            if (isMove)
            {
                nav.enabled = true;
                cController.enabled = false;

                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 6);

                Vector3 mousePos = new Vector3(hit.point.x, 0, hit.point.z);

                nav.SetDestination(mousePos);

                StopAllCoroutines();
                StartCoroutine(clickEffect.ClickEffectCtrl(new Vector3(hit.point.x, 1.1f, hit.point.z)));

                if (isRun)
                {
                    nav.speed = runMoveSpeed;
                    ani.SetFloat(hashSpeed, 1f);
                }
                else
                {
                    nav.speed = walkMoveSpeed;
                    ani.SetFloat(hashSpeed, 0.5f);
                }
            }
        }

        if (Vector3.Distance(nav.destination, transform.position) <= 0.1f)
        {
            nav.ResetPath();
            ani.SetFloat(hashSpeed, 0f);

            cController.enabled = true;
            nav.enabled = false;
        }
    }

    /// <summary>
    /// GetAxisRaw로 움직임을 받아서 움직이는 방향과 속도를 제어해줌.
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_z"></param>
    void setMoveDir(float _x, float _z)
    {
        if (_z == 1f)
        {
            if (!isRun)
                speed = walkMoveSpeed;
            else
                speed = runMoveSpeed;

            moveDirection = new Vector3(transform.forward.x * speed, moveDirection.y, transform.forward.z * speed);
        }
        else if (_z == -1)
        {
            speed = backMoveSpeed;
            moveDirection = new Vector3(-transform.forward.x * speed, moveDirection.y, -transform.forward.z * speed);
        }
        else if (_x == 1)
        {
            speed = walkMoveSpeed;
            moveDirection = new Vector3(transform.right.x * speed, moveDirection.y, transform.right.z * speed);
        }
        else if (_x == -1f)
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
            ani.SetFloat(hashSpeed, 0.5f);
        else if (speed == backMoveSpeed)
            ani.SetFloat(hashSpeed, 0.5f);
        else
            ani.SetFloat(hashSpeed, 0);
    }
}
