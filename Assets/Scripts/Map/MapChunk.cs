using System.Collections.Generic;
using UnityEngine;

// 맵 구간 컴포넌트
public class MapChunk : MonoBehaviour, IPoolable
{
    [SerializeField] private float chunkLength = 40f;
    private float currentZPosition;
    private float obstacleRate = 0.0f;

    // 오브젝트 풀링 관리용 장애물 정보
    private struct SpawnedObstacleInfo
    {
        public GameObject PrefabKey; 
        public ObstacleBase Obstacle;

        public SpawnedObstacleInfo(GameObject prefabKey, ObstacleBase obstacle)
        {
            PrefabKey = prefabKey;
            Obstacle = obstacle;
        }
    }

    private struct SpawnedItemInfo
    {
        public GameObject PrefabKey;
        public ItemPickable Item;

        public SpawnedItemInfo(GameObject prefabKey, ItemPickable item)
        {
            PrefabKey = prefabKey;
            Item = item;
        }
    }

    private List<SpawnedObstacleInfo> spawnedObstacles = new List<SpawnedObstacleInfo>();
    private List<SpawnedItemInfo> spawnedItems = new List<SpawnedItemInfo>();

    public float ChunkLength { get { return chunkLength; } }

    public void OnSpawn() { }

    // 맵 데이터 세팅
    public void SetupChunkData(MapPatternData data, float zPosition)
    {
        currentZPosition = zPosition;
        transform.position = new Vector3(0, 0, currentZPosition);

        SpawnObjectDynamically(data);
    }

    // 장애물 배치
    private void SpawnObjectDynamically(MapPatternData data)
    {
        if (data.ObstaclePrefabList == null || data.ObstaclePrefabList.Count == 0) return;
        if (data.ItemPrefabList == null || data.ItemPrefabList.Count == 0) return;
        if (data.SpawnPointsList == null || data.SpawnPointsList.Count == 0) return;

        for (int i = 0; i < data.SpawnPointsList.Count; i++)
        {
            // 장애물, 아이템 확률적 스폰
            if (Random.value < obstacleRate) 
            { 
                string[] coords = data.SpawnPointsList[i].Split(':');

                if (coords.Length == 3)
                {
                    int gridX; int gridY; float localZ;

                    if (int.TryParse(coords[0], out gridX) && int.TryParse(coords[1], out gridY) && float.TryParse(coords[2], out localZ))
                    {
                        int randomIndex = Random.Range(0, data.ObstaclePrefabList.Count);
                        string selectedPrefabId = data.ObstaclePrefabList[randomIndex];

                        GameObject loadedPrefab = Resources.Load<GameObject>("Prefabs/Obstacles/" + selectedPrefabId);

                        if (loadedPrefab != null)
                        {
                            GameObject obstacleObj = ObjectPoolingManager.Instance.SpawnFromPool(loadedPrefab, Vector3.zero, Quaternion.identity);

                            if (obstacleObj != null)
                            {
                                ObstacleBase obstacle = obstacleObj.GetComponent<ObstacleBase>();

                                if (obstacle != null)
                                {
                                    GridPosition spawnPos = new GridPosition(gridX, gridY, GravityDirection.Down, currentZPosition + localZ);
                                    obstacle.Initialize(spawnPos, currentZPosition + localZ);

                                    spawnedObstacles.Add(new SpawnedObstacleInfo(loadedPrefab, obstacle));
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                string[] coords = data.SpawnPointsList[i].Split(':');

                if (coords.Length == 3)
                {
                    int gridX; int gridY; float localZ;

                    if (int.TryParse(coords[0], out gridX) && int.TryParse(coords[1], out gridY) && float.TryParse(coords[2], out localZ))
                    {
                        int randomIndex = Random.Range(0, data.ItemPrefabList.Count);
                        string selectedPrefabId = data.ItemPrefabList[randomIndex];

                        GameObject loadedPrefab = Resources.Load<GameObject>("Prefabs/Items/" + selectedPrefabId);

                        if (loadedPrefab != null)
                        {
                            GameObject itemObj = ObjectPoolingManager.Instance.SpawnFromPool(loadedPrefab, Vector3.zero, Quaternion.identity);

                            if (itemObj != null)
                            {
                                ItemPickable item = itemObj.GetComponent<ItemPickable>();

                                if (item != null)
                                {
                                    GridPosition spawnPos = new GridPosition(gridX, gridY, GravityDirection.Down, currentZPosition + localZ);

                                    item.Initialize(spawnPos, currentZPosition + localZ);

                                    spawnedItems.Add(new SpawnedItemInfo(loadedPrefab, item));
                                }
                            }
                        }
                        else
                        {
                            Debug.Log("살려줘");
                        }
                    }
                }
            }
        }
    }



    public void OnDespawn()
    {
        // 오브젝트 풀 반납
        for (int i = 0; i < spawnedObstacles.Count; i++)
        {
            ObjectPoolingManager.Instance.ReturnToPool(spawnedObstacles[i].PrefabKey, spawnedObstacles[i].Obstacle.gameObject);
        }

        spawnedObstacles.Clear();

        for (int i = 0; i < spawnedItems.Count; i++)
        {
            ObjectPoolingManager.Instance.ReturnToPool(spawnedItems[i].PrefabKey, spawnedItems[i].Item.gameObject);
        }

        spawnedItems.Clear();
    }
}