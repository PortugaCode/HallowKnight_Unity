using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControll : MonoBehaviour
{
    [SerializeField] private PlayerConroll player;
    [SerializeField] private GameObject Head;
    [SerializeField] private GameObject Particle;
    [SerializeField] private GameObject Attack_pre;
    [SerializeField] private GameObject Attack_Start;
    [SerializeField] private GameObject Win;
    [SerializeField] private Transform GroundCheck;
    private new AudioSource audio;



    [SerializeField] private float RunRange;
    [SerializeField] private float AttRange;
    [SerializeField] private float RunSpeed;

    public Transform groundCheck;
    public LayerMask groundLayer;

    private bool isStunCan = true;
    private bool isStun = false;
    public bool IsStun => isStun;
    private float isStunDu = 8f;

    private bool isDie = false;
    private bool isDieCan = true;




    private bool isRun;
    private bool isRunDu;
    private bool isRunDa = false;

    private bool isAttack;
    private bool isAttackDu = false;

    private bool isJump;
    private bool isJumpAtt01;
    private bool isJumpAtt02;
    private bool isJumpAtt03;
    private bool isGrounded;
    private bool isJumpDu = false;

    private bool isShoot = true;

    private Rigidbody2D rig;
    private BoxCollider2D col;
    private Animator animator;
    [SerializeField] private Animator head_animator;


    [SerializeField] private float MaxHp;
    private float CurHp;
    public float maxhp => MaxHp;
    public float curhp => CurHp;

    private void Awake()
    {
        CurHp = MaxHp;
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);
        TryGetComponent(out rig);
        TryGetComponent(out animator);
        TryGetComponent(out col);
        TryGetComponent(out audio);
        Head.SetActive(false);
        Particle.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isStun)
        {
            return;
        }

        else if(collision.CompareTag("Player") && !player.IsDamege && CurHp <= 15)
        {
            player.StartCoroutine(player.Take(2));
            player.rig.velocity = Vector2.zero;
            if (player.transform.position.x > transform.position.x)
            {
                player.rig.AddForce(new Vector2(125f, player.transform.position.y));
                player.rig.velocity = new Vector2(player.rig.velocity.x, 2f);
            }
            else if (player.transform.position.x < transform.position.x)
            {
                player.rig.AddForce(new Vector2(-125f, player.transform.position.y));
                player.rig.velocity = new Vector2(player.rig.velocity.x, 2f);
            }
            col.isTrigger = false;
        }
        else if(collision.CompareTag("Player") && !player.IsDamege)
        {
            player.StartCoroutine(player.Take(1));
            player.rig.velocity = Vector2.zero;
            if (player.transform.position.x > transform.position.x)
            {
                player.rig.AddForce(new Vector2(125f, player.transform.position.y));
                player.rig.velocity = new Vector2(player.rig.velocity.x, 2f);
            }
            else if (player.transform.position.x < transform.position.x)
            {
                player.rig.AddForce(new Vector2(-125f, player.transform.position.y));
                player.rig.velocity = new Vector2(player.rig.velocity.x, 2f);
            }
            col.isTrigger = false;
        }
    }

    private void Update()
    {

        if (CurHp <= 0 && !isRunDu && !isJumpDu && !isAttackDu && !isStun && isDieCan)
        {
            isDieCan = false;
            onDie();
        }

        if (isGroundedOVER() == true)
        {
            isGrounded = true;
        }
        else if (isGroundedOVER() == false)
        {
            isGrounded = false;
        }

        animator.SetBool("Run", isRun);
        animator.SetBool("Attack", isAttack);
        animator.SetBool("Jump", isJump);
        animator.SetBool("Ground", isGrounded);
        animator.SetBool("Stun", isStun);
        animator.SetBool("Die", isDie);


        if (!isStun && CurHp <= 40 && CurHp >= 20 && !isRunDu && !isJumpDu && !isAttackDu && isStunCan)
        {
            isStunCan = false;
            StartCoroutine(Stun_co());
        }



        if (isDie)
        {
            return;
        }

        if (isStun)
        {
            return;
        }






        if (!isRunDu && !isAttack && !isJump && !isStun)
        {
            Flip();
        }

        if (!isStun && CurHp <= 40 && CurHp >= 20 && !isRunDu && !isJumpDu && !isAttackDu && isStunCan)
        {
            isStunCan = false;
            StartCoroutine(Stun_co());
        }

        RunAttack();
        JumpAttack();
        Attack();
        if(isAttackDu)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && isShoot)
            {
                Shooting();
                Particle.SetActive(true);
            }
        }
        
        








    }


    private bool Runbool()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if(distance > RunRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool Attbool()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < AttRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void RunAttack()
    {
        if (Runbool() && !isRunDu && !isAttackDu && !isJumpDu && !isStun)
        {
            StartCoroutine(Run());
        }
        else if (isRun)
        {
            Physics2D.IgnoreLayerCollision(8, 7, true);
            RaycastHit2D groundinfo = Physics2D.Raycast(GroundCheck.position, Vector2.left, 0.1f);
            if (groundinfo.collider == true)
            {
                if (groundinfo.collider.CompareTag("Wall"))
                {
                    isRun = false;
                    Physics2D.IgnoreLayerCollision(8, 7, false);
                    isRunDa = false;
                    Particle.SetActive(true);
                }

                else if (groundinfo.collider.CompareTag("Player") && !isRunDa)
                {
                    Debug.Log("À¯Àú ´ê¾Ò´Ù.");
                    player.StartCoroutine(player.Take(1));
                    player.rig.velocity = Vector2.zero;
                    player.rig.velocity = new Vector2(player.rig.velocity.x, 5f);
                    isRunDa = true;
                }
            }
        }
    }

    private IEnumerator Run()
    {
        isRunDu = true;
        isRun = true;
        yield return new WaitForSeconds(0.5f);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        rig.velocity = new Vector2(direction.x * RunSpeed*3f, rig.velocity.y);
        yield return new WaitForSeconds(2f);
        isRun = false;
        isRunDa = false;
        Physics2D.IgnoreLayerCollision(8, 7, false);
        Particle.SetActive(false);
        yield return new WaitForSeconds(1f);
        isRunDu = false;
    }

    private void Attack()
    {
        if(!Runbool() && !isAttackDu && !isRunDu && !isJumpDu && Attbool() && !isStun)
        {
            StartCoroutine(Attack_co());
        }
        else
        {
            return;
        }
    }

    private IEnumerator Attack_co()
    {
        isAttackDu = true;
        yield return new WaitForSeconds(0.6f);
        isAttack = true;
        yield return new WaitForSeconds(1.3f);
        isAttack = false;
        yield return new WaitForSeconds(0.6f);
        isAttackDu = false;
        Particle.SetActive(false);
        isShoot = true;
    }


    private void JumpAttack()
    {
        if (!Runbool() && !isAttackDu && !isRunDu && !isJumpDu && !Attbool() && !isStun)
        {
            StartCoroutine(Jump_co());
        }
        else
        {
            return;
        }
    }

    private IEnumerator Jump_co()
    {
        isJumpDu = true;
        yield return new WaitForSeconds(0.4f);
        rig.velocity = Vector2.zero;
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        yield return new WaitForSeconds(0.8f);
        isJump = true;
        yield return new WaitForSeconds(0.7f);

        rig.velocity = new Vector2(direction.x * RunSpeed * 0.6f, 10f);
        
        yield return new WaitForSeconds(1f);
        
        if (rig.velocity.y > 0f)
        {
            rig.AddForce(new Vector2(direction.x * RunSpeed*10f, -300f));
            //rig.velocity = new Vector2(rig.velocity.x, rig.velocity.y * 0.45f);
        }


        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("JumpAtt03"))
        {
            if(isGrounded == true && !isJumpAtt01)
            {
                isJumpAtt01 = true;
                animator.SetTrigger("JumpAttack");
            }
            else if(animator.GetCurrentAnimatorStateInfo(0).IsName("JumpAtt01") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !isJumpAtt02)
            {
                Particle.SetActive(true);
                isJumpAtt01 = false;
                isJumpAtt02 = true;
                animator.SetTrigger("JumpAttack02");
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("JumpAtt02") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !isJumpAtt03)
            {
                isJumpAtt02 = false;
                isJumpAtt03 = true;
                animator.SetTrigger("JumpAttack03");
            }
            yield return null;
        }
        isJumpAtt03 = false;
        isJump = false;
        Particle.SetActive(false);
        yield return new WaitForSeconds(1f);
        isJumpDu = false;
        
    }


    private bool isGroundedOVER()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);
    }


    private void Flip()
    {
        if (gameObject.tag == "Finish")
        {
            return;
        }
        if (player.transform.position.x > transform.position.x)
        {
            Vector3 localscale = transform.localScale;
            localscale.x = 1f;
            transform.localScale = localscale;
        }
        else if (player.transform.position.x < transform.position.x)
        {
            Vector3 localscale = transform.localScale;
            localscale.x = -1f;
            transform.localScale = localscale;
        }
    }

    private IEnumerator Stun_co()
    {
        isStun = true;

        rig.velocity = Vector2.zero;
        rig.velocity = new Vector2(rig.velocity.x, 4f);
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("StunOpenDone"))
        {

            if(animator.GetCurrentAnimatorStateInfo(0).IsName("StunOpen") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                Head.SetActive(true);
            }
            yield return null;
        }

        rig.bodyType = RigidbodyType2D.Static;
        gameObject.layer = 6;
        yield return new WaitForSeconds(isStunDu);
        gameObject.layer = 8;
        isStun = false;
        yield return new WaitForSeconds(0.07f);
        Head.SetActive(false);
        rig.bodyType = RigidbodyType2D.Dynamic;
    }



    public void TakeDamege(float d)
    {
        CurHp -= d;
        Debug.Log("¶§·È´Ù.");
        if(isStun)
        {
            head_animator.SetTrigger("StunHit");
            animator.SetTrigger("StunHit");
        }
    }


    private void onDie()
    {
        rig.velocity = new Vector2(rig.velocity.x, 6f);
        Destroy(gameObject, 5f);
        gameObject.tag = "Finish";
        gameObject.layer = 9;
        Physics2D.IgnoreLayerCollision(9, 7, true);
        StartCoroutine(Win_co());
        isDie = true;
    }

    private IEnumerator Win_co()
    {
        yield return new WaitForSeconds(4f);
        Win.SetActive(true);
    }

    private void Shooting()
    {
        GameObject bullet = Instantiate(Attack_pre, Attack_Start.transform.position, Quaternion.identity);
        
        if (player.transform.position.x > transform.position.x)
        {
            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(600f, 0f));
            Vector3 localscale = bullet.transform.localScale;
            localscale.x = 0.5f;
            bullet.transform.localScale = localscale;
        }
        else if (player.transform.position.x < transform.position.x)
        {
            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(-600f, 0f));
            Vector3 localscale = bullet.transform.localScale;
            localscale.x = -0.5f;
            bullet.transform.localScale = localscale;
        }
        isShoot = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, RunRange);
        Gizmos.DrawWireSphere(transform.position, AttRange);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, 0.5f);
    }
}
