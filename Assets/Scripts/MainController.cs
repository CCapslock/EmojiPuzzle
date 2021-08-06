using System;
using UnityEngine;
using UnityEngine.UI;


public class MainController : MonoBehaviour
{
    [SerializeField] private Text _timer;
    [SerializeField] private GameObject _topPanel;
    [SerializeField] private GameObject _startMenuPanel;
    [SerializeField] private GameObject _levelObjects;
    [SerializeField] private GameObject _endPanel;

    [SerializeField] private float _sqrtDistanceCheck = 1.0f;
    [SerializeField] private int _totalNumberOfParts = 10;
    
    private bool _pause = true;
    private float _time = 0.0f;

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

    public void EndGame()
    {
        _endPanel.SetActive(true);
        _topPanel.SetActive(false);
    }

    public float GetSqrtDistanceCheck()
    {
        return _sqrtDistanceCheck;
    }

    public float GetTotalNumberOfParts()
    {
        return _totalNumberOfParts;
    }
}
