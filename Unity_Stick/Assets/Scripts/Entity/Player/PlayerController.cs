using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity
{
    [Header("移动参数（关联GameConfig）")]
    public float moveSpeed;
    public float jumpForce;


    // 存储当前移动方向
    private Vector2 moveDirection;

    private bool isOnGrounded;
    private bool isOnSky;
    private bool isOnWall;


    //
    private int atkComboNum;
    private int atkComboMaxNum;
    private bool isAttacking;

    // Start is called before the first frame update
    protected override void Start()
    {

        // 开启输入检测
        InputMgr.GetInstance().StartOrEndCheck(true);

        // 注册按下输入事件监听
        EventCenter.GetInstance().AddEventListener<KeyCode>("某键按下", OnKeyDown);
        // 注册抬起输入事件监听
        EventCenter.GetInstance().AddEventListener<KeyCode>("某键抬起", OnKeyUp);

        

    }

    // Update is called once per frame
    protected override void Update()
    {
        if (isOnGrounded)
        {
            GroundedMoveCheck();
        }
        else if (isOnWall)
        {
            SkyMoveCheck();
        }
        else if (isOnSky)
        {
            WallMoveCheck();
        }
        
    }

    // 按键按下时
    private void OnKeyDown(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.W:

                
                break;
            case KeyCode.S:


                break;
            case KeyCode.A:
                print("1");
                moveDirection.x = -1; // 向左（假设X轴为左右方向）
                break;
            case KeyCode.D:
                moveDirection.x = 1; // 向右
                break;
            case KeyCode.Mouse0:
                print("1");
               
                break;
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
            case KeyCode.D:
                moveDirection.x = 0; // 重置左右方向
                break;
        }
    }

    // 移除事件监听，避免内存泄漏
    private void OnDestroy()
    {
        //移除注册的按键事件
        EventCenter.GetInstance().RemoveEventListener<KeyCode>("A键", OnKeyDown);
        EventCenter.GetInstance().RemoveEventListener<KeyCode>("D键", OnKeyDown);
        EventCenter.GetInstance().RemoveEventListener<KeyCode>("W键", OnKeyDown);
        EventCenter.GetInstance().RemoveEventListener<KeyCode>("S键", OnKeyDown);
        EventCenter.GetInstance().RemoveEventListener<KeyCode>("鼠标左键", OnKeyDown);

        EventCenter.GetInstance().RemoveEventListener<KeyCode>("A键", OnKeyUp);
        EventCenter.GetInstance().RemoveEventListener<KeyCode>("D键", OnKeyUp);
        EventCenter.GetInstance().RemoveEventListener<KeyCode>("W键", OnKeyUp);
        EventCenter.GetInstance().RemoveEventListener<KeyCode>("S键", OnKeyUp);
    }

    //地面移动检测
    private void GroundedMoveCheck()
    {
        moveDirection.Normalize(); // 归一化方向向量
    }

    //空中移动检测
    private void SkyMoveCheck()
    {

    }

    //墙面移动检测
    private void WallMoveCheck()
    {

    }

    private void AttackComboCheck()
    {
        if (AttackingStart()) 
            return;

        atkComboNum += 1;
        if (atkComboNum == atkComboMaxNum) 
            atkComboNum = 0;
    }

    private void Attack()
    {
        AttackComboCheck();

        anim.SetBool("Attack", true);

        AttackingStart();
    }

    //在攻击方法中调用，用来指代攻击动画开始
    private bool AttackingStart()
    {
        isAttacking = true;
        return isAttacking;
    }

    //在攻击动画末尾事件中调用，用来指代攻击动画结束
    private bool AttackingEnd()
    {
        isAttacking = false;
        return isAttacking;
    }
}

