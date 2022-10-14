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
    [Header("Point")]
    //Henter den specielle Tekst pakke.
    public TextMeshProUGUI scoreText;
    [HideInInspector] public int currentScore;
    [HideInInspector] public int bestScore;

    [Header("Kills")]
    //Henter den specielle Tekst pakke.
    public TextMeshProUGUI killText;
    [HideInInspector] public int currentkills;


    [Header("Timer")]
    //bool timerActive = false;
    //float currentTime = 0;
    public TextMeshProUGUI timerLabel;

    [Header("Music")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] Slider volumeSlider;

    [Header("Level Manager")]
    [SerializeField] TMPro.TextMeshProUGUI[] bestScoreLevelText;
    private int bestScoreNumber;
    private int levelNumber;
    [SerializeField] int maxLevels;

    [Header("Pause Menu")]
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;


    void Start()
    {
        //Hvis brugeren ikke har rørt ved volume, så spiller musikken med fuld kraft.
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

    public void ResetHighScores()
    {
        //Reset highscore points fra alle baner
        //levelSelector.DeleteBestTime();
    }
    #endregion

    #region MainMenu
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    #endregion

    #region Update


    //Denne bliver kaldt hver gang et våben rammer et IDamageAble
    //public void UpdateScore(int score)
    //{
    //    //Tager den nuværende score og plusser den med scoren som lige er kommet.
    //    currentScore += score;
    //    //Sætter den nuværende score til tekst.
    //    scoreText.text = "Point : " + currentScore.ToString();
    //}

    #endregion

    public void UpdateBestTime()
    {
        bestScoreNumber = 0;

        for (int i = 1; i < maxLevels + 1; i++)
        {
            levelNumber = i;
            bestScore = PlayerPrefs.GetInt("BestTime" + levelNumber);

            if (bestScore != float.MaxValue)
            {
                Debug.Log("Level number completed " + levelNumber);

                bestScoreLevelText[bestScoreNumber].text = PlayerPrefs.GetFloat("BestTime" + levelNumber, 0).ToString();
                TimeSpan time = TimeSpan.FromSeconds(bestScore);
                bestScoreLevelText[bestScoreNumber].text = time.ToString(@"mm\:ss\:fff");
                bestScoreNumber++;
            }
        }
    }


    #region Pause Menu
    void Pause()
    {
        //Tænder objectet
        pauseMenuUI.SetActive(true);
        //Stopper alt undtagen PauseUI



        Time.timeScale = 0f;
        //Stop animationer, tror de er sat til at virke selv med mindre timescale siden dash virker med timescale 0.75 tror jeg nok.

        gameIsPaused = true;
    }

    public void Resume()
    {
        //Slukker objectet
        pauseMenuUI.SetActive(false);
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
}
