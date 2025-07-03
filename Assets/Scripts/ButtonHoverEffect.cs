using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float duration = 0.2f;

    private RectTransform rect;
    private Vector3 originalScale;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalScale = rect.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.DOKill();
        rect.DOScale(originalScale * hoverScale, duration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.DOKill();
        rect.DOScale(originalScale, duration).SetEase(Ease.OutBack);
    }
}
