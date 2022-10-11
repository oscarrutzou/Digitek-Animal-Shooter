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

    //    //Hvis brugeren ikke har rørt ved volume, så spiller musikken med fuld kraft.
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
    //    //Dramatisk går fra -60DB til -40DB, dog kan mennesket ikke hører under -40DB så det går:)
    //    audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    //}

    //public void ChangeVolume()
    //{
    //    //Sætter volume til valuen på slideren og laver en save.
    //    AudioListener.volume = volumeSlider.value;
    //    Save();
    //}

    //private void Load()
    //{
    //    //Sætter den lig med hvad vi har gemt
    //    volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    //}

    //private void Save()
    //{
    //    //Sætter value fra slider ind i vores key name
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

    //Indsæt det nye gem highscore fra UfoGame


    //Denne bliver kaldt hver gang et våben rammer et IDamageAble
    public void UpdateScore(int score)
    {
        //Tager den nuværende score og plusser den med scoren som lige er kommet.
        currentScore += score;
        //Sætter den nuværende score til tekst.
        scoreText.text = "Point : " + currentScore.ToString();
    }

    //Bliver kaldt når spilleren mister noget af deres liv
    public void UpdateLife(int life)
    {
        //Fjerner liv fra det nuværende liv.
        currentLife -= life;
        //Sætter den nuværende liv til tekst.
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
