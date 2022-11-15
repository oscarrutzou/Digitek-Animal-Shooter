using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;
using UnityEngine.UIElements;

public class EnemyOwnData : MonoBehaviour
{
    public int currentHealth;
    public int score;

    public bool alive;

    public InGameDisplay inGameDisplay;

    private Transform enemyTransform;
    private CircleCollider2D circleCollider;
    private float radius;

    private void Start()
    {
        inGameDisplay = FindObjectOfType<InGameDisplay>();
        enemyTransform = GetComponent<Transform>();
        circleCollider = GetComponent<CircleCollider2D>();
        radius = circleCollider.radius * 2;
        alive = true;

        //OnDrawGizmos();
    }

    public void Damage()
    {
        Debug.Log("Animal health before: " + currentHealth);

        //Indsæt weapondamage her.
        currentHealth -= 10;

        Debug.Log("Animal health after: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("dead");

            OnDeath();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void OnDeath()
    {
        inGameDisplay.currentScore += score;

        inGameDisplay.currentKills += 1;

        alive = false;

        //Spawn dødslyd?

        Destroy(gameObject);

    }

}
