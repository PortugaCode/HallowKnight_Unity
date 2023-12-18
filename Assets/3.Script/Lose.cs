using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lose : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Win_co());
    }

    private IEnumerator Win_co()
    {
        yield return new WaitForSeconds(4f);
        AudioManager.Instance.PlayMusic("Inside Theme");
        SceneManager.LoadScene("Intro");
    }
}
