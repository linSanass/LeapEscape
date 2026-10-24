using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LeftTopUI : UIPanel
{
    [SerializeField] private Button Setting_But;
    [SerializeField] private Button Music_Btn;
    [SerializeField] private Button Tip_Btn;
    [SerializeField] private GameObject MusicOpen_Obj;
    [SerializeField] private GameObject Open_Obj;
    private float openDuration  = 0.2f;
    private float closeDuration = 0.1f;

    private RectTransform musicRect, tipRect;
    private Vector2 musicTarget, tipTarget, hiddenPos;
    private Tween tween;
    private float progress;// 0 = 收起, 1 = 展开
    private bool  isExpanded;

    private void Awake()
    {
        Init();
        Subscribe();
    }
    private void Init()
    {
        musicRect = (RectTransform)Music_Btn.transform;
        tipRect = (RectTransform)Tip_Btn.transform;

        musicTarget = musicRect.anchoredPosition;
        tipTarget = tipRect.anchoredPosition;
        hiddenPos = ((RectTransform)Setting_But.transform).anchoredPosition;
        SetCollapsed(0f); // 初始收起

        Setting_But.onClick.AddListener(() => SetExpanded(!isExpanded));
        Music_Btn.onClick.AddListener(OpenOrCloseBGM);
        Tip_Btn.onClick.AddListener(OpenOrCloseSFX);
    }

    void OnDestroy()
    {
        Unsubscribe();
    }
    private void Subscribe()
    {
        EventCenter.AddListener(Event.OnDataInit, HandleOnDataInit);
        EventCenter.AddListener(Event.GameStart, HandleGameStart);
        EventCenter.AddListener(Event.OnMusicChanged, RefreshAudioUI);
        EventCenter.AddListener(Event.OnSfxChanged, RefreshAudioUI);
    }
    private void Unsubscribe()
    {
        EventCenter.RemoveListener(Event.OnDataInit, HandleOnDataInit);
        EventCenter.AddListener(Event.GameStart, HandleGameStart);
        EventCenter.RemoveListener(Event.OnMusicChanged, RefreshAudioUI);
        EventCenter.RemoveListener(Event.OnSfxChanged, RefreshAudioUI);
    }
    private void HandleOnDataInit()
    {
        ShowButton();
        RefreshAudioUI();
    }
    private void HandleGameStart()
    {
        SetExpanded(false);
        tween.OnComplete(HideButton);
    }
    private void RefreshAudioUI()
    {
        var d = GameManager.Instance.GameData;

        AudioManager.Instance.SetBGMMute(!d.musicOn);
        MusicOpen_Obj.SetActive(d.musicOn);

        AudioManager.Instance.SetSFXMute(!d.sfxOn);
        Open_Obj.SetActive(d.sfxOn);
    }
    private void OpenOrCloseBGM()
    {
        GameManager.Instance.SetMusicEnabled(!GameManager.Instance.GameData.musicOn);
    }

    private void OpenOrCloseSFX()
    {
        GameManager.Instance.SetSfxEnabled(!GameManager.Instance.GameData.sfxOn);
    }

    public void ShowButton()
    {
        Setting_But.DOKill();
        Setting_But.transform.localScale = Vector3.zero;
        Setting_But.transform.DOScale(1f, 1f).SetEase(Ease.OutBack);
    }

    public void HideButton()
    {
        Setting_But.transform.DOKill();
        Setting_But.transform.DOScale(0f, 0.8f).SetEase(Ease.InBack);
    }
    /// <summary>
    /// 设置展开
    /// </summary>
    /// <param name="open"></param>
    private void SetExpanded(bool open)
    {
        isExpanded = open;
        tween?.Kill();

        if (open)
        {
            musicRect.gameObject.SetActive(true);
            tipRect.gameObject.SetActive(true);
        }

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

        bool visible = t > 0f;
        if (musicRect.gameObject.activeSelf != visible)
            musicRect.gameObject.SetActive(visible);
        if (tipRect.gameObject.activeSelf != visible)
            tipRect.gameObject.SetActive(visible);
    }
}