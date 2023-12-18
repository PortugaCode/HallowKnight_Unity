using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private PlayerConroll player;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !player.IsDamege)
        {
            player.rig.velocity = Vector2.zero;
            if (player.transform.position.x > transform.position.x)
            {
                player.StartCoroutine(player.Take(1));
                player.rig.AddForce(new Vector2(75f, player.transform.position.y));
                player.rig.velocity = new Vector2(player.rig.velocity.x, 2f);
                Destroy(gameObject);
            }
            else if (player.transform.position.x < transform.position.x)
            {
                player.StartCoroutine(player.Take(1));
                player.rig.AddForce(new Vector2(-75f, player.transform.position.y));
                player.rig.velocity = new Vector2(player.rig.velocity.x, 2f);
                Destroy(gameObject);
            }
        }

        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        Destroy(gameObject, 3f);
    }



}
