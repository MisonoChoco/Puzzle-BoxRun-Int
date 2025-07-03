using UnityEngine;

public class FrameBlock : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (other.TryGetComponent(out Rigidbody rb))
            rb.linearVelocity = new Vector3(0, -50, 0);

        if (other.TryGetComponent(out PlayerMovement pm))
            pm.enabled = false;

        Debug.Log("PlayerMovement disabled by FrameBlock.");
    }
}