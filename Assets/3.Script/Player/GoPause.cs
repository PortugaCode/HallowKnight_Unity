using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoPause : MonoBehaviour
{
    [SerializeField] GameObject Pause;
    [SerializeField] Button button;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) )
        {
            button.enabled = true;
            Cursor.visible = true;
            Pause.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
