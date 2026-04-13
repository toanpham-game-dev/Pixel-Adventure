using UnityEngine;

public class FruitController : MonoBehaviour
{
    [SerializeField] private int _scorePoint;

    private AnimationController _anim;

    private void Awake()
    {
        _anim = GetComponent<AnimationController>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerScore score = collision.gameObject.GetComponent<PlayerScore>();
            score.IncreaseScore(_scorePoint);
            _anim.PlayAnimation("Collected");
        }
    }

    public void DestroyFruit()
    {
        Destroy(gameObject);
    }
}
