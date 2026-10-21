using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float pressThreshold = 0.15f;

    public float Direction { get; private set; } = 1f;
    public bool IsPressing { get; private set; }

    private bool isLeft;
    private float pressTime;

    void Update()
    {
        bool isDown, isHold, isUp;

        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            isDown = t.phase == TouchPhase.Began;
            isHold = t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary;
            isUp   = t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled;
        }
        else
        {
            isDown = Input.GetMouseButtonDown(0);
            isHold = Input.GetMouseButton(0);
            isUp   = Input.GetMouseButtonUp(0);
        }

        if (isDown)
        {
            isLeft = !isLeft;
            Direction = isLeft ? -1f : 1f;
            pressTime = 0f;
            IsPressing = false;
        }

        if (isHold)
        {
            pressTime += Time.deltaTime;
            IsPressing = pressTime > pressThreshold;
        }

        if (isUp)
        {
            pressTime = 0f;
            IsPressing = false;
        }
    }
}