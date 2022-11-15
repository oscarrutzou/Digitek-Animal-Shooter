using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using UnityEditor;
using Unity.VisualScripting;

public class EnemyAnimal : MonoBehaviour
{
    public EnemyData[] data;
    [SerializeField] EnemyOwnData enemyOwnData;

    public GameObject visuals;

    [Header("Generation")]
    [SerializeField] bool randomGeneration;
    [SerializeField] int enemyArrayNumber;

    [Header("Spawn")]
    [SerializeField] int randomDataNumber;
    [SerializeField] float visualRaduis;


    [SerializeField] GameObject pos1;
    [SerializeField] GameObject pos2;

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


    //Collision
    Collider2D CollisionWithPlayer;
    Collider2D CollisionWithEnemy;
    private float enemyRadius;

    private void Awake()
    {
        objectActive = 0;

        minPosX = pos1.GetComponentInChildren<Transform>().position.x;
        maxPosX = pos2.GetComponentInChildren<Transform>().position.x;
        minPosY = pos1.GetComponentInChildren<Transform>().position.y;
        maxPosY = pos2.GetComponentInChildren<Transform>().position.y;

    }

    private void Start()
    {
        //if (data != null)
        //{
        //    if (randomGeneration)
        //    {
        //        StartCoroutine(SpawnRandom());
        //    }
        //    else
        //    {
        //        StartCoroutine(SpawnNormal(enemyArrayNumber));

        //    }

        //}

        //StartSpawner();
    }

    private void Update()
    {
        if (data == null)
        {
            enemyOwnData = null;
            visuals = null;
            return;
        }

        //Sørger for at der ikke går et antal tid før den spawner
        if (objectActive == 0 && firstTimeSpawn)
        {
            //Sæt at den spawner til et nyt tal.
            if (isSpawning == false)
            {
                isSpawning = true;
                firstTimeSpawn = false;

                Debug.Log("StartSpawner");

                StartSpawner();
            }

        }
        else if (objectActive == 0 && !firstTimeSpawn)
        {
            //Start spawner efter et antal sek.
            //Start timer

            if (isSpawning == false)
            {
                isSpawning = true;

                Invoke("StartSpawner", 2f);
                //if (currentTime >= spawnAfterTime)
                //{
                //    Debug.Log("StartSpawner after time");

                //    //StartSpawner();
                //}
            }

            //Så start spawner
        }

    }


    void StartSpawner()
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

    void GenerateRandomPosition()
    {
        //Lavet som en random range.

        float positionX = Random.Range(minPosX, maxPosX);

        float positionY = Random.Range(minPosY, maxPosY);

        //Sætter positionen af det skrald til det random stykke.
        position = new Vector2(positionX, positionY);
        //Debug.Log("Position " + position);
    }

    void CheckCollision(float actualRadius)
    {
        //Debug.Log("actualRadius " + actualRadius);

        

        CollisionWithEnemy = Physics2D.OverlapCircle(position, actualRadius, LayerMask.GetMask("EnemyLayer"));
        CollisionWithPlayer = Physics2D.OverlapCircle(position, actualRadius, LayerMask.GetMask("Player"));

        //Debug.Log("InCheckCollision:" + " Position " + position + " ObjectACtive " + (objectActive + 1) + ": CollisionWithEnemy = " + CollisionWithEnemy + " : CollisionWithPlayer = " + CollisionWithPlayer);
    }




    void SpawnRandom()
    {
        //Debug.Log("Rigtig pos " + position);

        randomDataNumber = Random.Range(0, data.Length);

        visuals = data[randomDataNumber].enemyModel;
        visualRaduis = visuals.GetComponent<CircleCollider2D>().radius * 2;
        //Debug.Log("VisualRaduis " + visualRaduis);
        //float enemyRadius = visuals.GetComponent<Collider2D>().bounds.max.x;
        

        GenerateRandomPosition();
        CheckCollision(visualRaduis);
        //Debug.Log("visuals " + visuals + "  enemyRadius " + enemyRadius);



        if (CollisionWithEnemy == true || CollisionWithPlayer == true)
        {

            for (int i = 0; i < 40; i++)
            {
                position = new Vector2(0,0);
                CollisionWithEnemy = null;
                CollisionWithPlayer = null;

                GenerateRandomPosition();
                CheckCollision(visualRaduis);
                Debug.Log(" 211: CollisionWithEnemy= " + CollisionWithEnemy + "  eller CollisionWithPlayer = " + CollisionWithPlayer);

                if (!CollisionWithEnemy && !CollisionWithPlayer)
                {
                    Debug.Log(" 215: CollisionWithEnemy= " + CollisionWithEnemy + "  eller CollisionWithPlayer = " + CollisionWithPlayer);

                    break;
                }   
            }            
        }

        CheckCollision(visualRaduis);

        if (CollisionWithEnemy == null && CollisionWithPlayer == null)
        {
            //Debug.Log(" 226: CollisionWithEnemy= " + CollisionWithEnemy + "  eller CollisionWithPlayer = " + CollisionWithPlayer);
            Debug.Log(" Spawning visuals: " + (objectActive + 1));

            //Load current enemy visuals
            visuals = Instantiate(data[randomDataNumber].enemyModel);
            visuals.transform.localPosition = position;
            visuals.transform.rotation = Quaternion.identity;
            //Sætter først til et child object her, da position ville være forkert hvis den kaldes før.
            visuals.transform.SetParent(this.transform);

            enemyOwnData = visuals.GetComponent<EnemyOwnData>();

            enemyOwnData.currentHealth = data[randomDataNumber].health;
            enemyOwnData.score = data[randomDataNumber].score;

            objectActive++;
            //Debug.Log("ObjectActive" + (objectActive + 1));

        }
        //else
        //{
        //    return;
        //}

    }

    //void SpawnNormal(int enemyNumber)
    //{
    //    //Lavet som en random range.

    //    float positionX = Random.Range(minPosX, maxPosX);

    //    float positionY = Random.Range(minPosY, maxPosY);

    //    //Sætter positionen af det skrald til det random stykke.
    //    var position = new Vector3(positionX, positionY, 0);
    //    Debug.Log("Rigtig pos " + position);


    //    //Load current enemy visuals
    //    visuals = Instantiate(data[enemyNumber].enemyModel);
    //    visuals.transform.localPosition = position;
    //    visuals.transform.rotation = Quaternion.identity;
    //    //Sætter først til et child object her, da position ville være forkert hvis den kaldes før.
    //    visuals.transform.SetParent(this.transform);



    //    enemyOwnData = visuals.GetComponent<EnemyOwnData>();

    //    enemyOwnData.currentHealth = data[enemyNumber].health;
    //    enemyOwnData.score = data[enemyNumber].score;
    //}




    //Spawn random object.
    //IEnumerator SpawnRandom()
    //{
    //    //Den venter et antal sekunder
    //    yield return new WaitForSeconds(timeBetweenSpawn);

    //    //Lavet som en random range.

    //    float positionX = Random.Range(minPosX, maxPosX);

    //    float positionY = Random.Range(minPosY, maxPosY);

    //    //Sætter positionen af det skrald til det random stykke.
    //    var position = new Vector3(positionX, positionY, 0);
    //    Debug.Log("Rigtig pos " + position);


    //    randomDataNumber =  Random.Range(0, data.Length);

    //    //Load current enemy visuals
    //    visuals = Instantiate(data[randomDataNumber].enemyModel);
    //    visuals.transform.localPosition = position;
    //    visuals.transform.rotation = Quaternion.identity;
    //    //Sætter først til et child object her, da position ville være forkert hvis den kaldes før.
    //    visuals.transform.SetParent(this.transform);



    //    enemyOwnData = visuals.GetComponent<EnemyOwnData>();

    //    enemyOwnData.currentHealth = data[randomDataNumber].health;
    //    enemyOwnData.score = data[randomDataNumber].score;

    //}

    //Spawn specifik objekt.
    //IEnumerator SpawnNormal(int enemyNumber)
    //{
    //    //Den venter et antal sekunder
    //    yield return new WaitForSeconds(timeBetweenSpawn);

    //    //Lavet som en random range.

    //    float positionX = Random.Range(minPosX, maxPosX);

    //    float positionY = Random.Range(minPosY, maxPosY);

    //    //Sætter positionen af det skrald til det random stykke.
    //    var position = new Vector3(positionX, positionY, 0);
    //    Debug.Log("Rigtig pos " + position);


    //    //Load current enemy visuals
    //    visuals = Instantiate(data[enemyNumber].enemyModel);
    //    visuals.transform.localPosition = position;
    //    visuals.transform.rotation = Quaternion.identity;
    //    //Sætter først til et child object her, da position ville være forkert hvis den kaldes før.
    //    visuals.transform.SetParent(this.transform);



    //    enemyOwnData = visuals.GetComponent<EnemyOwnData>();

    //    enemyOwnData.currentHealth = data[enemyNumber].health;
    //    enemyOwnData.score = data[enemyNumber].score;

    //}


}
