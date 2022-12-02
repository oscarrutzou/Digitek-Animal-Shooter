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

        //For at sørge for at den ikke sletter de current stats
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

        //Hvis brugeren ikke har rørt ved volume, så spiller lyden med fuld kraft.
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
            //Stopper tiden, så spilleren ikke kan løbe rundt
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
        //Gem nuværende kills.
        //Tjek om det er de højeste kills
        currentKills = inGameDisplay.currentKills;
        currentScore = inGameDisplay.currentScore;
        ammoUsed = inGameDisplay.ammoUsed;

        //Gemmer både kills og score for runden, til at blive vist i GameOver Menu.
        PlayerPrefs.SetInt("CurrentKills" + levelNumber, currentKills);
        PlayerPrefs.SetInt("CurrentScore" + levelNumber, currentScore);

        PlayerPrefs.SetInt("AmmoUsed" + levelNumber, ammoUsed);

        //Hvis de nuværende kills er større end den gemte, sættes den til den MostKills. 
        if (currentKills > PlayerPrefs.GetInt("MostKills" + levelNumber, int.MaxValue))
        {
            //Sætter de nuværende kills til MostKills
            PlayerPrefs.SetInt("MostKills" + levelNumber, currentKills);
        }

        //Hvis den nuværende score er større end den gemte, sættes den til den BestScore. 
        if (currentScore > PlayerPrefs.GetInt("BestScore" + levelNumber, int.MaxValue))
        {
            //Sætter den nuværende score til BestScore
            PlayerPrefs.SetInt("BestScore" + levelNumber, currentScore);
        }
    }

    #region OptionMenu
    public void SetVolume(float volume)
    {
        //Dramatisk går fra -60DB til -40DB, dog kan mennesket ikke hører under -40DB så det går:)
        audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }

    public void ChangeVolume()
    {
        //Sætter volume til valuen på slideren og laver en save.
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        //Sætter den lig med hvad vi har gemt
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        //Sætter value fra slider ind i vores key name
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
        //Tænder objectet
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
        //Sætter den til at spillet ikke er pauset mere.
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
    //Her ville alle ting til en in game shop ligge, samt metoder til f.eks. at købe IGC (InGameCurrency). 
    #endregion

}
