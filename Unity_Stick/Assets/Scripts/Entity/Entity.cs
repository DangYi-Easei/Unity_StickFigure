using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CapsuleCollider2D cd { get; private set; }


    #endregion

    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public System.Action onFlipped;


    // Start is called before the first frame update
    protected virtual void Awake()
    {

    }


    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    #region Velocity

    public void SetZeroVelocity()
    {
        //被击退时释放速度，防止没有速度清空而清空击退效果
        if (isKnocked)
            return;

        rb.velocity = new Vector2(0, 0);
    }


    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        //被击退时禁止移动
        if (isKnocked)
            return;


        //获取并设定x，y轴速度
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);//调用翻转控制函数
    }
    #endregion


    //角色翻转函数
    public void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if (onFlipped != null) 
            onFlipped();
    }

    //翻转控制函数
    public void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }

    public virtual void Die()
    {

    }
}
