using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TNT : MonoBehaviour
{
    public bool hasTakenDamage = false;
    private bool stopCoroutine = false;

    public Collider2D[] colliderEnemy;
    [SerializeField] private float radius = 5;
    [SerializeField] private int damage = 500;

    [SerializeField] private float timeBeforeBlowUp = 5f;
    [SerializeField] private float changeColorTime = 0.09f;

    public LayerMask enemyMask;

    EnemyOwnData enemyOwnData;
    Animator animator;
    SpriteRenderer spriteRenderer;
    ShadowCaster2D shadowCaster;
    Collider2D ownCollider;
    GameObject childObject;
    AudioManager audioManager;


    private void Start()
    {
        shadowCaster = GetComponent<ShadowCaster2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ownCollider = GetComponent<Collider2D>();
        childObject = this.gameObject.transform.GetChild(0).gameObject;
        animator = childObject.GetComponent<Animator>();
        audioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;

    }

    private void Update()
    {
        if (hasTakenDamage)
        {
            colliderEnemy = Physics2D.OverlapCircleAll(transform.position, radius, enemyMask);

            if (!stopCoroutine)
            {
                stopCoroutine = true;
                StartCoroutine(BlowUp());
            }
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

    IEnumerator BlowUp()
    {
        StartCoroutine(ChangeColor(changeColorTime));
        //Play sissel lyd, tag dynamit lyd
        yield return new WaitForSeconds(timeBeforeBlowUp);

        if (colliderEnemy.Length != 0)
        {
            for (int i = 0; i < colliderEnemy.Length; i++)
            {
                enemyOwnData = colliderEnemy[i].GetComponent<EnemyOwnData>();
                enemyOwnData.Damage(damage, 0, true);
                Debug.Log(damage + "  " + enemyOwnData);
            }
        }

        //Play blow up lyd
        //Maybe lav particels
        spriteRenderer.enabled = false;
        shadowCaster.enabled = false;
        ownCollider.enabled = false;

        childObject.SetActive(true);

        
        animator.SetBool("hitByShot", true);
        
        

       
        yield return new WaitForSeconds(1.2f);

        Destroy(this.gameObject);
    }



}
