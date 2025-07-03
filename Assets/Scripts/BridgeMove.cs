using UnityEngine;
using DG.Tweening;

public class BridgeMove : MonoBehaviour
{
    [SerializeField] public int direction = 1;
    [SerializeField] private float heightMin;
    [SerializeField] private float heightMax;
    [SerializeField] private float speed = 50f;

    private BoxCollider bridgeCollider;
    private bool isMoving = false;

    private void Awake()
    {
        bridgeCollider = GetComponentInChildren<BoxCollider>();
    }

    private void Update()
    {
        if (bridgeCollider != null)
            bridgeCollider.enabled = direction != -1;

        if (!isMoving && ((direction == -1 && transform.position.y > heightMin) || (direction == 1 && transform.position.y < heightMax)))
        {
            float targetY = (direction == -1) ? heightMin : heightMax;
            float distance = Mathf.Abs(transform.position.y - targetY);
            float duration = distance / speed;

            isMoving = true;

            transform.DOMoveY(targetY, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => isMoving = false);
        }
    }
}