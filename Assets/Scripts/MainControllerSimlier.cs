using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainControllerSimlier : MonoBehaviour
{
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
            Invoke(nameof(Pause), 0.3f);
            _endPanel.SetActive(true);
        }
    }

    public void AddPartOnBest()
    {
        _partsOnBest++;
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
}
