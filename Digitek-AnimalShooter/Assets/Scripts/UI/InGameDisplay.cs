using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using TheraBytes.BetterUi;

public class InGameDisplay : MonoBehaviour
{
    #region Singleton
    public static InGameDisplay Instance { get; private set; }

    private void InstantiateSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Flere singleton");
            Destroy(this);
        }
    }
    #endregion

    private GameObject playerObject;
    private GameObject canvasObject;
    public GameObject[] gunArray; //Gå igennem og indsæt i forhold til hvor lang _data er i  PlayerAimWeapon.
    //Mulighed for at kunne sætte alle våbene på hver plads også bare tænde den som man har købt?
    //Shop vil kun kunne købe en af våbene af gangen.
    public Image[] gunArrayUI;


    public PlayerAimWeapon playerAimWeapon;

    public Color nonActiveColor;
    public Color activeColor;


    [Header("Timer")]
    private bool timerActive = false;
    [HideInInspector] public float currentTime = 0f;
    [SerializeField] private float startingTime = 10f;
    private TimeSpan time;
    [SerializeField] private TMPro.TextMeshProUGUI timerLabel;


    [SerializeField] public int levelNumber;

    [Header("Point")]
    //Henter den specielle Tekst pakke.
    public TextMeshProUGUI scoreText;
    [HideInInspector] public int currentScore;

    [Header("Kills")]
    //Henter den specielle Tekst pakke.
    public TextMeshProUGUI killText;
    [HideInInspector] public int currentKills;

    [Header("Ammo")]
    public TextMeshProUGUI ammoText;
    public int ammoUsed;


    private int dataLength;
    

    private void Awake()
    {
        InstantiateSingleton();
    }

    private void Start()
    {
        currentTime = startingTime;

        playerObject = GameObject.Find("Player");
        playerAimWeapon = playerObject.GetComponent<PlayerAimWeapon>();

        canvasObject = GameObject.Find("Canvas");

        dataLength = playerAimWeapon._data.Length;

        //nonActiveColor = new Color(0, 0, 0, 0.2f);
        //activeColor = new Color(0, 0, 0, 0.4f);


        StartTimer();

        StartUpdateWeaponUI();
        ////Lav besttime til at blive 
        ////bestTime = PlayerPrefs.GetFloat("BestTime" + levelNumber);


    }

    
    public void StartUpdateWeaponUI()
    {
        gunArray = new GameObject[dataLength];
        gunArrayUI = new Image[dataLength];

        for (int i = 0; i < dataLength; i++)
        {
            //Debug.Log(i);
            gunArray[i] = canvasObject.transform.GetChild(0).GetChild(0).GetChild(i).gameObject;
            gunArrayUI[i] = gunArray[i].GetComponent<Image>();

            if (i == playerAimWeapon._dataCurrentNumber)
            {
                gunArrayUI[i].color = activeColor;
                //Debug.Log("ja " + i +  " data number" + playerAimWeapon._dataCurrentNumber +  "GunARYYAI i " + gunArrayUI[i].name);
            }
            else
            {
                gunArrayUI[i].color = nonActiveColor;
                //Debug.Log("Nej " + i + " data number" + playerAimWeapon._dataCurrentNumber + "GunARYYAI i " + gunArrayUI[i].name);
            } 
        }
    }

    public void UpdateWeaponUI(int dataCurrentNumber)
    {
        for (int i = 0; i < dataLength; i++)
        {
            if (i == dataCurrentNumber)
            {
                gunArrayUI[i].color = activeColor;
            }
            else if (gunArrayUI[i].color == activeColor)
            {
                gunArrayUI[i].color = nonActiveColor;
            }
        }
    }

    private void Update()
    {

        //Hvis timeren er aktiv, skal den tælle med time.deltaTime for at kunne tælle rigtigt.
        if (timerActive == true)
        {
            currentTime -= Time.deltaTime;
        }
        //For at kunne se tiden i minutter, sekunder og milisekunder
        time = TimeSpan.FromSeconds(currentTime);
        timerLabel.text = time.ToString(@"mm\:ss");

        scoreText.text = currentScore.ToString();

        killText.text = currentKills.ToString();

        ammoText.text = playerAimWeapon._tempAmmo.ToString() + " / " + playerAimWeapon._ammo.ToString();
    }

    //Starter timeren
    public void StartTimer()
    {
        timerActive = true;
    }

    //Slukker timeren
    public void TurnOffTimer()
    {
        timerActive = false;
    }

    public void ResetTimer()
    {
        TurnOffTimer();
        currentTime = 0f;
        StartTimer();
    }



    //public void LevelComplete()
    //{
    //    TurnOffTimer();

    //    //Hvis den nuværende tid er større end den gemte, sættes den til den bedste tid. 
    //    if (currentTime < PlayerPrefs.GetInt("BestTime" + levelNumber, int.MaxValue))
    //    {
    //        //Sætter den nuværende tid til den bedste tid.
    //        PlayerPrefs.SetInt("BestTime" + levelNumber, currentTime);
    //    }
    //    ShowUI();
    //}

    //private void ShowUI()
    //{
    //    bestTime = PlayerPrefs.GetFloat("BestTime" + levelNumber);
    //    bestTimeText.text = "Best Time : " + PlayerPrefs.GetFloat("BestTime" + levelNumber, 0).ToString();
    //    TimeSpan time = TimeSpan.FromSeconds(bestTime);
    //    //Viser tiden i minutter, sekunder og milisekunder
    //    bestTimeText.text = time.ToString(@"mm\:ss\:fff");
    //}

}
