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
    
    public Collider2D[] colliderEnemy;
    private float radius;

    EnemySpawner enemySpawner;

    public LayerMask enemyMask;

    private void Awake()
    {
        inGameDisplay = FindObjectOfType<InGameDisplay>();
        enemyTransform = GetComponent<Transform>();
        circleCollider = GetComponent<CircleCollider2D>();
        radius = circleCollider.radius;
        alive = true;

        enemySpawner = FindObjectOfType<EnemySpawner>();
        
    }

    private void Update()
    {

        colliderEnemy = Physics2D.OverlapCircleAll(transform.position, radius, enemyMask);
        //Debug.Log("EnemyOwnData collider længde" + name + " :  " + colliderEnemy.Length);



        //If længde er over >= 2 find nyt punkt
    }

    //private bool CheckCollider()
    //{
    //    colliderEnemy = Physics2D.OverlapCircleAll(transform.position, radius, enemyMask);
    //    Debug.Log("EnemyOwnData collider længde" + name + " :  " + colliderEnemy.Length);

    //    if (colliderEnemy.Length == 1)
    //    {
    //        Debug.Log("true");
    //        return true;
    //    }
    //    else
    //    {
    //        Debug.Log("False");
    //        return false;
    //    }
    //}

    public void Damage(int damage)
    {
        Debug.Log("Animal health before: " + currentHealth);

        //Indsæt weapondamage her.
        currentHealth -= damage;

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

        enemySpawner.objectActive--;
        //Spawn dødslyd?

        Destroy(gameObject);

    }

}
