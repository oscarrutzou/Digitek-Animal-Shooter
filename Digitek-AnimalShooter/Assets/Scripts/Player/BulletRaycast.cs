using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRaycast : MonoBehaviour
{

    [HideInInspector] public GameObject enemyGameObject;


    public void Shoot(Vector3 shootPosition, Vector3 shootDirection)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(shootPosition, shootDirection);

        if (raycastHit2D.collider != null)
        {
            //Hit dat target
            
            
            //if (enemyObject.TryGetComponent<EnemyAnimal>(out var enemyAnimal))
            //{
            //    Debug.Log("Can't find Script, EnemyAnimal");
            //}

            if (raycastHit2D.collider.CompareTag("Enemy"))
            {
                GameObject enemyObject = raycastHit2D.collider.gameObject;


                EnemyOwnData enemyOwnData = enemyObject.GetComponentInParent<EnemyOwnData>();


                if (enemyOwnData.currentHealth > 0)
                {
                    enemyOwnData.Damage();
                }
                else
                {
                    Debug.Log("Animal er død");
                    //Debug.LogWarning("Der er noget glat med at den ikke registrer* damage og health på animal ordenligt");
                    return;
                }
                //else if (enemyAnimal.currentHealth <= 0)
                //{
                //    inGameDisplay.currentScore += 5;

                //    inGameDisplay.currentKills += 1;
                    
                //}

                
            }

        }
    }
}
