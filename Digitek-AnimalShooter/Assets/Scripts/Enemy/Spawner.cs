using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    //Laver et array for at kunne nemt tage ind de forskellige PreFabs hvis man har brug for det et sted
    [SerializeField] public GameObject[] enemyPrefab;


    //Variabler som skal vises i inspector men ikke skal kaldes af andre scipts
    [SerializeField] float timeBetweenSpawn = 5f;


    [SerializeField] GameObject pos1;
    [SerializeField] GameObject pos2;

    private float minPosX;
    private float minPosY;
    private float maxPosX;
    private float maxPosY;


    [SerializeField] float activateDistance = 10f;
    private GameObject targetGameObject;
    private PlayerController playerController;
    private Transform target;



    private void Awake()
    {
        targetGameObject = GameObject.FindWithTag("Player");
        playerController = targetGameObject.GetComponent<PlayerController>();

        target = targetGameObject.transform;

        minPosX = pos1.GetComponentInChildren<Transform>().position.x;
        maxPosX = pos2.GetComponentInChildren<Transform>().position.x;
        minPosY = pos1.GetComponentInChildren<Transform>().position.y;
        maxPosY = pos2.GetComponentInChildren<Transform>().position.y;


        Debug.Log(minPosX + "  " + maxPosX + "  ");
        Debug.Log(minPosY + "  " + maxPosY + "  ");


    }

    //Ikke update.
    private void Start()
    {
        if (TargetInDistance())
        {
            StartCoroutine(Spawn());
        }
        else if (!TargetInDistance())
        {
            StopCoroutine(Spawn());
        }

    }

    private void Update()
    {

    }


    IEnumerator Spawn()
    {
        //Mens den er aktiv skal den spawne.
        while (true)
        {
            //Den venter et antal sekunder
            yield return new WaitForSeconds(timeBetweenSpawn);

            //Lavet som en random range.

            float positionX = Random.Range(minPosX, maxPosX);
            Debug.Log(positionX);

            float positionY = Random.Range(minPosY, maxPosY);
            Debug.Log(positionY);


            //Sætter positionen af det skrald til det random stykke.
            var position = new Vector3(positionX, positionY, transform.position.z);
            Debug.Log(position);


            //Spawner et random prefab fra vores array til vores position, uden at den bliver roteret.
            GameObject gameObject = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], position, Quaternion.identity);

        }
    }


    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.position) < activateDistance;
    }
}
