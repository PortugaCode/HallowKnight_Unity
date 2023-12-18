using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControll_Fly : MonoBehaviour
{
    [SerializeField] private PlayerConroll player;
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private GameObject Effect;
    [SerializeField] private GameObject Bullet_prefabs;
    [SerializeField] private GameObject Bullet_Start;

    [SerializeField] private float MaxHp;
    private float CurHp;
    public float maxhp => MaxHp;
    public float curhp => CurHp;

    private float backspeed = 0f;



    [SerializeField] private float speed;
    [SerializeField] private float inLine;
    [SerializeField] private float ShootingRange;
    [SerializeField] private float BulletSpeed;
    private bool shoot = false;


    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rig;

    public bool isDemage = false;
    public bool isDie = false;



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
                player.rig.AddForce(new Vector2(125f, player.transform.position.y));
                player.rig.velocity = new Vector2(player.rig.velocity.x, 2f);
            }
            else if (player.transform.position.x < transform.position.x)
            {
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

    private void Update()
    {
        animator.SetBool("Die", isDie);
        if (isDie)
        {
            return;
        }
        Vector3 a = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance < inLine && distance > ShootingRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            transform.position = new Vector3(a.x, a.y, 0f);
        }
        else if(distance < ShootingRange)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !shoot)
            {
                StartCoroutine(Shooting());
            }
            else if(!shoot)
            {
                transform.position = new Vector3(a.x, a.y, 0f);
            }
        }

     
        Flip();
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
            localscale.x = -0.8f;
            transform.localScale = localscale;
        }
        else if (player.transform.position.x < transform.position.x)
        {
            Vector3 localscale = transform.localScale;
            localscale.x = 0.8f;
            transform.localScale = localscale;
        }
    }


    public void TakeDamege(float d)
    {
        if (isDemage == true)
        {
            return;
        }
        else if (isDemage == false)
        {
            CurHp -= d;
            Debug.Log("¶§·È´Ù.");

            if (player.transform.position.x > transform.position.x)
            {
                StartCoroutine(KnockBackminus());

            }
            else if (player.transform.position.x < transform.position.x)
            {
                StartCoroutine(KnockBackplus());
            }

            if (CurHp <= 0)
            {
                onDie();
            }
            isDemage = true;
        }
    }


    private void onDie()
    {
        isDie = true;
        StartCoroutine(Damege_co());
        Destroy(gameObject, 0.8f);
        gameObject.tag = "Finish";
        gameObject.layer = 9;
    }


    private IEnumerator KnockBackplus()
    {
        rig.velocity = new Vector2(rig.velocity.x + 2f, rig.velocity.y);
        yield return new WaitForSeconds(0.5f);
        rig.velocity = Vector2.zero;
    }

    private IEnumerator KnockBackminus()
    {
        rig.velocity = new Vector2(rig.velocity.x - 2f, rig.velocity.y);
        yield return new WaitForSeconds(0.5f);
        rig.velocity = Vector2.zero;
    }

    private IEnumerator Damege_co()
    {
        Effect.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        Effect.SetActive(false);
    }

    private IEnumerator Shooting()
    {
        animator.SetTrigger("Attack");
        shoot = true;
        yield return new WaitForSeconds(1f);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        if(isDie)
        {
            yield break;
        }
        GameObject bullet = Instantiate(Bullet_prefabs, Bullet_Start.transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * BulletSpeed;
        yield return new WaitForSeconds(2f);
        shoot = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, inLine);
        Gizmos.DrawWireSphere(transform.position, ShootingRange);
    }
}
