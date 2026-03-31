using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleAnimation : MonoBehaviour
{
    RectTransform rect;
    float baseY;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        baseY = rect.anchoredPosition.y;

        PixelIdle();
    }

    void PixelIdle()
    {
        Sequence seq = DOTween.Sequence();

        seq = DOTween.Sequence();

        seq.Append(rect.DOAnchorPosY(baseY + 5, 0.5f, true))
           .SetEase(Ease.Linear);

        seq.Append(rect.DOAnchorPosY(baseY, 0.5f, true))
           .SetEase(Ease.Linear);

        seq.SetLoops(-1)
           .SetLink(gameObject);
    }
}