using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControll_Chock : MonoBehaviour
{
    [SerializeField] private PlayerConroll player;
    [SerializeField] private GameObject particle;
    private Animator animator;
    private bool partic = true;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);
        TryGetComponent(out animator);
        particle.SetActive(false);
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

    private void Update()
    {
        if(partic)
        {
            StartCoroutine(Parti());
        }
    }

    private IEnumerator Parti()
    {
        partic = false;
        particle.SetActive(true);
        yield return new WaitForSeconds(1f);
        particle.SetActive(false);
        partic = true;
    }











}
