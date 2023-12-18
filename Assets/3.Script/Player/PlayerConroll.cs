using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerConroll : MonoBehaviour
{
    [SerializeField] private GameObject AttackEffct;
    [SerializeField] private GameObject AttackEffct2;
    [SerializeField] private GameObject DashEffct;
    [SerializeField] private GameObject WaterEffct;
    [SerializeField] private GameObject DamegeEffct;
    [SerializeField] private GameObject Lose;
    [SerializeField] private JumpSound jump;
    [SerializeField] private DashSound dashSound;
    [SerializeField] private AttackSound attackSound;
    [SerializeField] private new AudioSource audio;
    [SerializeField] private AudioClip footstep;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float JumpForce;
    private float horizontal;

    private int Jump = 1;
    private int JumpCount = 0;
    private int wallJump = 1;
    private int wallJumpCount = 0;

    public int score;
    public int maxscore;

    private bool isGrounded = true;
    private bool isRun = false;
    private bool isFacingRight = true;
    private bool isDamege = false;
    public bool IsDamege => isDamege;
    public bool isAtt = false;
    public bool isright => isFacingRight;


    //대쉬 관련 ==========================================
    private bool canDash = true;
    private bool isDash = false;
    [SerializeField] private float dashPower = 2.4f;
    private float dashTime = 0.2f;
    private float dashCoolDown = 1f;
    //====================================================

    //벽슬라이딩 관련=====================================
    private bool isWall = false;
    private bool isWallSliding = true;
    [SerializeField] private float isWallSpeed;
    //====================================================



    [SerializeField] public Rigidbody2D rig;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform WallCheck;
    [SerializeField] private LayerMask groundLayer;


    //GhostEffect관련=====================================
    public SpriteRenderer render;
    public GhostEffect ghost;
    //====================================================


    //Hp 관련=============================================
    [SerializeField] private int MaxHp;
    private int CurHp;
    public int maxhp => MaxHp;
    public int curhp => CurHp;
    //====================================================

    float Timerate = 2f;
    float CurTime;
    private void Awake()
    {
        TryGetComponent(out rig);
        TryGetComponent(out animator);
        TryGetComponent(out render);
        TryGetComponent(out ghost);
        TryGetComponent(out audio);
        ghost.enabled = false;
        AttackEffct.SetActive(false);
        AttackEffct2.SetActive(false);
        DashEffct.SetActive(false);
        WaterEffct.SetActive(false);
        DamegeEffct.SetActive(false);
        CurHp = MaxHp;

        Cursor.visible = false;
    }

    private void SetHp()
    {
        if(CurHp >= maxhp)
        {
            CurHp = maxhp;
        }
    }

    public void SetScore()
    {
        if(score >= maxscore)
        {
            score = maxscore;
        }
    }

    private void FixedUpdate()
    {
        if (isDash)
        {
            return;
        }
        if (isDamege)
        {
            return;
        }


        else if (isRun && CurTime < Time.time && isGrounded)
        {
            audio.clip = footstep;
            audio.Play();
            CurTime = Time.time + Timerate;
        }
        else if(!isGrounded)
        {
            CurTime = 0f;
            audio.Stop();
        }
        

        rig.velocity = new Vector2(horizontal * MoveSpeed, rig.velocity.y);
    }

    
    private void Update()
    {
        if(isDash)
        {
            return;
        }
        if (isDamege)
        {
            return;
        }



        horizontal = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.A) && score >= 4 && CurHp < MaxHp)
        {
            CurHp += 1;
            SetHp();
            score = 0;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) && isGrounded)
        {
            WaterEffct.SetActive(true);
        }
        else
        {
            WaterEffct.SetActive(false);
        }
        if(!isGrounded)
        {
            WaterEffct.SetActive(false);
        }






        if (Input.GetKey(KeyCode.UpArrow) && isGroundedOVER() && JumpCount < Jump)
        {
            jump.audio.Play();
            JumpCount++;
            rig.velocity = new Vector2(rig.velocity.x, JumpForce);
            
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow) && rig.velocity.y > 0f)
        {
            rig.velocity = new Vector2(rig.velocity.x, rig.velocity.y * 0.45f);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canDash && !isWall)
        {
            StartCoroutine(Dash());
        }

        WallSlide();
        WallJump();
        Attack();

        Flip();
        Physics2D.IgnoreLayerCollision (7, 9, true);

        if (isGroundedOVER() == true)
        {
            JumpCount = 0;
            wallJumpCount = 0;
            isGrounded = true;
        }
        else if(isGroundedOVER() == false)
        {
            isGrounded = false;
        }

        if(isWallOVER() == true)
        {
            isWall = true;
        }
        else if(isWallOVER() == false)
        {
            isWall = false;
        }


        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            isRun = true;
        }
        else if(Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            audio.Stop();
            CurTime = 0f;
            isRun = false;
        }



        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("Run", isRun);
        animator.SetBool("Dash", isDash);
        animator.SetBool("IsWall", isWall);
    }



    private bool isGroundedOVER()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    private bool isWallOVER()
    {
        return Physics2D.OverlapCircle(WallCheck.position, 0.1f, groundLayer);
    }

/*    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.x > 0.7f || collision.contacts[0].normal.x < -0.7f)
        {
            isWall = true;
        }

        else
        {
            isWall = false;
        }
    }*/

    private void WallSlide()
    {
        if(isWall && !isGroundedOVER() && horizontal != 0f)
        {
            isWallSliding = true;
            rig.velocity = new Vector2(rig.velocity.x, Mathf.Clamp(rig.velocity.y, -isWallSpeed, float.MaxValue));
        }
        else
        {
            isWall = false;
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        
        if (isWallSliding)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && wallJumpCount < wallJump)
            {
                jump.audio.Play();
                isWall = false;
                wallJumpCount++;
                rig.velocity = new Vector2(rig.velocity.x, JumpForce);
                
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow) && rig.velocity.y > 0f)
            {
                isWall = false;
                rig.velocity = new Vector2(rig.velocity.x, rig.velocity.y * 0.45f);
            }
        }
    }

    private void Attack()
    {
        if(Input.GetKeyDown(KeyCode.Z) && !isWall)
        {
            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                attackSound.audio.Play();
                animator.SetTrigger("Attack");
                StartCoroutine(Attack_co());
            }
            else {  return; }
        }
    }

    private IEnumerator Attack_co()
    {
        AttackEffct.SetActive(true);
        AttackEffct2.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        AttackEffct.SetActive(false);
        AttackEffct2.SetActive(false);
    }

    private IEnumerator Damege_co()
    {
        DamegeEffct.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        DamegeEffct.SetActive(false);
    }


    private void Flip()
    {
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }
    }


    private IEnumerator Dash()
    {

        Physics2D.IgnoreLayerCollision(7, 8, true);
        canDash = false;
        isDash = true;
        ghost.enabled = true;
        float OriginalGravity = rig.gravityScale;
        rig.gravityScale = 0f;
        rig.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
        dashSound.audio.Play();
        if(isDash)
        {
            DashEffct.SetActive(true);
        }
        yield return new WaitForSeconds(dashTime);
        DashEffct.SetActive(false);
        rig.gravityScale = OriginalGravity;
        isDash = false;
        ghost.enabled = false;
        yield return new WaitForSeconds(0.3f);
        Physics2D.IgnoreLayerCollision(7, 8, false);
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }



    public IEnumerator Take(int a)
    {
        isDamege = true;
        CurHp -= a;

        StartCoroutine(Damege_co());

        StopCoroutine("DamegeColor");
        StartCoroutine("DamegeColor");

        Debug.Log("맞았다.");

        if (CurHp <= 0)
        {
            onDie();
        }

        yield return new WaitForSeconds(0.5f);
        isDamege = false;


    }

    private IEnumerator DamegeColor()
    {
        render.color = Color.black;
        yield return new WaitForSeconds(0.15f);
        render.color = Color.white;
    }

    private void onDie()
    {
        StartCoroutine(Die_co());
    }

    private IEnumerator Die_co()
    {
        yield return new WaitForSeconds(0.3f);
        Lose.SetActive(true);
        yield return new WaitForSeconds(0.01f);
        gameObject.SetActive(false);
    }

}
