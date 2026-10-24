using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单关配置数据
/// </summary>
public class LevelData
{
    public int levelId;
    public Color[] colors;

    public LevelData(int levelId, Color[] colors)
    {
        this.levelId = levelId;
        this.colors = colors;
    }
}

/// <summary>
/// 固定关卡配置管理器
/// </summary>
public class LevelConfigManager
{
    private readonly List<LevelData> levels = new List<LevelData>();

    private readonly Color[] palette =
    {
        new Color32(0xE7, 0x4C, 0x3C, 0xFF),
        new Color32(0xE6, 0x7E, 0x22, 0xFF),
        new Color32(0xF1, 0xC4, 0x0F, 0xFF),
        new Color32(0x2E, 0xCC, 0x71, 0xFF),
        new Color32(0x1A, 0xBC, 0x9C, 0xFF),
        new Color32(0x34, 0x98, 0xDB, 0xFF),
        new Color32(0x9B, 0x59, 0xB6, 0xFF),
        new Color32(0x34, 0x49, 0x5E, 0xFF),
        new Color32(0x95, 0xA5, 0xA6, 0xFF),
        new Color32(0xFF, 0x6B, 0x81, 0xFF),
        new Color32(0xFF, 0xA5, 0x02, 0xFF),
        new Color32(0x7B, 0xED, 0x9F, 0xFF),
        new Color32(0x70, 0xA1, 0xFF, 0xFF),
        new Color32(0xA5, 0x5E, 0xEA, 0xFF),
        new Color32(0x57, 0x65, 0x74, 0xFF),
        new Color32(0x8E, 0x2B, 0x2B, 0xFF),
        new Color32(0xA0, 0x52, 0x2D, 0xFF),
        new Color32(0x1E, 0x84, 0x49, 0xFF),
        new Color32(0x1B, 0x4F, 0x72, 0xFF),
        new Color32(0xEC, 0xF0, 0xF1, 0xFF)
    };

    public LevelConfigManager()
    {
        CreateLevels();
    }

    private void CreateLevels()
    {
        AddLevel(1, 0, 1, 2, 3);
        AddLevel(2, 4, 5, 6, 9);
        AddLevel(3, 7, 8, 11, 12);
        AddLevel(4, 10, 13, 14, 15);
        AddLevel(5, 16, 17, 18, 19);

        AddLevel(6, 0, 4, 8, 12);
        AddLevel(7, 1, 5, 9, 13);
        AddLevel(8, 2, 6, 10, 14);
        AddLevel(9, 3, 7, 11, 15);
        AddLevel(10, 4, 8, 16, 18);

        AddLevel(11, 0, 5, 10, 15);
        AddLevel(12, 1, 6, 11, 16);
        AddLevel(13, 2, 7, 12, 17);
        AddLevel(14, 3, 8, 13, 18);
        AddLevel(15, 4, 9, 14, 19);

        AddLevel(16, 0, 6, 12, 18);
        AddLevel(17, 1, 7, 13, 19);
        AddLevel(18, 2, 8, 14, 16);
        AddLevel(19, 3, 9, 15, 17);
        AddLevel(20, 5, 10, 14, 19);
    }

    private void AddLevel(int levelId, params int[] colorIds)
    {
        Color[] colors = new Color[colorIds.Length];

        for (int i = 0; i < colorIds.Length; i++)
        {
            colors[i] = palette[colorIds[i]];
        }

        levels.Add(new LevelData(levelId, colors));
    }

    public LevelData GetLevel(int levelId)
    {
        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i].levelId == levelId)
                return levels[i];
        }
        return null;
    }

    public LevelData GetNextLevel(int levelId)
    {
        return GetLevel(levelId + 1);
    }

    public LevelData GetPreviousLevel(int levelId)
    {
        return GetLevel(levelId - 1);
    }

    public int GetLevelCount()
    {
        return levels.Count;
    }
}