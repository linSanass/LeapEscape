using UnityEngine;

public class ObstacleDeathZone : MonoBehaviour
{
    private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        EventCenter.TriggerEvent(Event.GameOver);
        Debug.Log("玩家死亡");
    }
}