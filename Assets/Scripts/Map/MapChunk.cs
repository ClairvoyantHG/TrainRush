using System.Collections.Generic;
using UnityEngine;

public class MapChunk : MonoBehaviour, IPoolable
{
    [SerializeField] private float chunkLength = 40f;
    private float currentZPosition;

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

    private List<SpawnedObstacleInfo> spawnedObstacles = new List<SpawnedObstacleInfo>();

    public float ChunkLength { get { return chunkLength; } }

    public void OnSpawn() { }

    public void SetupChunkData(MapPatternData data, float zPosition)
    {
        currentZPosition = zPosition;
        transform.position = new Vector3(0, 0, currentZPosition);

        SpawnObstaclesDynamically(data);
    }

    private void SpawnObstaclesDynamically(MapPatternData data)
    {
        if (data.ObstaclePrefabList == null || data.ObstaclePrefabList.Count == 0) return;
        if (data.SpawnPointsList == null || data.SpawnPointsList.Count == 0) return;

        for (int i = 0; i < data.SpawnPointsList.Count; i++)
        {
            if (Random.value > 0.7f) continue;

            string[] coords = data.SpawnPointsList[i].Split(':');
            if (coords.Length == 3)
            {
                int gridX; int gridY; float localZ;

                if (int.TryParse(coords[0], out gridX) &&
                    int.TryParse(coords[1], out gridY) &&
                    float.TryParse(coords[2], out localZ))
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
    }

    public void OnDespawn()
    {
        for (int i = 0; i < spawnedObstacles.Count; i++)
        {
            ObjectPoolingManager.Instance.ReturnToPool(spawnedObstacles[i].PrefabKey, spawnedObstacles[i].Obstacle.gameObject);
        }
        spawnedObstacles.Clear();
    }
}