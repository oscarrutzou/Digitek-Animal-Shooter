using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class EnemyOwnData : MonoBehaviour
{
    public int currentHealth;
    public int score;

    public bool alive;

    public InGameDisplay inGameDisplay;

    private void Start()
    {
        inGameDisplay = FindObjectOfType<InGameDisplay>();
        alive = true;
    }

    public void Damage()
    {
        //Inds�t weapondamage her.
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

        alive = false;

        //�ndre s� gameobject kommer ind i en pool igen.
        Destroy(gameObject);

    }

}
