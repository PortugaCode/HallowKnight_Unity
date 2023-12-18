using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enalbe : MonoBehaviour
{

    [SerializeField] private GameObject a;

    private void Awake()
    {
        a.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(good());
    }

    private IEnumerator good()
    {
        yield return new WaitForSeconds(0.5f);
        a.SetActive(true);
    }
}
