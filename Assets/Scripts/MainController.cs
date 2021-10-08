using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class MainController : MonoBehaviour
{
    public AllLevelsParametrs LevelsParametrs;
    public Slider UiSlider;
    public TMP_Text FirstNumberText;
    public TMP_Text SecondNumberText;
    public SpriteRenderer BackgroundSpriteRenderer;
    public Sprite[] AllBackgrounds;

    [SerializeField] private Text _timer;
    [SerializeField] private GameObject _topPanel;
    [SerializeField] private GameObject _startMenuPanel;
    [SerializeField] private GameObject _levelObjects;
    [SerializeField] private GameObject _endPanel;

    [SerializeField] private float _sqrtDistanceCheck = 1.0f;

    [SerializeField] private Material _successMaterial;

    private string _prefsLevelsCompletedForUi = "CompletedLevel";
    private string _prefsLastPlayedLevel = "LastLevels";
    private string _prefsUsedBackgrounds = "UsedBackground";

    private float _time = 0.0f;
    private float _sliderValue = 0;
    private int _currentLevel = 0;
    private int _partsOnBest = 0;
    private bool _pause = true;

    private void Start()
    {
        if (PlayerPrefs.GetInt(_prefsLevelsCompletedForUi) == 0)
        {
            PlayerPrefs.SetInt(_prefsLevelsCompletedForUi, 1);
        }
        LevelsParametrs.CurrentLevel = (PlayerPrefs.GetInt(_prefsLastPlayedLevel));
        StartGame();

    }

    private void Update()
    {
        if (!_pause)
        {
            _time += Time.deltaTime;
            var ts = TimeSpan.FromSeconds(_time);
            _timer.text = $"{ts.Minutes:00}:{ts.Seconds:00}";
        }
    }

    public void StartGame()
    {
        ResetSlider();
        _topPanel.SetActive(true);
        UiSlider.maxValue = LevelsParametrs.Levels[LevelsParametrs.CurrentLevel].LevelPartsCount;
        _partsOnBest = 0;
        LevelsParametrs.Levels[LevelsParametrs.CurrentLevel].LevelPrefabOnScene.SetActive(false);
        _endPanel.SetActive(false);
        ParticlesManager.Current.PrepareParticles(LevelsParametrs.Levels[LevelsParametrs.CurrentLevel].BigParticle, LevelsParametrs.Levels[LevelsParametrs.CurrentLevel].LittleParticle);
        _time = 0.0f;
        LevelsParametrs.Levels[LevelsParametrs.CurrentLevel].LevelPrefabOnScene.SetActive(true);
        Time.timeScale = 1;
        ChangeBackground();
    }

    public void NextLevel()
    {

        ResetSlider();
        _partsOnBest = 0;
        LevelsParametrs.Levels[LevelsParametrs.CurrentLevel].LevelPrefabOnScene.SetActive(false);
        _endPanel.SetActive(false);
        LevelsParametrs.CurrentLevel++;
        if (LevelsParametrs.CurrentLevel == LevelsParametrs.Levels.Count - 1)
        {
            PlayerPrefs.SetInt(_prefsLastPlayedLevel, 0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }
        else
        {
            PlayerPrefs.SetInt(_prefsLastPlayedLevel, LevelsParametrs.CurrentLevel);
        }
        ParticlesManager.Current.PrepareParticles(LevelsParametrs.Levels[LevelsParametrs.CurrentLevel].BigParticle, LevelsParametrs.Levels[LevelsParametrs.CurrentLevel].LittleParticle);
        UiSlider.maxValue = LevelsParametrs.Levels[LevelsParametrs.CurrentLevel].LevelPartsCount;
        _time = 0.0f;
        LevelsParametrs.Levels[LevelsParametrs.CurrentLevel].LevelPrefabOnScene.SetActive(true);
        Time.timeScale = 1;
        ChangeBackground();
    }

    public float GetSqrtDistanceCheck()
    {
        return _sqrtDistanceCheck;
    }

    public void CheckEndLevel()
    {
        if (_partsOnBest == LevelsParametrs.Levels[LevelsParametrs.CurrentLevel].LevelPartsCount)
        {
            PlayerPrefs.SetInt(_prefsLevelsCompletedForUi, PlayerPrefs.GetInt(_prefsLevelsCompletedForUi) + 1);
            Invoke(nameof(PlayWinParticles), 0.5f);
            Invoke(nameof(NextLevel), 4f);
            _endPanel.SetActive(true);
        }
    }
    private void PlayWinParticles()
    {
        ParticlesManager.Current.MakeConfettiParticles();
        ParticlesManager.Current.MakeBigParticle();
        ParticlesManager.Current.MakeLittleParticle(new Vector3(2f, 2f, 0f), true);
        ParticlesManager.Current.MakeLittleParticle(new Vector3(2f, -2f, 0f), true);
        ParticlesManager.Current.MakeLittleParticle(new Vector3(-2f, 2f, 0f), true);
        ParticlesManager.Current.MakeLittleParticle(new Vector3(-2f, -2f, 0f), true);
        ParticlesManager.Current.MakeLittleParticle(new Vector3(-2f, 0f, 0f), true);
        ParticlesManager.Current.MakeLittleParticle(new Vector3(2f, 0f, 0f), true);
    }
    public void AddPartOnBest()
    {
        _partsOnBest++;
        IncreaseSliderValue();
        CheckEndLevel();
    }

    public Material GetSuccessMaterial()
    {
        return _successMaterial;
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }
    private void IncreaseSliderValue()
    {
        for (int i = 0; i < 100; i++)
        {
            Invoke("IncreaseSliderValueALittle", i * 0.005f);
        }
    }
    private void IncreaseSliderValueALittle()
    {
        _sliderValue += 0.01f;
        UiSlider.value = _sliderValue;
    }
    private void ResetSlider()
    {
        _sliderValue = 0;
        UiSlider.value = _sliderValue;
        FirstNumberText.text = PlayerPrefs.GetInt(_prefsLevelsCompletedForUi).ToString();
        SecondNumberText.text = (PlayerPrefs.GetInt(_prefsLevelsCompletedForUi) + 1).ToString();
    }
    private void ChangeBackground()
    {
        if (PlayerPrefs.GetInt(_prefsUsedBackgrounds) < AllBackgrounds.Length)
        {
            BackgroundSpriteRenderer.sprite = AllBackgrounds[PlayerPrefs.GetInt(_prefsUsedBackgrounds)];
            PlayerPrefs.SetInt(_prefsUsedBackgrounds, PlayerPrefs.GetInt(_prefsUsedBackgrounds) + 1);
        }
        else
        {
            PlayerPrefs.SetInt(_prefsUsedBackgrounds, 0);
            BackgroundSpriteRenderer.sprite = AllBackgrounds[PlayerPrefs.GetInt(_prefsUsedBackgrounds)];
        }
    }
}

[Serializable]
public class AllLevelsParametrs
{
    public int CurrentLevel;
    public List<SingleLevelParam> Levels;
}

[Serializable]
public class SingleLevelParam
{
    public GameObject LevelPrefabOnScene;
    public int LevelPartsCount;
    public ParticleSystem BigParticle;
    public ParticleSystem LittleParticle;
}
