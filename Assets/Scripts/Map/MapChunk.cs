using System.Collections.Generic;
using UnityEngine;

// 맵 내의 장애물 소환 위치
[System.Serializable]
public struct ObstacleSpawnData
{
    public int gridX;
    public int gridY;
    public float localZOffset;
}

// 맵 컴포넌트
public class MapChunk : MonoBehaviour, IPoolable
{
    [SerializeField] private float chunkLength = 20f;   // 맵 길이

    public List<GameObject> obstaclePrefabs = new List<GameObject>();           // 장애물 목록
    public List<ObstacleSpawnData> spawnPoints = new List<ObstacleSpawnData>(); // 장애물 위치 목록

    // 수거를 위한 장애물 정보
    private struct SpawnedObstacleInfo
    {
        public GameObject Instance;
        public GameObject PrefabKey;
    }

    private List<SpawnedObstacleInfo> spawnedObstacles = new List<SpawnedObstacleInfo>();

    // 맵 생성 시
    public void OnSpawn()
    {
        if (obstaclePrefabs.Count == 0 || spawnPoints.Count == 0 || ObjectPoolingManager.Instance == null)
        {
            return;
        }

        foreach (ObstacleSpawnData spawnData in spawnPoints)
        {
            // 스폰 지점에 장애물을 생성
            SpawnObstacle(spawnData);
        }
    }

    // 맵 제거 시
    public void OnDespawn()
    {
        // 맵이 생성한 장애물 수거
        if (ObjectPoolingManager.Instance != null)
        {
            foreach (SpawnedObstacleInfo info in spawnedObstacles)
            {
                ObjectPoolingManager.Instance.ReturnToPool(info.PrefabKey, info.Instance);
            }
        }

        spawnedObstacles.Clear();
    }

    // 장애물 생성
    private void SpawnObstacle(ObstacleSpawnData data)
    {
        int randomIndex = Random.Range(0, obstaclePrefabs.Count);

        GameObject prefabToSpawn = obstaclePrefabs[randomIndex];
        GameObject obstacleInstance = ObjectPoolingManager.Instance.SpawnFromPool(prefabToSpawn, Vector3.zero, Quaternion.identity);

        // 장애물의 z좌표 계산
        float absoluteZ = transform.position.z + data.localZOffset;
        GridPosition spawnGridPos = new GridPosition(data.gridX, data.gridY, GravityDirection.Down, absoluteZ);

        // 장애물 초기화
        ObstacleBase obstacleBase = obstacleInstance.GetComponent<ObstacleBase>();
        if (obstacleBase != null)
        {
            obstacleBase.Initialize(spawnGridPos, absoluteZ);
        }

        // 수거를 위해 리스트에 등록
        SpawnedObstacleInfo info = new SpawnedObstacleInfo();
        info.Instance = obstacleInstance;
        info.PrefabKey = prefabToSpawn;
        spawnedObstacles.Add(info);
    }

    public float GetChunkLength()
    {
        return chunkLength;
    }
}