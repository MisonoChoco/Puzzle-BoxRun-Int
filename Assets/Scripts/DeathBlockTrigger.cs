using UnityEngine;

public class DeathBlockTrigger : MonoBehaviour
{
    public Collider collidedObj;

    private void OnTriggerEnter(Collider other)
    {
        collidedObj = other;

        if (transform.parent.TryGetComponent(out DeathBlock deathBlock))
        {
            deathBlock.CollisionDetected(this);
        }
    }
}