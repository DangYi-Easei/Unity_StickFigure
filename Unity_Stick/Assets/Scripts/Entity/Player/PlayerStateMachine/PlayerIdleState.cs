using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerIdleState : PlayerState
{
    private E_PlayerStateType e_PlayerStateType;

    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName, E_PlayerStateType _playerStateType) : base(_player, _stateMachine, _animBoolName, _playerStateType)
    {
        //更改当前状态的位置标识
        _playerStateType = e_PlayerStateType;
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //if (PlayerController.GetInstance().xInput == player.facingDir)
        //    return;

        //检测到x轴有输入且玩家不忙碌时，转换为移动状态
        if (player.currentXSpeed != 0 && !player.isBusy)
            stateMachine.ChangeState(player.moveState);
    }
}
