using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] PlayerConroll player;


    public int health;
    public int numofHeart;

    public Image[] heart;
    public Sprite fullheart;
    public Sprite Emptyheart;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);
    }


    private void Update()
    {
        health = player.curhp;
        numofHeart = player.maxhp;


        if(health >= numofHeart)
        {
            health = numofHeart;
            for (int i = 0; i < heart.Length; i++)
            {
                if (i < health)
                {
                    heart[i].sprite = fullheart;
                }
                else
                {
                    heart[i].sprite = Emptyheart;
                }

                if (i < numofHeart)
                {
                    heart[i].enabled = true;
                }
                else
                {
                    heart[i].enabled = false;
                }
            }
        }
        
        else if(health < numofHeart)
        {
            for (int i = 0; i < heart.Length; i++)
            {
                if (i < health)
                {
                    heart[i].sprite = fullheart;
                }
                else
                {
                    heart[i].sprite = Emptyheart;
                }

                if (i < numofHeart)
                {
                    heart[i].enabled = true;
                }
                else
                {
                    heart[i].enabled = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < heart.Length; i++)
            {
                heart[i].sprite = Emptyheart;
            }
        }
    }
}
