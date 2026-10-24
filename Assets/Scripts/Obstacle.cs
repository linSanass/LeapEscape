using DG.Tweening;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Transform visualRoot;
    private string playerTag = "Player";
    private float hitScale = 1.3f;
    private float hitDuration = 0.15f;
    private Vector3 originalScale;

    void Start()
    {
        visualRoot = transform;
        originalScale = visualRoot.localScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        DOTween.Sequence().Append(visualRoot.DOScale(originalScale * hitScale, hitDuration)).Append(visualRoot.DOScale(originalScale, hitDuration));

        Debug.Log($"玩家撞到了 {name}");
    }
}