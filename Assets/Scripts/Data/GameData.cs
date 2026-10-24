using System;

/// <summary>
/// 玩家整体存档数据
/// </summary>
[Serializable]
public class GameData
{
    public int currentLevel;
    public int bestScore;
    public bool musicOn;
    public bool sfxOn;

    public GameData(int currentLevel = 1, int bestScore = 0, bool musicOn = true, bool sfxOn = true)
    {
        this.currentLevel = currentLevel;
        this.bestScore = bestScore;
        this.musicOn = musicOn;
        this.sfxOn = sfxOn;
    }
}