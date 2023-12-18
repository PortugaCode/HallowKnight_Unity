using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOn : MonoBehaviour
{
    [SerializeField] private GameObject a;
    [SerializeField] private CameraPosition c;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(open());
        }
    }



    private IEnumerator open()
    {
        yield return new WaitForSeconds(3.5f);
        a.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        c.boss = true;

        AudioManager.Instance.PlayMusic("Boss Theme");
        
    }
}
