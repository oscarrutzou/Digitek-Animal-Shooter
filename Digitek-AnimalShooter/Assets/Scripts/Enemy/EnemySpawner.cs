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
    [SerializeField] int randomDataNumber;
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

    //Object
    [Header("Objects")]
    [SerializeField] public int objectActive;
    [SerializeField] int maxObjects = 5;

    public Collider2D[] colliderEnemy;
    public LayerMask mask;

    private void Awake()
    {
        objectActive = 0;

        //Bare brug transform i stedet for et gameobject!!!
        minPosX = pos1.position.x;
        maxPosX = pos2.position.x;
        minPosY = pos1.position.y;
        maxPosY = pos2.position.y;


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

        //Debug.Log(objectActive);

        //Sørger for at der ikke går et antal tid før den spawner
        if (objectActive == 0 && firstTimeSpawn)
        {
            //Sæt at den spawner til et nyt tal.
            if (isSpawning == false)
            {
                isSpawning = true;
                firstTimeSpawn = false;

                Debug.Log("StartSpawner");

                StartCoroutine(StartSpawner());
                
            }

        }
        else if (objectActive == 0 && !firstTimeSpawn)
        {
            //Start spawner efter et antal sek.
            //Start timer

            if (isSpawning == false)
            {
                isSpawning = true;

                //Invoke("StartSpawner", 2f);
                //if (currentTime >= spawnAfterTime)
                //{
                //    Debug.Log("StartSpawner after time");

                //    //StartSpawner();
                //}
            }

            //Så start spawner
        }

    }

    IEnumerator StartSpawner()
    {
        if (objectActive == 0 || objectActive <= maxObjects)
        {
            //Spawn 
            //Debug.Log("ObjectActive" + (objectActive + 1));

            for (int i = objectActive; i < maxObjects; i++)
            {
                if (randomGeneration)
                {
                    //Debug.Log("randomgeneration " + randomGeneration);

                    SpawnRandom();
                    yield return null; //Null er en frame
                }
                else
                {
                    //Debug.Log("randomgeneration " + randomGeneration);
                    //SpawnNormal(enemyArrayNumber);

                }

                //objectActive++;
                //Debug.Log(objectActive);
            }

            isSpawning = false;
        }
    }



    void SpawnRandom()
    {
        randomDataNumber = Random.Range(0, data.Length);

        visuals = data[randomDataNumber].enemyModel;
        
        visualRadius = visuals.GetComponent<CircleCollider2D>().radius;
        enemyOwnData = visuals.GetComponent<EnemyOwnData>();

        Debug.Log("Raduis " + visualRadius);

        GenerateRandomPosition();


        if (!CheckCollider(position, visualRadius, mask))
        {
            //Debug.Log("enemyOwnData.colliderEnemy.Length > 1 :::: " + enemyOwnData.colliderEnemy.Length);

            for (int i = 0; i < 100; i++)
            {
                GenerateRandomPosition();


                if (CheckCollider(position, visualRadius, mask))
                {
                    //Debug.Log("enemyOwnData.colliderEnemy.Length == 1 :::: " + enemyOwnData.colliderEnemy.Length);
                    break;
                }
            }
        }

        //Load current enemy visuals
        visuals = Instantiate(data[randomDataNumber].enemyModel);

        visuals.name += (objectActive + 1);

        visuals.transform.localPosition = position;
        Debug.Log("position " + visuals.transform.localPosition);

        visuals.transform.rotation = Quaternion.identity;

        //Sætter først til et child object her, da position ville være forkert hvis den kaldes før.
        visuals.transform.SetParent(this.transform);

        enemyOwnData.currentHealth = data[randomDataNumber].health;
        enemyOwnData.score = data[randomDataNumber].score;

        objectActive++;
        //enemyOwnData = null;
        //visuals = null;

        //if (colliderEnemy.Length == 0)
        //{
        //    Debug.Log("videre til næste " + visuals.name);
            
        //}
        //else
        //{
        //    Debug.LogWarning("FEJL ikke tager længde ordenlig maybe");
            
        //}

        
    }

    public bool CheckCollider(Vector2 position, float radius, LayerMask layerMask)
    {
        colliderEnemy = Physics2D.OverlapCircleAll(position, radius, layerMask);
        Debug.Log("EnemyOwnData collider længde" + " :  " + colliderEnemy.Length);

        if (colliderEnemy.Length == 0)
        {
            Debug.Log("true" + objectActive);
            return true;
        }
        else
        {
            Debug.Log("False" + objectActive);
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
        Debug.Log("Position " + position);
    }


}
