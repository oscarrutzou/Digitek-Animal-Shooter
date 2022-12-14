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

        if (objectActive <= 0 && !isSpawning)
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
                //Debug.Log("Spawner igen" + currentTime);
                currentTime -= Time.deltaTime;

                //Invoke("StartSpawner", 2f);
                if (currentTime <= 0)
                {
                    isSpawning = true;
                    //Debug.Log("StartSpawner after time");
                    currentTime = spawnAfterTime;
                    objectActive = 0;
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
        //Hvis den skal spawne random s? v?lger den et nyt nummer i array hver gang.
        if (randomGeneration) 
        {
            enemyArrayNumber = Random.Range(0, data.Length);
        }

        //S?tter gameobject til det som ligger i dataen fra scribtebel objects.
        visuals = data[enemyArrayNumber].enemyModel;
        
        //Finder radius hos en circleCollider2D og et script til senere brug.
        visualRadius = visuals.GetComponent<CircleCollider2D>().radius;
        enemyOwnData = visuals.GetComponent<EnemyOwnData>();

        //Laver random Vector2 koordinator.
        GenerateRandomPosition();

        //S?rger for der ikke er noget p? positionen allerede.
        if (!CheckCollider(position, visualRadius, mask))
        {
            for (int i = 0; i < 100; i++) //Maks tjekker 100 gange
            {
                //Laver ny position
                GenerateRandomPosition();
                
                //Tjekker at der ikke st?r noget der p?, ogs? bryder ud hvis den er true
                if (CheckCollider(position, visualRadius, mask))
                {
                    break;
                }
            }
        }

        //Spawner dyret. Ville godt have brugt pools her i stedet for.
        visuals = Instantiate(data[enemyArrayNumber].enemyModel);

        visuals.name += (objectActive + 1);

        //S?tter positionen til den random position.
        visuals.transform.localPosition = position;
        //S?tter rotation til 0.
        visuals.transform.rotation = Quaternion.identity;

        //S?tter f?rst til et child object her, da position ville v?re forkert hvis den kaldes f?r.
        visuals.transform.SetParent(this.transform);

        //Loader liv og scoren p? gameobjectet.
        enemyOwnData.currentHealth = data[enemyArrayNumber].health;
        enemyOwnData.score = data[enemyArrayNumber].score;

        //Plusser p? objectActive for at kunne s?rge for at maks. spawne et hvis antal objecter.
        objectActive++; 
    }



    public bool CheckCollider(Vector2 position, float radius, LayerMask layerMask)
    {
        //Laver circel med bestemt position, radius og layer. Laver et Collider2D array.
        colliderEnemy = Physics2D.OverlapCircleAll(position, radius, layerMask);

        //For at tjekke om den fandt noget. Ja s? skal den v?re true, eller false.
        if (colliderEnemy.Length == 0)
            return true;
        else
            return false;
    }

    void GenerateRandomPosition()
    {
        //Lavet som en random range.

        float positionX = Random.Range(minPosX, maxPosX);

        float positionY = Random.Range(minPosY, maxPosY);

        //S?tter positionen til det random stykke.
        position = new Vector2(positionX, positionY);
        //Debug.Log("Position " + position);
    }


}
