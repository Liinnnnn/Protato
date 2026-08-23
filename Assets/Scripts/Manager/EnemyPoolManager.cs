using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager Instance { get; private set; }

    // Lưu các Pool tương ứng với từng Prefab
    private readonly Dictionary<GameObject, IObjectPool<GameObject>> _pools = new Dictionary<GameObject, IObjectPool<GameObject>>();
    
    // Theo dõi toàn bộ Enemy đang hoạt động trên màn hình
    private readonly List<GameObject> _activeEnemies = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Đăng ký trước các Prefab vào Pool để sẵn sàng sinh ra trong game.
    /// </summary>
    public void RegisterPrefab(GameObject prefab, int defaultCapacity = 10, int maxSize = 50)
    {
        if (prefab == null || _pools.ContainsKey(prefab)) return;

        IObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => CreateEnemyInstance(prefab),
            actionOnGet: OnGetEnemy,
            actionOnRelease: OnReleaseEnemy,
            actionOnDestroy: OnDestroyEnemy,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );

        _pools.Add(prefab, pool);
    }

    /// <summary>
    /// Lấy 1 enemy từ Pool ra vị trí truyền vào.
    /// </summary>
    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!_pools.ContainsKey(prefab))
        {
            RegisterPrefab(prefab);
        }

        GameObject enemy = _pools[prefab].Get();
        enemy.transform.position = position;
        enemy.transform.rotation = rotation;

        if (parent != null)
        {
            enemy.transform.SetParent(parent);
        }

        return enemy;
    }

    /// <summary>
    /// Thu hồi 1 enemy về lại Pool.
    /// </summary>
    public void Despawn(GameObject enemy)
    {
        if (enemy.TryGetComponent<PooledEnemyTracker>(out var tracker) && tracker.OriginPrefab != null)
        {
            if (_pools.TryGetValue(tracker.OriginPrefab, out var pool))
            {
                pool.Release(enemy);
                return;
            }
        }

        // Nếu enemy không nằm trong Pool thì tự hủy
        Destroy(enemy);
    }

    /// <summary>
    /// Dọn dẹp/Thu hồi tất cả Enemy đang xuất hiện về lại Pool.
    /// </summary>
    public void DespawnAllActive()
    {
        for (int i = _activeEnemies.Count - 1; i >= 0; i--)
        {
            Despawn(_activeEnemies[i]);
        }
        _activeEnemies.Clear();
    }

    #region Pool Internal Callbacks
    private GameObject CreateEnemyInstance(GameObject prefab)
    {
        GameObject instance = Instantiate(prefab);
        
        // Thêm thẻ đánh dấu xem nó sinh ra từ Prefab gốc nào
        PooledEnemyTracker tracker = instance.GetComponent<PooledEnemyTracker>();
        if (tracker == null)
        {
            tracker = instance.AddComponent<PooledEnemyTracker>();
        }
        tracker.OriginPrefab = prefab;

        return instance;
    }

    private void OnGetEnemy(GameObject enemy)
    {
        enemy.SetActive(true);
        _activeEnemies.Add(enemy);
    }

    private void OnReleaseEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        _activeEnemies.Remove(enemy);
    }

    private void OnDestroyEnemy(GameObject enemy)
    {
        Destroy(enemy);
    }
    #endregion
}

/// <summary>
/// Component phụ dùng để lưu thông tin Prefab nguồn.
/// </summary>
public class PooledEnemyTracker : MonoBehaviour
{
    public GameObject OriginPrefab { get; set; }
}