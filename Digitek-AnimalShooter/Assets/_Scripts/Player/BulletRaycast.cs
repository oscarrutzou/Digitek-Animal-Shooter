using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRaycast : MonoBehaviour
{

    [HideInInspector] public GameObject enemyGameObject;

    [SerializeField] EnemyOwnData enemyOwnData;

    [SerializeField] PlayerAimWeapon playerAimWeapon;
    [SerializeField] private float changeColorTime = 0.09f;

    public void Shoot(Vector3 shootPosition, Vector3 shootDirection, float distance)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(shootPosition, shootDirection, distance);

        if (raycastHit2D.collider != null)
        {
            if (raycastHit2D.collider.CompareTag("Enemy"))
            {
                
                GameObject enemyObject = raycastHit2D.collider.gameObject;

                enemyOwnData = enemyObject.GetComponent<EnemyOwnData>();

                if (enemyOwnData.currentHealth > 0)
                {
                    enemyOwnData.Damage(playerAimWeapon._damage, changeColorTime, false);
                }

                enemyObject = null;
                enemyOwnData = null;
            }

            else if (raycastHit2D.collider.CompareTag("DestroyAbleObjects"))
            {

                GameObject destroyAbleObject = raycastHit2D.collider.gameObject;

                if (destroyAbleObject.TryGetComponent(out TNT tnt))
                {
                    tnt.hasTakenDamage = true;
                    
                }

                destroyAbleObject = null;
                tnt = null;
            }
        }
    }
}
