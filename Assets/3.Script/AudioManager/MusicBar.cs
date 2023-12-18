using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicBar : MonoBehaviour
{
    public Slider musicbar;

    private void Awake()
    {
        TryGetComponent(out musicbar);
    }

    private void Update()
    {
        AudioManager.Instance.musicSource.volume = musicbar.value;
    }
}
