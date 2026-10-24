using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeControl : MonoBehaviour
{
    [SerializeField] private Image fade_Image;

    void Awake()
    {
        fade_Image.color = new Color(fade_Image.color.r, fade_Image.color.g, fade_Image.color.b, 1f);
        Subscribe();
    }
    void Start()
    {
        HandleGameStart();
    }
    void OnDestroy()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        // EventCenter.AddListener(Event.GameStart, HandleGameStart);
        EventCenter.AddListener(Event.GameOver, HandleGameOver);
    }
    private void Unsubscribe()
    {
        // EventCenter.RemoveListener(Event.GameStart, HandleGameStart);
        EventCenter.RemoveListener(Event.GameOver, HandleGameOver);
    }

    private void HandleGameStart()
    {
        fade_Image.DOFade(0f, 1f).SetEase(Ease.InQuart).OnComplete(() => PlayerInput.Instance.SetInputEnabled(true));
    }
    private void HandleGameOver()
    {
        fade_Image.DOFade(1f, 1f).SetEase(Ease.OutQuart);
    }
}