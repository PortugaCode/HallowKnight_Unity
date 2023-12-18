using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControll : MonoBehaviour
{
    [SerializeField] private PlayerConroll player;
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private GameObject Effect;

    [SerializeField] private float MaxHp;
    private float CurHp;
    public float maxhp => MaxHp;
    public float curhp => CurHp;



    
    [SerializeField] private float inLine;


    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private bool isGrounded = true;
    private bool isWalk = false;
    public bool isDemage = false;
    public bool isDie = false;




    //============================================================

    [SerializeField] private float speed;
    [SerializeField] private float distance2;
    private bool movingright = true;
    [SerializeField] private Transform groundCheck2;
    [SerializeField] private Transform groundCheck3;



    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);
        TryGetComponent(out animator);
        TryGetComponent(out rig);
        TryGetComponent(out render);

        Effect.SetActive(false);
        CurHp = MaxHp;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.StartCoroutine(player.Take(1));
            player.rig.velocity = Vector2.zero;
            if (player.transform.position.x > transform.position.x)
            {
                StartCoroutine(KnockBack(0.1f, -200f, player.transform.position));
                player.rig.AddForce(new Vector2(125f, player.transform.position.y));
                player.rig.velocity = new Vector2(player.rig.velocity.x, 2f);
            }
            else if (player.transform.position.x < transform.position.x)
            {
                StartCoroutine(KnockBack(0.1f, 200f, player.transform.position));
                player.rig.AddForce(new Vector2(-125f, player.transform.position.y));
                player.rig.velocity = new Vector2(player.rig.velocity.x, 2f);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack"))
        {
            isDemage = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack"))
        {
            isDemage = false;
        }
    }


    private void FixedUpdate()
    {
        if(isDie)
        {
            return;
        }
        Vector3 a = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        float distance = Vector3.Distance(player.transform.position, transform.position);
        isWalk = true;


        transform.Translate(Vector2.left * speed * Time.deltaTime);
        RaycastHit2D groundinfo = Physics2D.Raycast(groundCheck2.position, Vector2.down, distance2);
        RaycastHit2D groundinfo2 = Physics2D.Raycast(groundCheck3.position, Vector2.left, 0.1f);

        if (distance < inLine)
        {
            transform.position = new Vector3(a.x, transform.position.y, 0f);
            Flip();
        }

        else
        {
            if (groundinfo.collider == false || groundinfo2.collider == true && isGrounded)
            {
                if (movingright)
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    movingright = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    movingright = true;
                }
            }
        }
    }

    private void Update()
    {
        if (isGroundedOVER() == true)
        {
            isGrounded = true;
        }
        else if (isGroundedOVER() == false)
        {
            isGrounded = false;
        }

        animator.SetBool("Walk", isWalk);
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("Die", isDie);
        
    }


    private void Flip()
    {
        if(gameObject.tag == "Finish")
        {
            return;
        }
        if(player.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            movingright = false;
        }
        else if(player.transform.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            movingright = true;
        }
    }


    public void TakeDamege(float d)
    {
        if(isDemage == true)
        {
            return;
        }
        else if(isDemage == false)
        {
            CurHp -= d;
            Debug.Log("¶§·È´Ù.");

            if (player.transform.position.x > transform.position.x)
            {
                StartCoroutine(KnockBack(0.07f, -250f, transform.position));
            }
            else if (player.transform.position.x < transform.position.x)
            {
                StartCoroutine(KnockBack(0.07f, 250f, transform.position));
            }

            if (CurHp <= 0)
            {
                rig.velocity = new Vector2(rig.velocity.x, 4f);
                onDie();
            }
            isDemage = true;
        }
    }


    private void onDie()
    {
        StartCoroutine(Damege_co());
        Destroy(gameObject, 2f);
        gameObject.tag = "Finish";
        gameObject.layer = 9;
        isDie = true;
    }

    private bool isGroundedOVER()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    private IEnumerator KnockBack(float knockbackDur, float knockbackPwr, Vector3 knockbackDir)
    {
        float timer = 0f;
        rig.velocity = Vector3.zero;

        while (knockbackDur > timer)
        {
            timer += Time.deltaTime;
            rig.AddForce(new Vector3(knockbackDir.x + knockbackPwr, transform.position.y, 0f));
        }
        yield return null;
    }

    private IEnumerator Damege_co()
    {
        Effect.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        Effect.SetActive(false);
    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, inLine);
    }

}
