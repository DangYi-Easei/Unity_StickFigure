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

    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;
    public float attackOneTimeBusyTime = 0.15f;

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    //public PlayerAirState airState { get; private set; }
    //public PlayerWallSlideState wallSlide { get; private set; }
    //public PlayerWallJumpState wallJump { get; private set; }
    //public PlayerDashState dashState { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    //public PlayerCounterAttackState counterAttack { get; private set; }
    //public PlayerDeadState deadState { get; private set; }

    #endregion

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

    
}
