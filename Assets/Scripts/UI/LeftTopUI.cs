using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LeftTopUI : UIPanel
{
    [SerializeField] private Button Setting_But;
    [SerializeField] private Button Music_Btn;
    [SerializeField] private Button Tip_Btn;

    [SerializeField] private float openDuration  = 0.25f;
    [SerializeField] private float closeDuration = 0.2f;

    private RectTransform musicRect, tipRect;
    private Vector2 musicTarget, tipTarget;
    private Vector2 hiddenPos;
    private bool isExpanded;

    private void Awake()
    {
        musicRect = (RectTransform)Music_Btn.transform;
        tipRect   = (RectTransform)Tip_Btn.transform;

        // 记录编辑器里摆好的最终位置
        musicTarget = musicRect.anchoredPosition;
        tipTarget   = tipRect.anchoredPosition;

        // 收起位置 = 设置按钮的位置（水平对齐，垂直贴着按钮）
        hiddenPos = ((RectTransform)Setting_But.transform).anchoredPosition;

        Setting_But.onClick.AddListener(Toggle);
        SetExpanded(false, true);   // 初始收起，不播动画
    }

    private void Toggle()
    {
        isExpanded = !isExpanded;
        SetExpanded(isExpanded, false);
    }

    private void SetExpanded(bool open, bool instant)
    {
        musicRect.DOKill();
        tipRect.DOKill();
        Music_Btn.interactable = open;
        Tip_Btn.interactable   = open;

        if (open)
        {
            Music_Btn.gameObject.SetActive(true);
            Tip_Btn.gameObject.SetActive(true);
        }

        if (instant)
        {
            musicRect.anchoredPosition = open ? musicTarget : hiddenPos;
            tipRect.anchoredPosition   = open ? tipTarget   : hiddenPos;
            if (!open)
            {
                Music_Btn.gameObject.SetActive(false);
                Tip_Btn.gameObject.SetActive(false);
            }
            return;
        }

        if (open)
        {
            // 从按钮位置滑到目标位置
            musicRect.anchoredPosition = hiddenPos;
            tipRect.anchoredPosition   = hiddenPos;
            musicRect.DOAnchorPos(musicTarget, openDuration).SetEase(Ease.OutQuart);
            tipRect.DOAnchorPos(tipTarget, openDuration).SetEase(Ease.OutQuart);
        }
        else
        {
            musicRect.DOAnchorPos(hiddenPos, closeDuration).SetEase(Ease.InQuart);
            tipRect.DOAnchorPos(hiddenPos, closeDuration).SetEase(Ease.InQuart)
                .OnComplete(() =>
                {
                    if (!isExpanded)
                    {
                        Music_Btn.gameObject.SetActive(false);
                        Tip_Btn.gameObject.SetActive(false);
                    }
                });
        }
    }
}