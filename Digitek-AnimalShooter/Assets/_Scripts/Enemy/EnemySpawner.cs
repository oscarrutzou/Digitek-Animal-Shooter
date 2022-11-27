using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using UnityEditor;
using Unity.VisualScripting;

public class EnemySpawner : MonoBehaviour
{
    public EnemyData[] data;
    [SerializeField] EnemyOwnData enemyOwnData;

    public GameObject visuals;

    [Header("Generation")]
    [SerializeField] bool randomGeneration;
    [SerializeField] int enemyArrayNumber;

    [Header("Spawn")]
    //[SerializeField] int randomDataNumber;
    [SerializeField] float visualRadius;


    [SerializeField] Transform pos1;
    [SerializeField] Transform pos2;

    [SerializeField] private bool firstTimeSpawn = true;

    private bool isSpawning = false;

    private float minPosX;
    private float minPosY;
    private float maxPosX;
    private float maxPosY;

    private Vector2 position;

    [SerializeField] private float currentTime;
    [SerializeField] private float spawnAfterTime = 5f; 


    //Object
    [Header("Objects")]
    [SerializeField] public int objectActive;
    [SerializeField] int maxObjects = 5;

    public Collider2D[] colliderEnemy;
    public LayerMask mask;

    private void Awake()
    {
        objectActive = 0;

        
        minPosX = pos1.position.x;
        maxPosX = pos2.position.x;
        minPosY = pos1.position.y;
        maxPosY = pos2.position.y;

        currentTime = spawnAfterTime;

        CheckCollider(new Vector2(-2, -2), 1, mask);
    }

    private void Update()
    {
        if (data == null)
        {
            enemyOwnData = null;
            visuals = null;
            return;
        }

        if (objectActive == 0 && !isSpawning)
        {
            if (firstTimeSpawn)
            {
                isSpawning = true;
                firstTimeSpawn = false;

                //Debug.Log("StartSpawner");

                StartCoroutine(StartSpawner());
            }
            else if (!firstTimeSpawn)
            {

                currentTime -= Time.deltaTime;

                //Invoke("StartSpawner", 2f);
                if (currentTime <= 0)
                {
                    isSpawning = true;
                    //Debug.Log("StartSpawner after time");
                    currentTime = spawnAfterTime;
                    StartCoroutine(StartSpawner());

                }
            }
        }
    }

    IEnumerator StartSpawner()
    {
        for (int i = objectActive; i < maxObjects; i++)
        {
            SpawnVisuals();
            yield return null; //Null er en frame
        }

        isSpawning = false;
    }



    void SpawnVisuals()
    {
        if (randomGeneration)
        {
            enemyArrayNumber = Random.Range(0, data.Length);
        }

        //randomDataNumber = Random.Range(0, data.Length);

        visuals = data[enemyArrayNumber].enemyModel;
        
        visualRadius = visuals.GetComponent<CircleCollider2D>().radius;
        enemyOwnData = visuals.GetComponent<EnemyOwnData>();

        //Debug.Log("Raduis " + visualRadius);

        GenerateRandomPosition();


        if (!CheckCollider(position, visualRadius, mask))
        {
            for (int i = 0; i < 100; i++)
            {
                GenerateRandomPosition();


                if (CheckCollider(position, visualRadius, mask))
                {
                    break;
                }
            }
        }

        //Load current enemy visuals
        visuals = Instantiate(data[enemyArrayNumber].enemyModel);

        visuals.name += (objectActive + 1);

        visuals.transform.localPosition = position;
        //Debug.Log("position " + visuals.transform.localPosition);

        visuals.transform.rotation = Quaternion.identity;

        //Sætter først til et child object her, da position ville være forkert hvis den kaldes før.
        visuals.transform.SetParent(this.transform);

        enemyOwnData.currentHealth = data[enemyArrayNumber].health;
        enemyOwnData.score = data[enemyArrayNumber].score;

        objectActive++; 
    }



    public bool CheckCollider(Vector2 position, float radius, LayerMask layerMask)
    {
        colliderEnemy = Physics2D.OverlapCircleAll(position, radius, layerMask);
        //Debug.Log("EnemyOwnData collider længde" + " :  " + colliderEnemy.Length);

        if (colliderEnemy.Length == 0)
        {
            //Debug.Log("true" + objectActive);
            return true;
        }
        else
        {
            //Debug.Log("False" + objectActive);
            return false;
        }
    }

    void GenerateRandomPosition()
    {
        //Lavet som en random range.

        float positionX = Random.Range(minPosX, maxPosX);

        float positionY = Random.Range(minPosY, maxPosY);

        //Sætter positionen til det random stykke.
        position = new Vector2(positionX, positionY);
        //Debug.Log("Position " + position);
    }


}
