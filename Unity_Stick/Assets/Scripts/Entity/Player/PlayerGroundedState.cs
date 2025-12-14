using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player,PlayerStateMachine _playerMachine,string _animBoolName):base(_player,_playerMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        
    }
}
