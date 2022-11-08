using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class EnemyAnimal : MonoBehaviour
{
    public EnemyData data;
    [SerializeField] public int currentHealth;

    [SerializeField] EnemyOwnData enemyOwnData;

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

        enemyOwnData = visuals.GetComponent<EnemyOwnData>();

        //EnemyOwnData enemyOwnData = visuals.GetComponentInChildren<EnemyOwnData>();
        ////EnemyOwnData enemyOwnData;
        ////_ = visuals.GetComponent<EnemyOwnData>();

        //if (enemyOwnData = null)
        //{
        //    Debug.LogWarning("Noget galt med at getcomponent i Enemy object.");
        //}
        
        enemyOwnData.currentHealth = data.health;
        enemyOwnData.score = data.score;

    }


}
