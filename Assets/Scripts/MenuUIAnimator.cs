using UnityEngine;
using DG.Tweening;

public class MenuUIAnimator : MonoBehaviour
{
    [Header("Logo")]
    [SerializeField] private RectTransform logo;

    [SerializeField] private float fallDuration = 1f;
    [SerializeField] private float fallOffsetY = 500f;

    [Header("Idle Shake")]
    [SerializeField] private float scaleUpFactor = 1.1f;

    [SerializeField] private float scaleUpDuration = 0.3f;
    [SerializeField] private float punchAngle = 15f;
    [SerializeField] private float punchDuration = 0.5f;
    [SerializeField] private float idleInterval = 3f;

    private Vector3 originalLogoScale;
    private Vector2 originalLogoPos;

    private void Start()
    {
        originalLogoScale = logo.localScale;
        originalLogoPos = logo.anchoredPosition;

        logo.anchoredPosition = originalLogoPos + Vector2.up * fallOffsetY;

        DOTween.Sequence()
            .Append(logo
                .DOAnchorPos(originalLogoPos, fallDuration)
                .SetEase(Ease.OutBounce)
            )
            .OnComplete(() =>
            {
                logo
                    .DOScale(originalLogoScale * scaleUpFactor, scaleUpDuration)
                    .SetEase(Ease.OutSine)
                    .OnComplete(() =>
                        logo
                            .DOScale(originalLogoScale, scaleUpDuration * 0.5f)
                            .SetEase(Ease.InSine)
                    );

                logo
                    .DOPunchRotation(new Vector3(0, 0, punchAngle), punchDuration, vibrato: 10, elasticity: 1f)
                    .SetEase(Ease.InOutSine);

                DOTween.Sequence()
                    .AppendInterval(idleInterval)
                    .AppendCallback(() =>
                        logo
                          .DOPunchRotation(new Vector3(0, 0, punchAngle), punchDuration, vibrato: 10, elasticity: 1f)
                          .SetEase(Ease.InOutSine)
                    )
                    .SetLoops(-1);
            });
    }
}