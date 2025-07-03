using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float duration = 0.3f;
    private Vector3 scale;

    public bool isRotating = false;
    private float directionX = 0;
    private float directionZ = 0;

    private float startAngleRad = 0;
    private Vector3 startPos;
    private float rotationTime = 0;
    private float radius = 1;
    private Quaternion preRotation;
    private Quaternion postRotation;

    [Header("Audio")]
    [SerializeField] private AudioClip moveClip;

    private AudioSource audioSource;

    public bool isGrounded = false;

    private void Start()
    {
        scale = transform.lossyScale;

        this.transform.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, -50, 0);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = moveClip;
    }

    private void Update()
    {
        if (!isRotating & isGrounded)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = 0;

            if (x == 0)
                y = Input.GetAxisRaw("Vertical");

            if ((x != 0 || y != 0) && !isRotating)
            {
                if (moveClip != null)
                    audioSource.PlayOneShot(moveClip);

                directionX = y;
                directionZ = x;
                startPos = transform.position;
                preRotation = transform.rotation;
                transform.Rotate(directionZ * 90, 0, directionX * 90, Space.World);
                postRotation = transform.rotation;
                transform.rotation = preRotation;
                SetRadius();
                rotationTime = 0;
                isRotating = true;
                GameManager.moves += 1;
            }
        }
        else
        {
            this.transform.position += new Vector3(0, 0.1f, 0);
            this.transform.position -= new Vector3(0, 0.1f, 0);
        }
    }

    private void FixedUpdate()
    {
        if (isRotating)
        {
            rotationTime += Time.fixedDeltaTime;
            float ratio = Mathf.Lerp(0, 1, rotationTime / duration);

            float rotAng = Mathf.Lerp(0, Mathf.PI / 2f, ratio);
            float distanceX = -directionX * radius * (Mathf.Cos(startAngleRad) - Mathf.Cos(startAngleRad + rotAng));
            float distanceY = radius * (Mathf.Sin(startAngleRad + rotAng) - Mathf.Sin(startAngleRad));
            float distanceZ = directionZ * radius * (Mathf.Cos(startAngleRad) - Mathf.Cos(startAngleRad + rotAng));
            transform.position = new Vector3(startPos.x + distanceX, startPos.y + distanceY, startPos.z + distanceZ);

            transform.rotation = Quaternion.Lerp(preRotation, postRotation, ratio);

            if (ratio == 1)
            {
                isRotating = false;
                directionX = 0;
                directionZ = 0;
                rotationTime = 0;
            }
        }
    }

    private void SetRadius()
    {
        Vector3 dirVec = new Vector3(0, 0, 0);
        Vector3 nomVec = Vector3.up;

        if (directionX != 0)
            dirVec = Vector3.right;
        else if (directionZ != 0)
            dirVec = Vector3.forward;

        if (Mathf.Abs(Vector3.Dot(transform.right, dirVec)) > 0.99)
        {
            if (Mathf.Abs(Vector3.Dot(transform.up, nomVec)) > 0.99)
            {
                radius = Mathf.Sqrt(Mathf.Pow(scale.x / 2f, 2f) + Mathf.Pow(scale.y / 2f, 2f));
                startAngleRad = Mathf.Atan2(scale.y, scale.x);
            }
            else if (Mathf.Abs(Vector3.Dot(transform.forward, nomVec)) > 0.99)
            {
                radius = Mathf.Sqrt(Mathf.Pow(scale.x / 2f, 2f) + Mathf.Pow(scale.z / 2f, 2f));
                startAngleRad = Mathf.Atan2(scale.z, scale.x);
            }
        }
        else if (Mathf.Abs(Vector3.Dot(transform.up, dirVec)) > 0.99)
        {
            if (Mathf.Abs(Vector3.Dot(transform.right, nomVec)) > 0.99)
            {
                radius = Mathf.Sqrt(Mathf.Pow(scale.y / 2f, 2f) + Mathf.Pow(scale.x / 2f, 2f));
                startAngleRad = Mathf.Atan2(scale.x, scale.y);
            }
            else if (Mathf.Abs(Vector3.Dot(transform.forward, nomVec)) > 0.99)
            {
                radius = Mathf.Sqrt(Mathf.Pow(scale.y / 2f, 2f) + Mathf.Pow(scale.z / 2f, 2f));
                startAngleRad = Mathf.Atan2(scale.z, scale.y);
            }
        }
        else if (Mathf.Abs(Vector3.Dot(transform.forward, dirVec)) > 0.99)
        {
            if (Mathf.Abs(Vector3.Dot(transform.right, nomVec)) > 0.99)
            {
                radius = Mathf.Sqrt(Mathf.Pow(scale.z / 2f, 2f) + Mathf.Pow(scale.x / 2f, 2f));
                startAngleRad = Mathf.Atan2(scale.x, scale.z);
            }
            else if (Mathf.Abs(Vector3.Dot(transform.up, nomVec)) > 0.99)
            {
                radius = Mathf.Sqrt(Mathf.Pow(scale.z / 2f, 2f) + Mathf.Pow(scale.y / 2f, 2f));
                startAngleRad = Mathf.Atan2(scale.y, scale.z);
            }
        }
    }

    private void OnCollisionEnter(Collision theCollision)
    {
        if (theCollision.gameObject.tag == "Tile")
            isGrounded = true;
    }

    public void TriggerMove(int horizontal, int vertical)
    {
        if (isRotating || !isGrounded)
            return;

        if (moveClip != null)
            audioSource.PlayOneShot(moveClip);

        directionX = vertical;
        directionZ = horizontal;
        startPos = transform.position;
        preRotation = transform.rotation;
        transform.Rotate(directionZ * 90, 0, directionX * 90, Space.World);
        postRotation = transform.rotation;
        transform.rotation = preRotation;
        SetRadius();
        rotationTime = 0;
        isRotating = true;
        GameManager.moves += 1;
    }
}