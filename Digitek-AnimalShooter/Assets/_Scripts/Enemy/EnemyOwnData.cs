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
    SpriteRenderer spriteRenderer;

    public LayerMask enemyMask;

    private void Awake()
    {
        inGameDisplay = FindObjectOfType<InGameDisplay>();
        enemyTransform = GetComponent<Transform>();
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        radius = circleCollider.radius;
        alive = true;

        enemySpawner = FindObjectOfType<EnemySpawner>();
        
    }

    //private void Update()
    //{

    //    colliderEnemy = Physics2D.OverlapCircleAll(transform.position, radius, enemyMask);

    //}

    IEnumerator ChangeColor(float changeColorTime)
    {
        Color colorBefore = spriteRenderer.color;
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(changeColorTime);

        spriteRenderer.color = colorBefore;
    }

    public void Damage(int damage, float changeColorTime, bool explosion)
    {
        if (!explosion)  StartCoroutine(ChangeColor(changeColorTime));

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
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
