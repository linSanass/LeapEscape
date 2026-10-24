
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;

    [Header("生成数量")]
    public int obstacleNum = 250;

    [Header("障碍物最大直径")]
    public float obstacleMaxDiameter = 0.3f;

    [Header("障碍物最小直径")]
    public float obstacleMinDiameter = 0.15f;

    [Header("障碍物间距")]
    public float spacingX = 0.1f;
    public float spacingY = 0.1f;

    [Header("随机偏移")]
    [Range(0f, 0.4f)]
    public float randomOffset = 0.2f;

    [Header("纵向候选区域")]
    public int rowCount = 100;

    // Prefab 原始直径
    private const float prefabDiameter = 0.3f;

    private Camera mainCamera;
    private float halfWidth;

    // 保存当前生成的障碍物，方便重新生成时归还对象池
    private readonly List<GameObject> spawnedList = new List<GameObject>();

    private void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("没有找到 MainCamera");
            return;
        }

        halfWidth = mainCamera.orthographicSize * mainCamera.aspect;

        ReGenerate();
    }

    /// <summary>
    /// 归还旧障碍物，并重新生成
    /// </summary>
    [ContextMenu("重新生成")]
    public void ReGenerate()
    {
        // 将旧障碍物归还对象池
        for (int i = 0; i < spawnedList.Count; i++)
        {
            if (spawnedList[i] != null)
            {
                ObjectPoolManager.Instance.Release(
                    obstaclePrefab,
                    spawnedList[i]
                );
            }
        }

        spawnedList.Clear();

        CreateObstacle();
    }

    /// <summary>
    /// 计算布局并生成障碍物
    /// </summary>
    private void CreateObstacle()
    {
        if (rowCount <= 0 || obstaclePrefab == null)
            return;

        float intervalX = obstacleMaxDiameter + spacingX;
        float intervalY = obstacleMaxDiameter + spacingY;

        if (intervalX <= 0f || intervalY <= 0f)
            return;

        float offsetX = spacingX * randomOffset;
        float offsetY = spacingY * randomOffset;

        float minX = -halfWidth + obstacleMaxDiameter / 2f + offsetX;
        float maxX = halfWidth - obstacleMaxDiameter / 2f - offsetX;

        if (minX > maxX)
            return;

        int columnCount = Mathf.FloorToInt(
            (maxX - minX) / intervalX
        ) + 1;

        float startX = -(columnCount - 1) * intervalX / 2f;
        float startY = -mainCamera.orthographicSize / 5f;

        int count = Mathf.Min(obstacleNum, rowCount * columnCount);

        // 平均分配每行的障碍物数量
        int basePerRow = count / rowCount;
        int extra = count % rowCount;

        int[] perRow = new int[rowCount];

        for (int i = 0; i < rowCount; i++)
        {
            perRow[i] = basePerRow;
        }

        // 随机打乱行号
        int[] rowOrder = new int[rowCount];

        for (int i = 0; i < rowCount; i++)
        {
            rowOrder[i] = i;
        }

        for (int i = rowCount - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            int temp = rowOrder[i];
            rowOrder[i] = rowOrder[j];
            rowOrder[j] = temp;
        }

        // 将剩余数量随机分配给不同的行
        for (int i = 0; i < extra; i++)
        {
            perRow[rowOrder[i]]++;
        }

        // 每行随机选择不重复的列
        for (int y = 0; y < rowCount; y++)
        {
            if (perRow[y] <= 0)
                continue;

            HashSet<int> usedX = new HashSet<int>();

            while (usedX.Count < perRow[y])
            {
                usedX.Add(Random.Range(0, columnCount));
            }

            foreach (int x in usedX)
            {
                float posX = startX
                    + x * intervalX
                    + Random.Range(-offsetX, offsetX);

                float posY = startY
                    - y * intervalY
                    + Random.Range(-offsetY, offsetY);

                CreateSingleObstacle(
                    new Vector2(posX, posY)
                );
            }
        }
    }

    /// <summary>
    /// 从对象池获取障碍物并设置状态
    /// </summary>
    private void CreateSingleObstacle(Vector2 position)
    {
        GameObject obstacle =
            ObjectPoolManager.Instance.Get(obstaclePrefab);

        // 重置位置和旋转
        obstacle.transform.position =
            new Vector3(position.x, position.y, 0f);

        obstacle.transform.rotation = Quaternion.identity;

        // 随机设置障碍物大小
        float diameter = Random.Range(
            obstacleMinDiameter,
            obstacleMaxDiameter
        );

        obstacle.transform.localScale =
            Vector3.one * (diameter / prefabDiameter);

        spawnedList.Add(obstacle);
    }
}