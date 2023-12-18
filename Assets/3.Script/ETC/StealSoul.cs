using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StealSoul : MonoBehaviour
{
    [SerializeField] PlayerConroll player;


    public int score;
    public int maxscore;

    public Image[] images;


    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);
    }

    private void Update()
    {
        score = player.score;
        maxscore = player.maxscore;


        if (score == 0)
        {
            images[0].enabled = true;
            images[1].enabled = false;
            images[2].enabled = false;
            images[3].enabled = false;
            images[4].enabled = false;
        }
        else if (score == 1)
        {
            images[0].enabled = false;
            images[1].enabled = true;
            images[2].enabled = false;
            images[3].enabled = false;
            images[4].enabled = false;
        }
        else if (score == 2)
        {
            images[0].enabled = false;
            images[1].enabled = false;
            images[2].enabled = true;
            images[3].enabled = false;
            images[4].enabled = false;
        }
        else if (score == 3)
        {
            images[0].enabled = false;
            images[1].enabled = false;
            images[2].enabled = false;
            images[3].enabled = true;
            images[4].enabled = false;
        }
        else if (score == 4)
        {
            images[0].enabled = false;
            images[1].enabled = false;
            images[2].enabled = false;
            images[3].enabled = false;
            images[4].enabled = true;
        }
    }
}
