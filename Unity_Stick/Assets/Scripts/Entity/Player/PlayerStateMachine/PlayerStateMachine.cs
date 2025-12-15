using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState {  get; private set; }

    public void Initialize(PlayerState _startState)
    {
        //设置起始状态
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        //先退出旧状态
        currentState.Exit();
        //更新 新状态
        currentState = _newState;
        //进入新状态
        currentState.Enter();
    }
}
