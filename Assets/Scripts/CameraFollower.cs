using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Follow Settings")]
    [SerializeField] private float height = 10f;

    [SerializeField] private float distance = 0f;
    [SerializeField] private float followSpeed = 5f;

    [Header("View Angle")]
    [SerializeField] private Vector3 rotation = new Vector3(90f, 0f, 0f); // Y will be adjusted with Q/E

    [Header("Offset")]
    [SerializeField] private Vector3 positionOffset = Vector3.zero;

    private void LateUpdate()
    {
        // Auto-find player if not yet assigned
        if (target == null)
        {
            PlayerMovement player = Object.FindFirstObjectByType<PlayerMovement>();
            if (player != null)
                target = player.transform;
            else
                return;
        }

        // Handle camera rotation input
        if (Input.GetKeyDown(KeyCode.Q))
            rotation.y -= 90f;
        else if (Input.GetKeyDown(KeyCode.E))
            rotation.y += 90f;

        Vector3 moveInput = Vector3.zero;

        if (Input.GetKey(KeyCode.I)) moveInput += Vector3.left;
        if (Input.GetKey(KeyCode.K)) moveInput += Vector3.right;
        if (Input.GetKey(KeyCode.J)) moveInput += Vector3.back;
        if (Input.GetKey(KeyCode.L)) moveInput += Vector3.forward;

        if (moveInput != Vector3.zero)
        {
            // World-space movement:
            positionOffset += moveInput * Time.deltaTime * 5f;

            // move relative to current camera Y rotation
            positionOffset += Quaternion.Euler(0, rotation.y, 0) * moveInput * Time.deltaTime * 5f;
        }

        Vector3 desiredPosition = target.position + positionOffset;
        desiredPosition += Quaternion.Euler(0, rotation.y, 0) * (Vector3.back * distance);
        desiredPosition += Vector3.up * height;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        float smoothY = Mathf.LerpAngle(transform.eulerAngles.y, rotation.y, followSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(rotation.x, smoothY, rotation.z);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}