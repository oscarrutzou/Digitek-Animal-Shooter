using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : MonoBehaviour
{
    public bool hasTakenDamage = false;
    private bool stopCoroutine = false;

    public Collider2D[] colliderEnemy;
    [SerializeField] private float radius = 5;
    [SerializeField] private int damage = 500;

    [SerializeField] private float timeBeforeBlowUp = 5f;

    public LayerMask enemyMask;

    EnemyOwnData enemyOwnData;

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


    IEnumerator BlowUp()
    {
        //Play sissel lyd, tag dynamit lyd

        yield return new WaitForSeconds(timeBeforeBlowUp);

        if (colliderEnemy.Length != 0)
        {
            for (int i = 0; i < colliderEnemy.Length; i++)
            {
                enemyOwnData = colliderEnemy[i].GetComponent<EnemyOwnData>();
                enemyOwnData.Damage(damage);
                Debug.Log(damage + "  " + enemyOwnData);
                //colliderEnemy[i].Ge
            }
        }

        //Play blow up lyd
        //Maybe lav particels

        Destroy(this.gameObject);

    }



}
