using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    [SerializeField] new Camera camera;

    Vector3 target;

    private void Awake()
    {
        Cursor.visible = true;
        TryGetComponent(out camera);
    }

/*    private void Update()
    {
        SetCursorPosition();
    }

    private void SetCursorPosition()
    {
        target = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        editCursor.transform.position = target;
    }
*/

    public void GameStart()
    {
        StartCoroutine(GameStart_co());
    }

    public void Option()
    {
        StartCoroutine(Option_co());
    }

    public void Intro()
    {
        StartCoroutine(Intro_co());
    }
    public void GameExit()
    {
        StartCoroutine(GameExit_co());
    }

    private IEnumerator GameExit_co()
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("∞‘¿” ≥°");
        Application.Quit();
    }


    private IEnumerator GameStart_co()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("Stage01");
    }

    private IEnumerator Option_co()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("Intro_Option");
    }

    private IEnumerator Intro_co()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("Intro");
    }

}
