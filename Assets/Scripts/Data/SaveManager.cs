
using System.IO;
using UnityEngine;

/// <summary>
/// 本地存档管理器
/// </summary>
public static class SaveManager
{
    private static readonly string DataPath = Path.Combine(Application.persistentDataPath, "Data");
    private static readonly string SavePath = Path.Combine(DataPath, "GameData.json");

    /// <summary>
    /// 读取存档
    /// </summary>
    public static GameData Load()
    {
        // 首次启动时没有存档，返回默认数据
        if (!File.Exists(SavePath))
        {
            GameData newData = new GameData();
            Save(newData);
            return newData;
        }

        try
        {
            string json = File.ReadAllText(SavePath);
            GameData data = JsonUtility.FromJson<GameData>(json);

            if (data == null)
            {
                Debug.LogWarning("存档内容为空，创建默认存档。");
                data = new GameData();
                Save(data);
            }

            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"读取存档失败：{e.Message}");
            return new GameData();
        }
    }

    /// <summary>
    /// 保存存档
    /// </summary>
    public static void Save(GameData data)
    {
        if (data == null)
        {
            Debug.LogError("保存失败：存档数据为空！");
            return;
        }

        try
        {
            // 如果 Data 文件夹不存在，就创建它
            if (!Directory.Exists(DataPath))
            {
                Directory.CreateDirectory(DataPath);
            }

            // 将数据转换成 JSON
            string json = JsonUtility.ToJson(data, true);

            // 写入文件
            File.WriteAllText(SavePath, json);

            Debug.Log($"存档保存成功：{SavePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"保存存档失败：{e.Message}");
        }
    }

    /// <summary>
    /// 获取存档文件路径
    /// </summary>
    public static string GetSavePath()
    {
        return SavePath;
    }
}
