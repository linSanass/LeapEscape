using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : SingLetonAutoMono<PlayerInput>, IPointerDownHandler, IPointerUpHandler
{
    public float pressThreshold = 0.15f;

    public int Direction { get; private set; } = -1;
    public bool IsPressing { get; private set; }
    public bool IsHolding { get; private set; }

    // 开局关闭，开始游戏后再打开
    public bool IsInputEnabled { get; private set; } = false;

    private bool isLeft = true;
    private float pressTime;

    public void SetInputEnabled(bool value)
    {
        if (IsInputEnabled == value) return;
        IsInputEnabled = value;
        if (!value) ResetState();
    }

    private void ResetState()
    {
        IsHolding = false;
        IsPressing = false;
        pressTime = 0f;
    }

    public void OnPointerDown(PointerEventData e)
    {
        if (!IsInputEnabled) return;
        
        isLeft = !isLeft;
        Direction = isLeft ? -1 : 1;
        pressTime = 0f;
        IsPressing = false;
        IsHolding = true;
    }

    public void OnPointerUp(PointerEventData e)
    {
        ResetState();
    }

    void Update()
    {
        if (!IsInputEnabled) return;
        if (IsHolding)
        {
            pressTime += Time.deltaTime;
            IsPressing = pressTime > pressThreshold;
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus) ResetState();
    }
}