using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRaycast : MonoBehaviour
{

    public GameObject enemyGameObject;


    
    public void Shoot(Vector3 shootPosition, Vector3 shootDirection)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(shootPosition, shootDirection);

        if (raycastHit2D.collider != null)
        {
            //Hit dat target
            
            GameObject enemyObject = raycastHit2D.collider.gameObject;
            

            EnemyAnimal enemyAnimal = enemyObject.GetComponentInParent<EnemyAnimal>();
            //if (enemyObject.TryGetComponent<EnemyAnimal>(out var enemyAnimal))
            //{
            //    Debug.Log("Can't find Script, EnemyAnimal");
            //}

            if (raycastHit2D.collider.CompareTag("Enemy"))
            {
                Debug.Log("Hit animal" + enemyObject + "  sd" + enemyAnimal);
                //Debug.Log("Animal " + enemyAnimal.data.health + "  " + enemyAnimal.currentHealth);
                //enemyAnimal.data.health = 10;
                enemyAnimal.Damage();
            }
        }
    }
}
