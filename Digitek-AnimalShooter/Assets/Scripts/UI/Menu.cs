using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;


public class Menu : MonoBehaviour
{
    [Header("Point")]
    //Henter den specielle Tekst pakke.
    public TextMeshProUGUI scoreText;
    [HideInInspector] public int currentScore;

    [Header("Timer")]
    //bool timerActive = false;
    //float currentTime = 0;
    public TextMeshProUGUI timerLabel;


    [Header("Life")]
    public TextMeshProUGUI lifeText;
    [SerializeField] int currentLife = 100;


    [Header("Level")]
    [SerializeField] GameObject EXPBar;
    [SerializeField] TextMeshProUGUI levelText;

    private Slider slider;

    [Header("Music")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] Slider volumeSlider;

    private void Awake()
    {
        slider = EXPBar.GetComponent<Slider>();
    }

    //void Start()
    //{

    //    //Hvis brugeren ikke har r�rt ved volume, s� spiller musikken med fuld kraft.
    //    //Ellers loader den deres settings.
    //    if (!PlayerPrefs.HasKey("musicVolume"))
    //    {
    //        PlayerPrefs.SetFloat("musicVolume", 1);
    //        Load();
    //    }
    //    else
    //    {
    //        Load();
    //    }
    //}

    //#region OptionMenu
    //public void SetVolume(float volume)
    //{
    //    //Dramatisk g�r fra -60DB til -40DB, dog kan mennesket ikke h�rer under -40DB s� det g�r:)
    //    audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    //}

    //public void ChangeVolume()
    //{
    //    //S�tter volume til valuen p� slideren og laver en save.
    //    AudioListener.volume = volumeSlider.value;
    //    Save();
    //}

    //private void Load()
    //{
    //    //S�tter den lig med hvad vi har gemt
    //    volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    //}

    //private void Save()
    //{
    //    //S�tter value fra slider ind i vores key name
    //    PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    //}
    //#endregion

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

    //Inds�t det nye gem highscore fra UfoGame


    //Denne bliver kaldt hver gang et v�ben rammer et IDamageAble
    public void UpdateScore(int score)
    {
        //Tager den nuv�rende score og plusser den med scoren som lige er kommet.
        currentScore += score;
        //S�tter den nuv�rende score til tekst.
        scoreText.text = "Point : " + currentScore.ToString();
    }

    //Bliver kaldt n�r spilleren mister noget af deres liv
    public void UpdateLife(int life)
    {
        //Fjerner liv fra det nuv�rende liv.
        currentLife -= life;
        //S�tter den nuv�rende liv til tekst.
        lifeText.text = " Life : " + currentLife.ToString();
    }
    #endregion

    public void UpDateExperienceSlider(int current, int target)
    {
        slider.maxValue = target;
        slider.value = current;
    }

    public void SetLevelText(int level)
    {
        levelText.text = "Level : " + level.ToString();
    }

}
