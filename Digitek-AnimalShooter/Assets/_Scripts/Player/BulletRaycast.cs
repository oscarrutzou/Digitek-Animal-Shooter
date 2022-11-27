using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRaycast : MonoBehaviour
{

    [HideInInspector] public GameObject enemyGameObject;

    [SerializeField] EnemyOwnData enemyOwnData;

    [SerializeField] PlayerAimWeapon playerAimWeapon;

    public void Shoot(Vector3 shootPosition, Vector3 shootDirection, float distance)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(shootPosition, shootDirection, distance);
        //Debug.Log("shootPosition " + shootPosition + "shootDirection" + shootDirection);
        //Debug.DrawRay(shootPosition, shootDirection, Color.white, 0.2f);

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
                //Debug.Log(enemyOwnData);


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

            else if (raycastHit2D.collider.CompareTag("DestroyAbleObjects"))
            {

                GameObject destroyAbleObject = raycastHit2D.collider.gameObject;

                if (destroyAbleObject.TryGetComponent(out TNT tnt))
                {
                    tnt.hasTakenDamage = true;
                }
                
                
                //TNT tnt = destroyAbleObject.GetComponent<TNT>();

            }
        }
    }
}
