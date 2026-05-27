using System.Collections.Generic;
using UnityEngine;

// 맵 생성 매니저
public class MapGenerator : SingletonBase<MapGenerator>
{
    [Header("Stage Settings")]
    [SerializeField] private string currentStageId = "Stage_001";
    [SerializeField] private int initialChunkCount = 5;         // 시작 청크 수

    private struct ActiveChunkInfo
    {
        public GameObject PrefabKey;
        public MapChunk Chunk;

        public ActiveChunkInfo(GameObject prefabKey, MapChunk chunk)
        {
            PrefabKey = prefabKey;
            Chunk = chunk;
        }
    }

    private List<string> stageMapPatternIds = new List<string>();
    private float nextSpawnZ = 0f;
    private int currentChunkIndex = 0;

    private Queue<ActiveChunkInfo> activeChunks = new Queue<ActiveChunkInfo>();

    private void Start()
    {
        StageData stageData = GameDataManager.Instance.GetData<StageData>(currentStageId);
        if (stageData != null && stageData.MapPatternIdList != null && stageData.MapPatternIdList.Count > 0)
        {
            stageMapPatternIds = stageData.MapPatternIdList;
        }
        else
        {
            Debug.LogError("[MapGenerator] 스테이지 데이터를 불러올 수 없습니다: " + currentStageId);
            return;
        }

        for (int i = 0; i < initialChunkCount; i++)
        {
            SpawnNextChunk();
        }
    }

    public void SpawnNextChunk()
    {
        string targetPatternId = stageMapPatternIds[currentChunkIndex];
        currentChunkIndex = (currentChunkIndex + 1) % stageMapPatternIds.Count;

        MapPatternData patternData = GameDataManager.Instance.GetData<MapPatternData>(targetPatternId);
        if (patternData == null) return;

        GameObject loadedMapPrefab = Resources.Load<GameObject>("Prefabs/MapChunks/" + patternData.MapPrefabId);
        if (loadedMapPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(0f, 0f, nextSpawnZ);
            GameObject chunkObj = ObjectPoolingManager.Instance.SpawnFromPool(loadedMapPrefab, spawnPosition, Quaternion.identity);

            if (chunkObj != null)
            {
                MapChunk chunk = chunkObj.GetComponent<MapChunk>();
                if (chunk != null)
                {
                    chunk.SetupChunkData(patternData, nextSpawnZ);

                    nextSpawnZ += chunk.ChunkLength;

                    activeChunks.Enqueue(new ActiveChunkInfo(loadedMapPrefab, chunk));
                }
            }
        }
        else
        {
            Debug.LogError("[MapGenerator] 맵 프리팹을 찾을 수 없습니다: " + patternData.MapPrefabId);
        }
    }

    public void RecycleOldestChunk()
    {
        if (activeChunks.Count > 0)
        {
            ActiveChunkInfo oldChunkInfo = activeChunks.Dequeue();
            ObjectPoolingManager.Instance.ReturnToPool(oldChunkInfo.PrefabKey, oldChunkInfo.Chunk.gameObject);
        }
    }
}