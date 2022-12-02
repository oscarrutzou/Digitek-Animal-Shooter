using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;
using System;


public class Menu : MonoBehaviour
{
    InGameDisplay inGameDisplay;

    [Header("Music")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] Slider volumeSlider;

    [Header("Pause Menu")]
    public bool gameIsPaused = false;
    public GameObject pauseMenuUI;

    [Header("Level")]
    [SerializeField] public int levelNumber;

    [Header("Text")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI killText;
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI bestKillText;

    [Header("Ammo")]
    [HideInInspector] public int ammoUsed;

    //Score
    [HideInInspector] public int currentScore;
    [HideInInspector] public int bestScore;

    //Kills
    [HideInInspector] public int currentKills;
    [HideInInspector] public int bestKills;



    void Start()
    {
        inGameDisplay = GetComponent<InGameDisplay>();

        //For at s�rge for at den ikke sletter de current stats
        if (SceneManager.GetActiveScene().name != "End")
        {
            levelNumber = inGameDisplay.levelNumber;
            //For at refresh kills hver gang den starter.
            PlayerPrefs.SetInt("CurrentKills" + levelNumber, int.MaxValue);
            PlayerPrefs.SetInt("CurrentScore" + levelNumber, int.MaxValue);


        }
        else
        {
            ShowStats();
        }

        //Hvis brugeren ikke har r�rt ved volume, s� spiller lyden med fuld kraft.
        //Ellers loader den deres settings.
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }

    }



    #region Update
    private void Update()
    {
        if (inGameDisplay.currentTime <= -0.005f)
        {
            //Stopper tiden, s� spilleren ikke kan l�be rundt
            Time.timeScale = 0f;

            //Gem alle information
            GameOver();

            //Spil transition
            //Send til anden Dead scene. 
            Time.timeScale = 1f;
            SceneManager.LoadScene(2);

        }
    }


    #endregion

    public void ShowStats()
    {
        killText.text = PlayerPrefs.GetInt("CurrentKills" + levelNumber, 0).ToString();
        bestKillText.text = PlayerPrefs.GetInt("MostKills" + levelNumber, 0).ToString();

        scoreText.text = PlayerPrefs.GetInt("CurrentScore" + levelNumber, 0).ToString();
        bestScoreText.text = PlayerPrefs.GetInt("BestScore" + levelNumber, 0).ToString();

        ammoText.text = PlayerPrefs.GetInt("AmmoUsed" + levelNumber, 0).ToString();
        
    }

    public void GameOver()
    {
        //Gem nuv�rende kills.
        //Tjek om det er de h�jeste kills
        currentKills = inGameDisplay.currentKills;
        currentScore = inGameDisplay.currentScore;
        ammoUsed = inGameDisplay.ammoUsed;

        //Gemmer b�de kills og score for runden, til at blive vist i GameOver Menu.
        PlayerPrefs.SetInt("CurrentKills" + levelNumber, currentKills);
        PlayerPrefs.SetInt("CurrentScore" + levelNumber, currentScore);

        PlayerPrefs.SetInt("AmmoUsed" + levelNumber, ammoUsed);

        //Hvis de nuv�rende kills er st�rre end den gemte, s�ttes den til den MostKills. 
        if (currentKills > PlayerPrefs.GetInt("MostKills" + levelNumber, int.MaxValue))
        {
            //S�tter de nuv�rende kills til MostKills
            PlayerPrefs.SetInt("MostKills" + levelNumber, currentKills);
        }

        //Hvis den nuv�rende score er st�rre end den gemte, s�ttes den til den BestScore. 
        if (currentScore > PlayerPrefs.GetInt("BestScore" + levelNumber, int.MaxValue))
        {
            //S�tter den nuv�rende score til BestScore
            PlayerPrefs.SetInt("BestScore" + levelNumber, currentScore);
        }
    }

    #region OptionMenu
    public void SetVolume(float volume)
    {
        //Dramatisk g�r fra -60DB til -40DB, dog kan mennesket ikke h�rer under -40DB s� det g�r:)
        audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }

    public void ChangeVolume()
    {
        //S�tter volume til valuen p� slideren og laver en save.
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        //S�tter den lig med hvad vi har gemt
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        //S�tter value fra slider ind i vores key name
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }

    public void DeleteHighScore()
    {
        PlayerPrefs.SetInt("MostKills" + levelNumber, 0);
        PlayerPrefs.SetInt("BestScore" + levelNumber, 0);

        ShowStats();
    }
    #endregion

    #region MainMenu
    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    #endregion

    #region Pause Menu
    public void Pause()
    {
        gameIsPaused = true;
        //T�nder objectet
        pauseMenuUI.SetActive(true);
        //Stopper alt undtagen PauseUI

        Time.timeScale = 0f;
        //Stop animationer, tror de er sat til at virke selv med mindre timescale siden dash virker med timescale 0.75 tror jeg nok        
    }

    public void Resume()
    {
        //Slukker objectet
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        else
        {
            return;
        }
        
        //Starter tiden igen
        Time.timeScale = 1f;
        //S�tter den til at spillet ikke er pauset mere.
        gameIsPaused = false;
    }

    public void LoadMenu()
    {
        //Starter tiden igen
        Time.timeScale = 1f;
        //Sender brugeren tilbage til den tidligere scene som er menu

        SceneManager.LoadScene(0);
    }

    #endregion

    #region Shop
    //Her ville alle ting til en in game shop ligge, samt metoder til f.eks. at k�be IGC (InGameCurrency). 
    #endregion

}
