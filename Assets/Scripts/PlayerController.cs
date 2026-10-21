using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speedX = 3f;
    [SerializeField] private float learpSpeedX = 2f;

    [SerializeField] private float pressSpeedX = 5f;
    [SerializeField] private float speedY = 2f;
    [SerializeField] private PlayerInput input;

    private Vector2 velocity;

    void Update()
    {
        float targetX = (input.IsPressing ? pressSpeedX : speedX) * input.Direction;

        velocity.x = Mathf.Lerp(velocity.x, targetX, Time.deltaTime * learpSpeedX);
        velocity.y = Mathf.Lerp(velocity.y, -speedY, Time.deltaTime * learpSpeedX);

        transform.Translate(velocity * Time.deltaTime);
    }
}