using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speedX = 3f;
    [SerializeField] private float learpSpeedX = 2f;

    [SerializeField] private float pressSpeedX = 5f;
    [SerializeField] private float speedY = 2f;
    [SerializeField] private PlayerInput input;
    private Collider2D col;
    private Camera mainCamera;
    private Vector2 velocity;
    private bool isDead;

    void Start()
    {
        mainCamera = Camera.main;
        col = GetComponent<Collider2D>();
    }
    void Update()
    {
        if (isDead) return;

        float targetX = (input.IsPressing ? pressSpeedX : speedX) * input.Direction;

        velocity.x = Mathf.Lerp(velocity.x, targetX, Time.deltaTime * learpSpeedX);
        velocity.y = Mathf.Lerp(velocity.y, -speedY, Time.deltaTime * learpSpeedX);

        transform.Translate(velocity * Time.deltaTime);

        CheckScreenEdge();
    }

    private void CheckScreenEdge()
    {
        float halfWidth = mainCamera.orthographicSize * mainCamera.aspect;

        float screenLeft  = mainCamera.transform.position.x - halfWidth;
        float screenRight = mainCamera.transform.position.x + halfWidth;

        if (col.bounds.min.x <= screenLeft || col.bounds.max.x >= screenRight)
            Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("死亡");
    }
}