using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseManager<PlayerController>
{

    // Start is called before the first frame update
    public void ControllerOpen()
    {
        // 开启输入检测
        InputMgr.GetInstance().StartOrEndCheck(true);

        // 注册按下输入事件监听
        EventCenter.GetInstance().AddEventListener<KeyCode>("某键按下", OnKeyDown);
        // 注册抬起输入事件监听
        EventCenter.GetInstance().AddEventListener<KeyCode>("某键抬起", OnKeyUp);
    }

    // 按键按下时
    private void OnKeyDown(KeyCode key)
    {
        //地面操作
        if (Player.Instance.stateMachine.currentState.playerStateType == E_PlayerStateType.Ground)
        {
            switch (key)
            {
                case KeyCode.Mouse0:
                    Player.Instance.stateMachine.ChangeState(Player.Instance.primaryAttack);

                    break;
            }
        }
        //空中操作
        else if(Player.Instance.stateMachine.currentState.playerStateType == E_PlayerStateType.Air)
        {
            switch (key)
            {
                //case KeyCode.W:

                //    break;
                //case KeyCode.S:


                //    break;
                //case KeyCode.A:


                //    break;
                //case KeyCode.D:

                //    break;


                case KeyCode.Mouse0:
                    Player.Instance.stateMachine.ChangeState(Player.Instance.primaryAttack);

                    break;
            }
        }
    }

    // 按键抬起时
    private void OnKeyUp(KeyCode key)
    {
        switch (key)
        {
            //case KeyCode.W:
            //case KeyCode.S:
                
            //    break;
            //case KeyCode.A:
            //case KeyCode.D:
                
            //    break;
        }
    }

    // 移除事件监听，避免内存泄漏
    public void ControllerClose()
    {
        //移除注册的按键事件
        EventCenter.GetInstance().RemoveEventListener<KeyCode>("某键按下", OnKeyDown);
        EventCenter.GetInstance().RemoveEventListener<KeyCode>("某键抬起", OnKeyUp);
    }

    //协程
    public IEnumerator BusyFor(float _seconds)
    {
        Player.Instance.SetBusy(true);

        yield return new WaitForSeconds(_seconds);

        Player.Instance.SetBusy(false);
    }
}

