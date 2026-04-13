using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevelController : MonoBehaviour
{
    private AnimationController _anim;
    private PlayerController _player;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _desappearDelayTime;

    [Header("Level Config")]
    [SerializeField] private int _levelIndex;

    private void Awake()
    {
        _anim = GetComponent<AnimationController>();

        string currentScene = SceneManager.GetActiveScene().name;
        _levelIndex = int.Parse(currentScene.Replace("Lv", ""));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _anim.PlayAnimation("Idle");
            _player = collision.gameObject.GetComponent<PlayerController>();
        }
    }

    public void FinishLevel()
    {
        if (_player != null)
        {
            _player.Movement.ExternalJump(_jumpForce);
            StartCoroutine(FinishDesappear());
            GameManager.Instance.CompleteLevel(_levelIndex, _player.Health.CurrentHealth);
            SceneLoader.Instance.LoadAdditionalScene("Win");
        }
    }

    IEnumerator FinishDesappear()
    {
        yield return new WaitForSeconds(_desappearDelayTime);
        _player.Desappear();
    }
}
