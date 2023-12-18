using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject barrel;
    [SerializeField] private GameObject AttackEffct2;
    [SerializeField] private GameObject Stun;
    [SerializeField] private PlayerConroll player;
    [SerializeField] private EnemyControll Enemy;
    [SerializeField] private float Power;

    private Vector3 re = new Vector3(0.57f, 0.13f, 0f);



    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);
    }


    private void Update()
    {
        if(transform.localPosition != re)
        {
            transform.localPosition = re;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyControll>().TakeDamege(1f);
            PlayerBack_Enemy();
            player.score += 1;
            player.SetScore();

            if (collision.GetComponent<EnemyControll>().curhp <= 0)
            {
                GameObject a = Instantiate(Stun, player.transform.position, Quaternion.identity);
                Destroy(a, 1f);
            }

            gameObject.SetActive(false);
        }

        else if (collision.CompareTag("Enemy_Fly"))
        {
            collision.GetComponent<EnemyControll_Fly>().TakeDamege(1f);
            PlayerBack_Enemy();
            player.score += 1;
            player.SetScore();

            if (collision.GetComponent<EnemyControll_Fly>().curhp <= 0)
            {
                GameObject a = Instantiate(Stun, player.transform.position, Quaternion.identity);
                Destroy(a, 1f);
            }

            gameObject.SetActive(false);
        }

        else if (collision.CompareTag("Boss"))
        {
            collision.GetComponent<BossControll>().TakeDamege(1f);
            PlayerBack_Enemy();
            player.score += 1;
            player.SetScore();

            if (collision.GetComponent<BossControll>().curhp <= 0)
            {
                GameObject a = Instantiate(Stun, player.transform.position, Quaternion.identity);
                Destroy(a, 1f);
            }

            gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Wall"))
        {
            PlayerBack_Wall();
            gameObject.SetActive(false);
        }

        else if(collision.CompareTag("Chock"))
        {
            PlayerBack_Wall();
            gameObject.SetActive(false);
        }
    }



    private void PlayerBack_Wall()
    {
        player.rig.velocity = Vector2.zero;
        if (player.isright)
        {
            player.rig.AddForce(new Vector2(-1500f, player.transform.position.y));
            player.rig.velocity = new Vector2(player.rig.velocity.x, 2f);
            GameObject a = Instantiate(barrel, transform.position, Quaternion.identity);
            Destroy(a, 1f);
        }
        else if (!player.isright)
        {
            player.rig.AddForce(new Vector2(1500f, player.transform.position.y));
            player.rig.velocity = new Vector2(player.rig.velocity.x, 2f);
            GameObject a = Instantiate(barrel, transform.position, Quaternion.identity);
            Destroy(a, 1f);
        }
    }

    private void PlayerBack_Enemy()
    {
        player.rig.velocity = Vector2.zero;
        if (player.isright)
        {
            player.rig.AddForce(new Vector2(-1500f, player.transform.position.y));
            player.rig.velocity = new Vector2(player.rig.velocity.x, 2f);
            GameObject a = Instantiate(barrel, transform.position, Quaternion.identity);
            GameObject b = Instantiate(AttackEffct2, player.transform.position, Quaternion.identity);
            Destroy(a, 1f);
            Destroy(b, 0.5f);
        }
        else if (!player.isright)
        {
            player.rig.AddForce(new Vector2(1500f, player.transform.position.y));
            player.rig.velocity = new Vector2(player.rig.velocity.x, 2f);
            GameObject a = Instantiate(barrel, transform.position, Quaternion.identity);
            GameObject b = Instantiate(AttackEffct2, player.transform.position, Quaternion.identity);
            Destroy(a, 1f);
            Destroy(b, 0.5f);
        }
    }



}
