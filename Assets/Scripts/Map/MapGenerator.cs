using System.Collections.Generic;
using UnityEngine;

// 맵 생성, 관리용
public class MapGenerator : SingletonBase<MapGenerator>
{
    // 스테이지 시스템 아직 미완성
    [SerializeField] private string currentStageId = "Stage_001";
    [SerializeField] private int initialChunkCount = 5;

    // 맵 수거 기준점용
    private Transform cameraTransform;

    // 오브젝트 풀링 반환용 맵 정보
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
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        // 데이터 로드
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

        // 초기 맵 생성
        for (int i = 0; i < initialChunkCount; i++)
        {
            SpawnNextChunk();
        }
    }

    private void Update()
    {
        if (cameraTransform == null || activeChunks.Count == 0) return;

        // 가장 오래된 맵의 z좌표를 확인하여 맵 생성 삭제 반복
        ActiveChunkInfo oldestChunk = activeChunks.Peek();

        float chunkEndZ = oldestChunk.Chunk.transform.position.z + oldestChunk.Chunk.ChunkLength;

        if (cameraTransform.position.z > chunkEndZ + 10f)
        {
            RecycleOldestChunk();

            SpawnNextChunk();
        }
    }

    // 다음 맵 생성
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
    }

    // 
    public void RecycleOldestChunk()
    {
        if (activeChunks.Count > 0)
        {
            ActiveChunkInfo oldChunkInfo = activeChunks.Dequeue();
            ObjectPoolingManager.Instance.ReturnToPool(oldChunkInfo.PrefabKey, oldChunkInfo.Chunk.gameObject);
        }
    }
}