using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRaycast : MonoBehaviour
{

    [HideInInspector] public GameObject enemyGameObject;

    [SerializeField] EnemyOwnData enemyOwnData;

    [SerializeField] PlayerAimWeapon playerAimWeapon;

    public void Shoot(Vector3 shootPosition, Vector3 shootDirection)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(shootPosition, shootDirection);

        

        if (raycastHit2D.collider != null)
        {
            //Hit dat target
            //Debug.Log(raycastHit2D.collider.gameObject);

            if (raycastHit2D.collider.CompareTag("Enemy"))
            {
                enemyGameObject = null;
                enemyOwnData = null;

                GameObject enemyObject = raycastHit2D.collider.gameObject;

                //Debug.Log("GAmeObject + " + enemyObject);


                enemyOwnData = enemyObject.GetComponent<EnemyOwnData>();
                Debug.Log(enemyOwnData);


                if (enemyOwnData.currentHealth > 0)
                {
                    enemyOwnData.Damage(playerAimWeapon._damage);
                }
                //else
                //{
                //    Debug.Log("Animal er død");
                //    return;
                //}
            }
        }
    }
}
