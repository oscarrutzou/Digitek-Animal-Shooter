using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class EnemyAnimal : MonoBehaviour
{
    public EnemyData data;
    [SerializeField] public int currentHealth;

    private void Start()
    {
        if (data != null)
        {
            LoadEnemy(data);
        }
    }

    private void LoadEnemy(EnemyData _data)
    {
        //Remove children objects i.e. visuals
        foreach (Transform child in this.transform)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(child.gameObject);
            }
            else
            {
                Destroy(child.gameObject);
            }
        }

        //Load current enemy visuals
        GameObject visuals = Instantiate(data.enemyModel);
        visuals.transform.SetParent(this.transform);
        visuals.transform.localPosition = Vector3.zero;
        visuals.transform.rotation = Quaternion.identity;

        currentHealth = data.health;
        Debug.Log("currentHealth " + currentHealth);
    }

    public void Damage()
    {
        //Indsæt weapondamage her.
        Debug.Log("Animal health before: " + currentHealth);

        currentHealth -= 10;
        Debug.Log("Animal health after: " + currentHealth);
    }
}
