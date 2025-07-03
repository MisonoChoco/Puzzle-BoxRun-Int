using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DeathBlock : MonoBehaviour
{
    [SerializeField] private GameObject triggerObject;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private float horDist;
    [SerializeField] private float vertDist;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float direction = 1;
    [SerializeField] private int firstPointIndex = 0;

    private Vector3 toPoint;
    private Vector3[] points = new Vector3[4];
    private bool isMoving = false;

    private void Start()
    {
        horDist *= 4;
        vertDist *= 4;
        GeneratePoints();
        toPoint = points[firstPointIndex];
    }

    private void Update()
    {
        if (!isMoving)
        {
            isMoving = true;
            float duration = Vector3.Distance(transform.position, toPoint) / speed;

            transform.DOMove(toPoint, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    transform.position = toPoint;
                    UpdateNextPoint();
                    isMoving = false;
                });
        }
    }

    private void GeneratePoints()
    {
        points[0] = startPosition;
        points[1] = points[0] + new Vector3(0, 0, horDist);
        points[2] = points[1] + new Vector3(vertDist, 0, 0);
        points[3] = points[2] + new Vector3(0, 0, -horDist);
    }

    private void UpdateNextPoint()
    {
        int index = System.Array.IndexOf(points, toPoint);
        if (direction == 1)
            index = (index + 1) % 4;
        else
            index = (index - 1 + 4) % 4;

        toPoint = points[index];
    }

    public void CollisionDetected(DeathBlockTrigger trigger)
    {
        StartCoroutine(RespawnPlayer(trigger.collidedObj.gameObject));
    }

    private IEnumerator RespawnPlayer(GameObject player)
    {
        if (player.TryGetComponent(out PlayerMovement movement))
            movement.enabled = false;

        if (player.TryGetComponent(out Rigidbody rb))
            rb.detectCollisions = false;

        yield return new WaitForSeconds(1.2f);
        // PlayerManager or GameManager may reinstantiate here if needed
    }
}