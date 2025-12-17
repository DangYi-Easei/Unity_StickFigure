using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMoveState : PlayerState
{
    public E_PlayerStateType e_PlayerStateType;

    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName, E_PlayerStateType _playerStateType) : base(_player, _stateMachine, _animBoolName, _playerStateType)
    {
        //更改当前状态的位置标识
        _playerStateType = e_PlayerStateType;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.currentXSpeed, rb.velocity.y);


        //检测到x轴无速度时（或角色跑近墙壁时），转换为闲置状态
        if (player.currentXSpeed == 0)
            stateMachine.ChangeState(player.idleState);
    }
}
