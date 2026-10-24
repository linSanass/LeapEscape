using UnityEngine;

public class GameManager : MonoBehaviour
{
    enum GameState
    {
        GameStart,
        GamePause,
        GameOver
    }
    private GameState currentGameState;
    private PlayerInput input;
    private bool isStart;
    void Awake()
    {
        input = PlayerInput.Instance;
        input.SetInputEnabled(false);
    }

    void Update()
    {
        if (!isStart && input.Direction == 1)
        {
            EventCenter.TriggerEvent(Event.GameStart);
            currentGameState = GameState.GameStart;
            UIManager.Instance.Close<TutorialPanel>();
            isStart = true;
        }
    }
}
