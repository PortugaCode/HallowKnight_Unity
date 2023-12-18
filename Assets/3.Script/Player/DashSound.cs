using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSound : MonoBehaviour
{
    public AudioSource audio;

    private void Awake()
    {
        TryGetComponent(out audio);
    }
}
