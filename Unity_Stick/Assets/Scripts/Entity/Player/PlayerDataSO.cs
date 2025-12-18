using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "GameData/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    [Header("移动参数")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float dashForce = 10f;

    [Header("战斗参数")]
    public int maxHP = 5;
    public int attackDamage = 1;
}
