using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(Win_co());
    }

    private IEnumerator Win_co()
    {
        yield return new WaitForSeconds(6f);
        AudioManager.Instance.PlayMusic("Inside Theme");
        SceneManager.LoadScene("Intro");
    }
}
