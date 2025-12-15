using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    private E_PlayerStateType e_PlayerStateType = E_PlayerStateType.Ground;

    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName, E_PlayerStateType _playerStateType) : base(_player, _stateMachine, _animBoolName, _playerStateType)
    {
        //将标识改为地面
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
    }
}
