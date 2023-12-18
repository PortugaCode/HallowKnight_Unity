using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonControll : MonoBehaviour
{
    [SerializeField] GameObject Pause;
    [SerializeField] Button button;



    public void GameStart()
    {
        StartCoroutine(GameStart_co());
    }

    private IEnumerator GameStart_co()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("Stage01");
    }


    public void Intro()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.PlayMusic("Inside Theme");
        SceneManager.LoadScene("Intro", LoadSceneMode.Single);
    }

    public void Resume()
    {
        Cursor.visible = false;
        Time.timeScale = 1f;
        button.enabled = false;
        Pause.SetActive(false);
    }

}
