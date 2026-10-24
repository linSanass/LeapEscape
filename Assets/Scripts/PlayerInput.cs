using UnityEngine;

public class PlayerInput : SingLetonAutoMono<PlayerInput>
{
    public float pressThreshold = 0.15f;

    public int Direction { get; private set; } = -1;
    public bool IsPressing { get; private set; }
    public bool IsInputEnabled { get; private set; } = true;

    private bool isLeft = true;
    private float pressTime;

    public void SetInputEnabled(bool value)
    {
        if (IsInputEnabled == value) return;
        IsInputEnabled = value;
        pressTime = 0f;
        IsPressing = false;
    }

    void Update()
    {
        if (!IsInputEnabled) return;

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
            Direction = isLeft ? -1 : 1;
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