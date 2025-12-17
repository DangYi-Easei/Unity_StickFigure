using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;


public enum E_PlayerStateType
{
    Ground,
    Air
}

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    public E_PlayerStateType e_playerStateType { get; private set; }
    protected Player player;

    protected Rigidbody2D rb;

    
    private string animBoolName;

    protected float stateTimer;//×´Ì¬¼ÆÊ±Æ÷
    protected bool triggerCalled;//´¥·¢Æ÷

    public PlayerState(Player _player,PlayerStateMachine _stateMachine,string _animBoolName, E_PlayerStateType _playerStateType)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
        this.e_playerStateType = _playerStateType;
    }


    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.time;
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
