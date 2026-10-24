
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 对象池管理器
/// </summary>
public class ObjectPoolManager : SingLetonAutoMono<ObjectPoolManager>
{
    private Dictionary<GameObject, ObjectPool<GameObject>> pools = new Dictionary<GameObject, ObjectPool<GameObject>>();

    /// <summary>
    /// 获取对象
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public GameObject Get(GameObject prefab)
    {
        if (!pools.TryGetValue(prefab, out var pool))
        {
            pool = CreatePool(prefab);
            pools.Add(prefab, pool);
        }

        return pool.Get();
    }

    /// <summary>
    /// 创建指定 Prefab 的对象池
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    private ObjectPool<GameObject> CreatePool(GameObject prefab)
    {
        return new ObjectPool<GameObject>(
            () =>
            {
                GameObject obj = Instantiate(prefab, transform);
                obj.SetActive(false);
                return obj;
            },
            obj =>
            {
                obj.SetActive(true);
            },
            obj =>
            {
                obj.SetActive(false);
                obj.transform.SetParent(transform);
            },
            obj =>
            {
                Destroy(obj);
            }
        );
    }

    /// <summary>
    /// 归还对象
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="obj"></param>
    public void Release(GameObject prefab, GameObject obj)
    {
        if (pools.TryGetValue(prefab, out var pool))
        {
            pool.Release(obj);
        }
        else
        {
            Debug.LogWarning("没有找到该对象对应的对象池！");
        }
    }

    private void OnDestroy()
    {
        foreach (var pool in pools.Values)
        {
            pool.Dispose();
        }

        pools.Clear();
    }
}
