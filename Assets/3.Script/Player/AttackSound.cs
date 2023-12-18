using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSound : MonoBehaviour
{
    public AudioSource audio;

    private void Awake()
    {
        TryGetComponent(out audio);
    }
}
