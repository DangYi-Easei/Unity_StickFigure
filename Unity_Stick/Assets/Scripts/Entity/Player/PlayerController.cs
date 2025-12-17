using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseManager<PlayerController>
{

    public float xInput { get; private set; }
    public float yInput { get; private set; }

    

    // Start is called before the first frame update
    public void ControllerOpen()
    {
        // 开启输入检测
        InputMgr.GetInstance().StartOrEndCheck(true);

        // 注册按下输入事件监听
        EventCenter.GetInstance().AddEventListener<KeyCode>(EventName.KeyDown, OnKeyDown);
        // 注册抬起输入事件监听
        EventCenter.GetInstance().AddEventListener<KeyCode>(EventName.KeyUp, OnKeyUp);
    }

    public void InputUpdate()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        // 发布X轴输入变化事件（传递当前xInput值）
        EventCenter.GetInstance().EventTrigger<float>(EventName.X_InputChanged, xInput);

        //加速度逻辑
        if (xInput != 0)
        {
            Player.Instance.currentXSpeed = Mathf.MoveTowards(
                Player.Instance.currentXSpeed,
                xInput * Player.Instance.moveSpeed,
                Player.Instance.acceleration_X * Time.deltaTime);
        }
        else
        {
            // 无输入时，平滑减速到0
            Player.Instance.currentXSpeed = Mathf.MoveTowards(
                Player.Instance.currentXSpeed,
                0,
                Player.Instance.acceleration_X * Time.deltaTime);
        }
    }

    // 按键按下时
    private void OnKeyDown(KeyCode key)
    {
        Debug.Log("按钮按下检测");
        //地面操作
        if (Player.Instance.stateMachine.currentState.e_playerStateType == E_PlayerStateType.Ground)
        {
            Debug.Log("按钮检测");
            switch (key)
            {

                case KeyCode.A:
                    Debug.Log("A");
                    //如果有反方向速度存在，将其设为0，防止影响手感
                    if (Player.Instance.currentXSpeed>0)
                        Player.Instance.currentXSpeed = 0;
                    break;
                case KeyCode.D:
                    Debug.Log("D");
                    //如果有反方向速度存在，将其设为0，防止影响手感
                    if (Player.Instance.currentXSpeed < 0)
                        Player.Instance.currentXSpeed = 0;
                    break;

                case KeyCode.Mouse0:
                    //Player.Instance.stateMachine.ChangeState(Player.Instance.primaryAttack);

                    break;
            }
        }
        //空中操作
        else if(Player.Instance.stateMachine.currentState.e_playerStateType == E_PlayerStateType.Air)
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
                    //Player.Instance.stateMachine.ChangeState(Player.Instance.primaryAttack);

                    break;
            }
        }
    }

    // 按键抬起时
    private void OnKeyUp(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.W:
            case KeyCode.S:

                break;
            case KeyCode.A:
                break;  
            case KeyCode.D:
                break;
        }
    }

    // 移除事件监听，避免内存泄漏
    public void ControllerClose()
    {
        //移除注册的按键事件
        EventCenter.GetInstance().RemoveEventListener<KeyCode>(EventName.KeyDown, OnKeyDown);
        EventCenter.GetInstance().RemoveEventListener<KeyCode>(EventName.KeyUp, OnKeyUp);
    }

    //协程
    public IEnumerator BusyFor(float _seconds)
    {
        Player.Instance.SetBusy(true);

        yield return new WaitForSeconds(_seconds);

        Player.Instance.SetBusy(false);
    }
}

