using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneAttack : MonoBehaviour
{
    [SerializeField] private PlayerConroll player;
    [SerializeField] private GameObject effect;
    [SerializeField] private GameObject Bottominfo;
    private Rigidbody2D rig;
    private BoxCollider2D col;
    [SerializeField] private float RaycastLength;
    private bool canatt = true;
    private bool canfind = true;

    private void Awake()
    {
        TryGetComponent(out rig);
        TryGetComponent(out col);
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);
        effect.SetActive(false);
        col.enabled = false;
        rig.gravityScale = 0f;
    }


    private void Update()
    {
        FindPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !player.IsDamege && canatt)
        {
            player.rig.velocity = Vector2.zero;
            if (player.transform.position.x > transform.position.x)
            {
                player.StartCoroutine(player.Take(1));
                player.rig.AddForce(new Vector2(75f, player.transform.position.y));
                player.rig.velocity = new Vector2(player.rig.velocity.x, 2f);
                gameObject.tag = "Finish";
                gameObject.layer = 9;
            }
            else if (player.transform.position.x < transform.position.x)
            {
                player.StartCoroutine(player.Take(1));
                player.rig.AddForce(new Vector2(-75f, player.transform.position.y));
                player.rig.velocity = new Vector2(player.rig.velocity.x, 2f);
                gameObject.tag = "Finish";
                gameObject.layer = 9;
            }
            col.isTrigger = false;
            canatt = false;
        }

        else if (collision.CompareTag("Wall"))
        {
            gameObject.tag = "Finish";
            gameObject.layer = 9;
            canatt = false;
            col.isTrigger = false;
        }


        else if (collision.CompareTag("Enemy"))
        {
            gameObject.tag = "Finish";
            gameObject.layer = 9;
            canatt = false;
            col.isTrigger = false;
        }

        else if (collision.CompareTag("Enemy_Fly"))
        {
            gameObject.tag = "Finish";
            gameObject.layer = 9;
            canatt = false;
            col.isTrigger = false;
        }
    }

    private void FindPlayer()
    {
        RaycastHit2D Find = Physics2D.Raycast(Bottominfo.transform.position, Vector3.down, RaycastLength);

        if (Find.collider == true && Find.collider.CompareTag("Player") && canfind)
        {
            Debug.Log("플레이어 감지");
            col.enabled = true;
            col.isTrigger = true;
            StartCoroutine(Shooting());
            canfind = false;
        }
        else
        {
            return;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(Bottominfo.transform.position, Vector3.down * RaycastLength);
    }

    private IEnumerator Shooting()
    {
        Debug.Log("슈팅");
        effect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        rig.gravityScale = 4f;
        /*Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        rig.velocity = new Vector2(rig.velocity.x, direction.y * 8f);*/
        //rig.velocity = direction * 4f;
        yield return new WaitForSeconds(1f);
        effect.SetActive(false);
    }
}
