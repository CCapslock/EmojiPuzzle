using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MainControllerSimlier : MonoBehaviour
{
    public Slider UiSlider;
    public TMP_Text FirstNumberText;
    public TMP_Text SecondNumberText;

    [SerializeField] private Text _timer;
    [SerializeField] private GameObject _topPanel;
    [SerializeField] private GameObject _startMenuPanel;
    [SerializeField] private GameObject _levelObjects;
    [SerializeField] private GameObject _endPanel;

    [SerializeField] private float _sqrtDistanceCheck = 1.0f;
    [SerializeField] private int _totalNumberOfParts = 10;

    [SerializeField] private List<PuzzlePartSimlier> _parts;

    [SerializeField] private Material _successMaterial;

    private bool _pause = true;
    private float _time = 0.0f;

    [SerializeField] private List<GameObject> _levels;
    [SerializeField] private List<int> _levelPartsCount;
    private float _sliderValue = 0;
    private int _currentLevel = 0;
    private int _partsOnBest = 0;

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
        _pause = false;
        _topPanel.SetActive(true);
        _startMenuPanel.SetActive(false);
        _levelObjects.SetActive(true);

        UiSlider.maxValue = _levelPartsCount[_currentLevel];
        UiSlider.value = 0;
        int num = UnityEngine.Random.Range(2, 16);
        FirstNumberText.text = num.ToString();
        SecondNumberText.text = (num+1).ToString();
    }

    public void NextLevel()
    {
        _partsOnBest = 0;
        _levels[_currentLevel].SetActive(false);
        _endPanel.SetActive(false);
        _currentLevel++;
        if (_currentLevel == _levelPartsCount.Count)
        {
            return;
        }
        _time = 0.0f;
        _levels[_currentLevel].SetActive(true);
        Time.timeScale = 1;
    }

    public float GetSqrtDistanceCheck()
    {
        return _sqrtDistanceCheck;
    }

    public void CheckEndLevel()
    {
        if (_partsOnBest == _levelPartsCount[_currentLevel])
        {
            Invoke(nameof(PlayWinParticles), 0.5f);
            Invoke(nameof(Pause), 4f);
            _endPanel.SetActive(true);
        }
    }
    private void PlayWinParticles()
    {
        ParticlesManager.Current.MakeConfettiParticles();
        ParticlesManager.Current.MakeBigSimpleSmile();
        ParticlesManager.Current.MakeSimpleSmile(new Vector3(2f, 2f, 0f), true);
        ParticlesManager.Current.MakeSimpleSmile(new Vector3(2f, -2f, 0f), true);
        ParticlesManager.Current.MakeSimpleSmile(new Vector3(-2f, 2f, 0f), true);
        ParticlesManager.Current.MakeSimpleSmile(new Vector3(-2f, -2f, 0f), true);
        ParticlesManager.Current.MakeSimpleSmile(new Vector3(-2f, 0f, 0f), true);
        ParticlesManager.Current.MakeSimpleSmile(new Vector3(2f, 0f, 0f), true);
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
}
