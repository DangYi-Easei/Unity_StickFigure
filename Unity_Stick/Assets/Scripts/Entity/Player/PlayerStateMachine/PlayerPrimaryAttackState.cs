using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName, E_PlayerStateType _playerStateType) : base(_player, _stateMachine, _animBoolName, _playerStateType)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", player.attackOneTimeBusyTime);//每次攻击时让玩家处于忙碌状态0.1秒，防止发生移动
    }

    public override void Update()
    {
        base.Update();
    }
}
