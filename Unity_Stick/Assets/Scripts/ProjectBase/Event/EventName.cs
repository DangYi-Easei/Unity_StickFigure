using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventName
{
    //输入事件：X轴输入变化
    public const string X_InputChanged = "X_InputChanged";
    //输入事件：按键按下
    public const string KeyDown = "KeyDown";
    //输入事件：按键抬起
    public const string KeyUp = "KeyUp";
    //状态事件：玩家状态变化
    public const string PlayerStateChanged = "PlayerStateChanged";
    //状态事件：玩家忙碌状态变化
    public const string PlayerBusyChanged = "PlayerBusyChanged";
}
