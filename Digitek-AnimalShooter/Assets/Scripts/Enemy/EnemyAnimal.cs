using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class EnemyAnimal : MonoBehaviour
{
    public EnemyData[] data;
    [SerializeField] EnemyOwnData enemyOwnData;

    public GameObject visuals;

    private void Start()
    {
        if (data != null)
        {
            LoadEnemy(data[0]);
        }
    }

    private void Update()
    {
        if (data == null)
        {
            enemyOwnData = null;
            visuals = null;
            return;
        }
    }

    //Lav en spawner som skriver loadEnemy f.eks. 20 sek efter den er død.
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
        //Måske lav et child gameobject og load visuals på den?


        //Load current enemy visuals
        visuals = Instantiate(data[0].enemyModel);
        visuals.transform.SetParent(this.transform);
        visuals.transform.localPosition = Vector3.zero;
        visuals.transform.rotation = Quaternion.identity;

        enemyOwnData = visuals.GetComponent<EnemyOwnData>();

        enemyOwnData.currentHealth = data[0].health;
        enemyOwnData.score = data[0].score;
    }


}
