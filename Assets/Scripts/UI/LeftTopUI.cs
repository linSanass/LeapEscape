using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LeftTopUI : UIPanel
{
    [SerializeField] private Button Setting_But;
    [SerializeField] private Button Music_Btn;
    [SerializeField] private Button Tip_Btn;
    private float openDuration  = 0.2f;
    private float closeDuration = 0.1f;

    private RectTransform musicRect, tipRect;
    private Vector2 musicTarget, tipTarget, hiddenPos;
    private Tween tween;
    private float progress;// 0 = 收起, 1 = 展开
    private bool  isExpanded;

    private void Awake()
    {
        musicRect = (RectTransform)Music_Btn.transform;
        tipRect = (RectTransform)Tip_Btn.transform;

        musicTarget = musicRect.anchoredPosition;
        tipTarget = tipRect.anchoredPosition;
        hiddenPos = ((RectTransform)Setting_But.transform).anchoredPosition;

        Setting_But.onClick.AddListener(() => SetExpanded(!isExpanded));
        SetCollapsed(0f); // 初始收起
    }

    /// <summary>
    /// 设置展开
    /// </summary>
    /// <param name="open"></param>
    private void SetExpanded(bool open)
    {
        isExpanded = open;
        tween?.Kill();

        float target = open ? 1f : 0f;
        float duration = open ? openDuration : closeDuration;
        Ease ease = open ? Ease.OutQuart : Ease.InQuart;

        tween = DOVirtual.Float(progress, target, duration, SetCollapsed).SetEase(ease);
    }

    /// <summary>
    /// 设置收起
    /// </summary>
    /// <param name="t"></param>
    private void SetCollapsed(float t)
    {
        progress = t;
        musicRect.anchoredPosition = Vector2.Lerp(hiddenPos, musicTarget, t);
        tipRect.anchoredPosition = Vector2.Lerp(hiddenPos, tipTarget, t);
        Music_Btn.interactable = Tip_Btn.interactable = t > 0.5f;
    }
}