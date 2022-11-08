using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class EnemyOwnData : MonoBehaviour
{
    public int currentHealth;
    public int score;

    public InGameDisplay inGameDisplay;

    private void Start()
    {
        inGameDisplay = FindObjectOfType<InGameDisplay>();

    }

    public void Damage()
    {
        //Indsæt weapondamage her.
        Debug.Log("Animal health before: " + currentHealth);

        currentHealth -= 10;

        Debug.Log("Animal health after: " + currentHealth);

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        inGameDisplay.currentScore += score;

        inGameDisplay.currentKills += 1;

        //Put gameobject væk.
        

    }

}
