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
    public LayerMask enemyMask;

    public EnemySpawner enemySpawner;
    SpriteRenderer spriteRenderer;
    Collider2D ownCollider;
    GameObject childObject;
    Animator animator;

    AudioManager audioManager;

    private void Start()
    {
        inGameDisplay = FindObjectOfType<InGameDisplay>();
        enemyTransform = GetComponent<Transform>();
        circleCollider = GetComponent<CircleCollider2D>();
        ownCollider = GetComponent<Collider2D>();
        enemySpawner = GetComponentInParent<EnemySpawner>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        childObject = this.gameObject.transform.GetChild(0).gameObject;
        animator = childObject.GetComponent<Animator>();

        audioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;

        radius = circleCollider.radius;
        alive = true;

    }


    public void Damage(int damage, float changeColorTime, bool explosion)
    {
        if (!explosion)  StartCoroutine(ChangeColor(changeColorTime));

        currentHealth -= damage;

        if (currentHealth <= 0 && alive)
        {
            StartCoroutine(OnDeath());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    IEnumerator ChangeColor(float changeColorTime)
    {
        Color colorBefore = spriteRenderer.color;
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(changeColorTime);

        spriteRenderer.color = colorBefore;
    }

    IEnumerator OnDeath()
    {
        inGameDisplay.currentScore += score;

        inGameDisplay.currentKills += 1;

        alive = false;

        


        spriteRenderer.enabled = false;

        childObject.SetActive(true);


        //Spawn splat lyd
        audioManager.Play("BloodSplat");

        yield return new WaitForSeconds(0.3f);

        enemySpawner.objectActive--;
        Destroy(gameObject);

    }

}
