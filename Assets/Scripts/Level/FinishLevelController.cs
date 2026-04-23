using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevelController : MonoBehaviour
{
    private AnimationController _anim;

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
            PlayerController player  = collision.gameObject.GetComponent<PlayerController>();
            Collider2D[] cols = collision.GetComponents<Collider2D>();
            if (player == null || cols == null) return;
            foreach (Collider2D col in cols)
            {
                col.enabled = false;
            }
            player.RB.bodyType = RigidbodyType2D.Static;
            GameManager.Instance.CompleteLevel(_levelIndex, player.Health.CurrentHealth);
            SceneLoader.Instance.LoadAdditionalScene("Win");
        }
    }
}
