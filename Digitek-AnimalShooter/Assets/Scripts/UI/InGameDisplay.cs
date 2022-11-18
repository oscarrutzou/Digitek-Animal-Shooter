using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

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


    //Til timeren
    private bool timerActive = false;
    public float currentTime = 0f;
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




    private void Awake()
    {
        InstantiateSingleton();
    }

    private void Start()
    {
        currentTime = startingTime;
        StartTimer();

        ////Lav besttime til at blive 
        ////bestTime = PlayerPrefs.GetFloat("BestTime" + levelNumber);


        //if (bestTime != float.MaxValue)
        //{
        //    ShowUI();
        //}

    }

    private void Update()
    {

        //Hvis timeren er aktiv, skal den t�lle med time.deltaTime for at kunne t�lle rigtigt.
        if (timerActive == true)
        {
            currentTime -= 1* Time.deltaTime;
        }
        //For at kunne se tiden i minutter, sekunder og milisekunder
        time = TimeSpan.FromSeconds(currentTime);
        timerLabel.text = time.ToString(@"mm\:ss");

        scoreText.text = currentScore.ToString();

        killText.text = currentKills.ToString();
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

    //    //Hvis den nuv�rende tid er st�rre end den gemte, s�ttes den til den bedste tid. 
    //    if (currentTime < PlayerPrefs.GetInt("BestTime" + levelNumber, int.MaxValue))
    //    {
    //        //S�tter den nuv�rende tid til den bedste tid.
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
