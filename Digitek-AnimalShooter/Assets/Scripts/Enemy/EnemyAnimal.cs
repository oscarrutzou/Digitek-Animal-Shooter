using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemyAnimal : MonoBehaviour
{
    public EnemyData[] data;
    [SerializeField] EnemyOwnData enemyOwnData;

    public GameObject visuals;

    [SerializeField] bool randomGeneration;
    [SerializeField] int enemyArrayNumber;


    [SerializeField] int randomDataNumber;

    //Variabler som skal vises i inspector men ikke skal kaldes af andre scipts
    [SerializeField] float timeBetweenSpawn = 5f;

    [SerializeField] GameObject pos1;
    [SerializeField] GameObject pos2;

    private float minPosX;
    private float minPosY;
    private float maxPosX;
    private float maxPosY;

    private void Awake()
    {
        minPosX = pos1.GetComponentInChildren<Transform>().position.x;
        maxPosX = pos2.GetComponentInChildren<Transform>().position.x;
        minPosY = pos1.GetComponentInChildren<Transform>().position.y;
        maxPosY = pos2.GetComponentInChildren<Transform>().position.y;


        Debug.Log(minPosX + "  " + maxPosX + "  ");
        Debug.Log(minPosY + "  " + maxPosY + "  ");


    }

    //Spawn like 5 - 10 og vent til at alle er døde.
    //Tjek om der er smartere måder at gøre det på.
    //Ellers virker spawner nu, så ik rør ved den før du arbejder med pools!:d

    private void Start()
    {
        if (data != null)
        {
            if (randomGeneration)
            {
                StartCoroutine(SpawnRandom());
            }
            else
            {
                StartCoroutine(SpawnNormal(enemyArrayNumber));

            }

            //StartCoroutine(Spawn());
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

    //Spawn random.
    IEnumerator SpawnRandom()
    {
        //Den venter et antal sekunder
        yield return new WaitForSeconds(timeBetweenSpawn);

        //Lavet som en random range.

        float positionX = Random.Range(minPosX, maxPosX);

        float positionY = Random.Range(minPosY, maxPosY);

        //Sætter positionen af det skrald til det random stykke.
        var position = new Vector3(positionX, positionY, 0);
        Debug.Log("Rigtig pos " + position);


        randomDataNumber =  Random.Range(0, data.Length);

        //Load current enemy visuals
        visuals = Instantiate(data[randomDataNumber].enemyModel);
        visuals.transform.localPosition = position;
        visuals.transform.rotation = Quaternion.identity;
        //Sætter først til et child object her, da position ville være forkert hvis den kaldes før.
        visuals.transform.SetParent(this.transform);



        enemyOwnData = visuals.GetComponent<EnemyOwnData>();

        enemyOwnData.currentHealth = data[randomDataNumber].health;
        enemyOwnData.score = data[randomDataNumber].score;

    }

    //Spawn random.
    IEnumerator SpawnNormal(int enemyNumber)
    {
        //Den venter et antal sekunder
        yield return new WaitForSeconds(timeBetweenSpawn);

        //Lavet som en random range.

        float positionX = Random.Range(minPosX, maxPosX);

        float positionY = Random.Range(minPosY, maxPosY);

        //Sætter positionen af det skrald til det random stykke.
        var position = new Vector3(positionX, positionY, 0);
        Debug.Log("Rigtig pos " + position);


        //Load current enemy visuals
        visuals = Instantiate(data[enemyNumber].enemyModel);
        visuals.transform.localPosition = position;
        visuals.transform.rotation = Quaternion.identity;
        //Sætter først til et child object her, da position ville være forkert hvis den kaldes før.
        visuals.transform.SetParent(this.transform);



        enemyOwnData = visuals.GetComponent<EnemyOwnData>();

        enemyOwnData.currentHealth = data[enemyNumber].health;
        enemyOwnData.score = data[enemyNumber].score;

    }

    //Lav en spawner som skriver loadEnemy f.eks. 20 sek efter den er død.
    //private void CreateEnemy(EnemyData _data)
    //{
    //    //Remove children objects i.e. visuals
    //    foreach (Transform child in this.transform)
    //    {
    //        if (Application.isEditor)
    //        {
    //            DestroyImmediate(child.gameObject);
    //        }
    //        else
    //        {
    //            Destroy(child.gameObject);
    //        }
    //    }
    //    //Måske lav et child gameobject og load visuals på den?


    //    GameObject gameObject = Instantiate(data[Random.Range(0, data.enemyModel.Length)].enemyModel, position, Quaternion.identity);


    //    //Load current enemy visuals
    //    visuals = Instantiate(data[0].enemyModel);
    //    visuals.transform.SetParent(this.transform);
    //    visuals.transform.localPosition = Vector3.zero;
    //    visuals.transform.rotation = Quaternion.identity;

    //    enemyOwnData = visuals.GetComponent<EnemyOwnData>();

    //    enemyOwnData.currentHealth = data[0].health;
    //    enemyOwnData.score = data[0].score;
    //}


}
