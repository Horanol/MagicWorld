using UnityEngine;
using System.Collections;

public class TempStateControl : MonoBehaviour
{
    public Animator anim;
    public CharacterController charControl;
    public float smoothness = 5.5f;
    public float movingSpeed = 4f;
    public float jumpSpeed = 13.0f;
    public float gravity = 20.0f;

    public Vector3 direction = Vector3.zero;
    public Transform hitPosition;
    public GameObject AttackingEffects;
    public PlayerStates currentState;

    public AnimatorStateInfo info;

    public void OnEnable()
    {
        Messenger<PlayerStates>.AddListener("SwitchCurrentState", SwitchCurrentState);
        Messenger<float, float>.AddListener("SetDirection", SetDirection);
    }

    public void OnDisable()
    {
        Messenger<PlayerStates>.RemoveListener("SwitchCurrentState", SwitchCurrentState);
        Messenger<float, float>.RemoveListener("SetDirection", SetDirection);
    }


    void Start()
    {
        anim = GetComponent<Animator>();
        charControl = GetComponent<CharacterController>();
        currentState = PlayerStates.idleState;

    }
    public void CheckState(float h,float v)
    {
        SetDirection(h, v);
        //若当前处于idle状态
        if (currentState == PlayerStates.idleState)
        {
            //判断方向输入是否为空
            if (direction != Vector3.zero)
            {
                //输入不为空则切换到行走状态
                SwitchCurrentState(PlayerStates.walkingState);
                DoCurrentState();
            }
            else
            {
                //若输入方向为空，则判断是否按下其他键
                if (Input.GetKeyDown(KeyCode.R))
                {
                    SwitchCurrentState(PlayerStates.shoutingState);
                    DoCurrentState();
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    SwitchCurrentState(PlayerStates.bigAttackingState1);
                    DoCurrentState();
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    SwitchCurrentState(PlayerStates.normalAttackingState1);
                    DoCurrentState();
                }
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    SwitchCurrentState(PlayerStates.normalAttackingState2);
                    DoCurrentState();
                }
                else if (Input.GetKeyDown(KeyCode.V))
                {
                    SwitchCurrentState(PlayerStates.normalAttackingState3);
                    DoCurrentState();
                }
            }
          }
        //若当前处于walking状态
        else if (currentState == PlayerStates.walkingState)
        {
            //若输入方向为零
            if (direction == Vector3.zero)
            {
                //切换到idle
                SwitchCurrentState(PlayerStates.idleState);
            }
            DoCurrentState();
        }
    }
    void Update()
    {
        if (Time.frameCount % 50 == 0)
        {
            System.GC.Collect();//定期垃圾回收
        }

        if (Time.frameCount % 6 == 0)
        {
            //若处于攻击或者呐喊状态,获取当前动画信息
            info = anim.GetCurrentAnimatorStateInfo(0);
            //判断动画是否已经切换到idle动画了，注意不能用normalizedTime判断，因为还没播放完就切换到idle了
            //还有要加上条件不能处于动画过渡期间！！！
            if (info.IsName(names.baseLayer_Idle) && !anim.IsInTransition(0))
            {
                //切换状态为idle
                SwitchCurrentState(PlayerStates.idleState);
                DoCurrentState();
            }
        }

    }
    void DoCurrentState()
    {
        switch (currentState)
        {
            case PlayerStates.idleState:
                anim.SetBool("isWalking", false);
                break;
            case PlayerStates.walkingState:
                anim.SetBool("isWalking", true);
                RotateBody();
                MoveTowards();
                break;
            case PlayerStates.shoutingState:
                anim.SetTrigger("shout");
                break;
            case PlayerStates.bigAttackingState1:
                anim.SetTrigger("bigAttack1");
                break;
            case PlayerStates.normalAttackingState1:
                anim.SetTrigger("normalAttack1");
                break;
            case PlayerStates.normalAttackingState2:
                anim.SetTrigger("normalAttack2");
                break;
            case PlayerStates.normalAttackingState3:
                anim.SetTrigger("normalAttack3");
                break;
        }
    }
    void SwitchCurrentState(PlayerStates changeState)
    {
        //若从idle切换到战斗状态，则激活刀光
        if (currentState == PlayerStates.idleState)
        {
            if (changeState != PlayerStates.walkingState)
            {
                AttackingEffects.SendMessage(names.SetLeftTrailEffect, true);
                AttackingEffects.SendMessage(names.SetRightTrailEffect, true);
            }
        }
        //若从战斗状态切换到idle状态，则失活刀光
        else if (changeState == PlayerStates.idleState)
        {
            if (currentState != PlayerStates.walkingState)
            {
                AttackingEffects.SendMessage(names.SetLeftTrailEffect, false);
                AttackingEffects.SendMessage(names.SetRightTrailEffect, false);
            }
        }
        currentState = changeState;
    }
    void SetDirection(float h, float v)
    {
        direction = new Vector3(h, 0.0f, v);
        direction.Normalize();
    }
    void RotateBody()
    {
        if (direction != Vector3.zero)//当方向不为空时，旋转
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothness);
        }
    }
    void MoveTowards()
    {
        Vector3 moveDirection = direction * movingSpeed;
        if (charControl.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;//设置跳起距离	
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;//模拟重力
        charControl.Move(moveDirection * Time.deltaTime);//计算一帧的移动距离并移动
    }




}
