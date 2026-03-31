using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private Animator[] _smallHeart;
    private int _currentHeart;

    [SerializeField] private TextMeshProUGUI _coinText;
    private int _cointPoint;

    void Start()
    {
        _currentHeart = _smallHeart.Length - 1;
    }

    public void RemoveHeart()
    {
        if (_currentHeart < 0) return;

        _smallHeart[_currentHeart].Play("Hit");
        _currentHeart--;
    }

    public void SetCoinPoint(int coinPoint)
    {
        _cointPoint += coinPoint;
        _coinText.text = _cointPoint.ToString();
    }    

    public void OnPauseButtonClick()
    {
        SceneLoader.Instance.LoadAdditionalScene("Paused");
    }

    public void PlaySFX(AudioClip clip)
    {
        SoundManager.Instance.PlaySFX(clip);
    }
}
