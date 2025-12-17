using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{

    private E_PlayerStateType e_PlayerStateType;
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName, E_PlayerStateType _playerStateType) : base(_player, _stateMachine, _animBoolName, _playerStateType)
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
    }
}
