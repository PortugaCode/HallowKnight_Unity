using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOpen : MonoBehaviour
{
    Animator animator;
    BoxCollider2D col;
    private bool canopen = true;
    AudioSource audio;

    private bool touch = false;

    private void Awake()
    {
        TryGetComponent(out animator);
        TryGetComponent(out col);
        TryGetComponent(out audio);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && canopen && !touch)
        {
            StartCoroutine("open");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine("close");
        }
    }

    private IEnumerator open()
    {
        touch = true;
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("Open");
        audio.Play();
        col.isTrigger = true;
    }

    private IEnumerator close()
    {
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("Close");
        col.isTrigger = false;
        canopen = false;
    }
}
