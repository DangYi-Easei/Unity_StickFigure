using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Player : Entity
{
    private static Player instance;
    public static Player Instance => instance;

    public bool isBusy { get; private set; }

    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce;
    // 当前水平移动速度（受加速度/最大速度限制）
    public float currentXSpeed;
    public float acceleration_X = 12f;

    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;
    public float attackOneTimeBusyTime = 0.15f;

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    //public PlayerAirState airState { get; private set; }
    //public PlayerWallSlideState wallSlide { get; private set; }
    //public PlayerWallJumpState wallJump { get; private set; }
    //public PlayerDashState dashState { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    //public PlayerCounterAttackState counterAttack { get; private set; }
    //public PlayerDeadState deadState { get; private set; }

    #endregion


    
    protected override void Awake()
    {
        base.Awake();

        instance = this;

        stateMachine = new PlayerStateMachine();
        
        idleState = new PlayerIdleState(this,stateMachine,"Idle",E_PlayerStateType.Ground);
        moveState = new PlayerMoveState(this, stateMachine, "Move", E_PlayerStateType.Ground);
        jumpState = new PlayerJumpState(this, stateMachine, "Jump", E_PlayerStateType.Ground);
        fallState = new PlayerFallState(this, stateMachine,"Fall",E_PlayerStateType.Air);

        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack", E_PlayerStateType.Ground);
    }

    protected override void Start()
    {
        base.Start();

        //开启玩家控制器
        PlayerController.GetInstance().ControllerOpen();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        //调用玩家控制器的持续检测输入
        PlayerController.GetInstance().InputUpdate();

        stateMachine.currentState.Update();
    }

    private void OnDestroy()
    {
        //关闭玩家控制器
        PlayerController.GetInstance().ControllerClose();
    }

    // 提供外部触发“设置忙碌状态”的方法，并加入校验
    public void SetBusy(bool busy)
    {
        // 示例：只有在“从非忙碌到忙碌”或“从忙碌到非忙碌”时才执行操作
        if (isBusy == busy) return;

        isBusy = busy;
        // 统一添加状态变化时的逻辑（如通知事件、播放音效等）
        if (busy)
        {
            Debug.Log("角色进入忙碌状态");
            // EventCenter.Broadcast("OnPlayerBusy");
        }
        else
        {
            Debug.Log("角色退出忙碌状态");
            // EventCenter.Broadcast("OnPlayerIdle");
        }
    }

    public override void SetVelocity(float _xVelocity, float _yVelocity)
    {

        

        base.SetVelocity(_xVelocity, _yVelocity);
    }
}
