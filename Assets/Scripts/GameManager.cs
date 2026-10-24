using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum GameState
    {
        GameStart,
        GamePause,
        GameOver
    }

    private GameState currentGameState;

    private PlayerInput input;

    // 固定关卡配置管理器
    private LevelConfigManager levelConfigManager;

    // 玩家存档
    private GameData gameData;

    // 当前关卡配置
    private LevelData currentLevelData;

    private bool isStart;

    public LevelData CurrentLevelData => currentLevelData;

    public GameData GameData => gameData;

    private void Awake()
    {
        PrepareData();
    }

    private void Update()
    {
        // 等待玩家输入开始游戏
        if (!isStart && input.Direction == 1)
        {
            StartGame();
            isStart = true;
        }
    }

    /// <summary>
    /// 准备初始数据
    /// </summary>
    private void PrepareData()
    {
        // 创建关卡配置
        levelConfigManager = new LevelConfigManager();

        // 读取玩家存档
        gameData = SaveManager.Load();

        // 加载当前关卡
        LoadCurrentLevel();

        // 初始状态
        input = PlayerInput.Instance;
        input.SetInputEnabled(false);
        currentGameState = GameState.GamePause;
    }

    /// <summary>
    /// 加载当前关卡配置
    /// </summary>
    private bool LoadCurrentLevel()
    {
        currentLevelData = levelConfigManager.GetLevel(gameData.currentLevel);

        if (currentLevelData == null)
        {
            Debug.LogError($"找不到第 {gameData.currentLevel} 关配置！");
            return false;
        }

        return true;
    }

    /// <summary>
    /// 开始当前关卡
    /// </summary>
    public void StartGame()
    {
        currentGameState = GameState.GameStart;
        input.SetInputEnabled(true);
        UIManager.Instance.Close<TutorialPanel>();
        EventCenter.TriggerEvent(Event.GameStart);
    }

    /// <summary>
    /// 当前关卡通关
    /// </summary>
    public void CompleteCurrentLevel(int score)
    {
        currentGameState = GameState.GameOver;
        input.SetInputEnabled(false);

        // 更新当前关卡历史最高分
        gameData.bestScore = score > gameData.bestScore ? score : gameData.bestScore;

        // 保存本关最高分
        SaveManager.Save(gameData);

        Debug.Log($"第 {gameData.currentLevel} 关通关，" + $"本次分数：{score}，" + $"历史最高分：{gameData.bestScore}");
    }

    /// <summary>
    /// 进入下一关
    /// </summary>
    public void EnterNextLevel()
    {
        int nextLevel = gameData.currentLevel + 1;

        // 判断是否还有下一关
        if (levelConfigManager.GetLevel(nextLevel) == null)
        {
            Debug.Log("恭喜！所有关卡已通关。");
            return;
        }

        // 更新关卡编号
        gameData.currentLevel = nextLevel;

        // 新关卡的最高分从 0 开始记录
        gameData.bestScore = 0;

        // 加载下一关配置
        if (!LoadCurrentLevel())
        {
            return;
        }

        // 重置游戏状态
        isStart = false;
        currentGameState = GameState.GamePause;
        input.SetInputEnabled(false);

        // 保存当前关卡
        SaveManager.Save(gameData);
        
    }

    /// <summary>
    /// 修改音乐开关
    /// </summary>
    public void SetMusicEnabled(bool enabled)
    {
        gameData.musicOn = enabled;
        SaveManager.Save(gameData);
    }

    /// <summary>
    /// 修改音效开关
    /// </summary>
    public void SetSfxEnabled(bool enabled)
    {
        gameData.sfxOn = enabled;
        SaveManager.Save(gameData);
    }

    /// <summary>
    /// 暂停游戏
    /// </summary>
    public void PauseGame()
    {
        if (currentGameState == GameState.GamePause)
            return;

        currentGameState = GameState.GamePause;
        input.SetInputEnabled(false);
    }
}