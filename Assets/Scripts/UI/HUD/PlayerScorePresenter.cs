using TMPro;
using UnityEngine;

public class PlayerScorePresenter : MonoBehaviour
{
    [SerializeField] private PlayerScore _playerScore;
    [SerializeField] private TMP_Text _scoreText;

    private void Start()
    {
        _playerScore = FindFirstObjectByType<PlayerScore>();
        if (_playerScore != null)
        {
            _playerScore.ScoreChanged += OnScoreChanged;
        }
        UpdateView();
    }

    private void OnDestroy()
    {
        if (_playerScore != null)
        {
            _playerScore.ScoreChanged -= OnScoreChanged;
        }
    }

    public void Eat(int amount)
    {
        _playerScore?.IncreaseScore(amount);
    }


    public void UpdateView()
    {
        if (_playerScore == null) return;
        _scoreText.text = _playerScore?.CurrentScore.ToString();
    }

    public void OnScoreChanged()
    {
        UpdateView();
    }
}
