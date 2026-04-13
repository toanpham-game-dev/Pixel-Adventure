using System;
using UnityEngine;

public class PlayerScore : MonoBehaviour, IPlayerScore
{
    private const int _minScore = 0;
    private int _currentScore;

    public int CurrentScore
    {
        get => _currentScore;
        set => _currentScore = value;
    }
    public int MinScore => _minScore;

    public event Action ScoreChanged;

    private void Start()
    {
        _currentScore = _minScore;
    }

    public void IncreaseScore(int amount)
    {
        _currentScore += amount;
        UpdateScore();
    }

    public void UpdateScore()
    {
        ScoreChanged?.Invoke();
    }
}