using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : SingletonBase<MapGenerator>
{
    [Header("Stage Settings")]
    [SerializeField] private string currentStageId = "Stage_001";
    [SerializeField] private int initialChunkCount = 5;

    // 수거할 때 어떤 프리팹 키(원본)로 생성했는지 알아야 하므로 구조체로 묶어 큐에 보관합니다.
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
        // 1. 스테이지 데이터 로드
        StageData stageData = GameDataManager.Instance.GetStageData(currentStageId);
        if (stageData != null && stageData.MapPatternIdList != null && stageData.MapPatternIdList.Count > 0)
        {
            stageMapPatternIds = stageData.MapPatternIdList;
        }
        else
        {
            Debug.LogError("[MapGenerator] 스테이지 데이터를 불러올 수 없습니다: " + currentStageId);
            return;
        }

        // 2. 초기 맵 청크 생성
        for (int i = 0; i < initialChunkCount; i++)
        {
            SpawnNextChunk();
        }
    }

    public void SpawnNextChunk()
    {
        // 1. 현재 순서의 MapTable ID를 가져옴
        string targetTableId = stageMapPatternIds[currentChunkIndex];
        currentChunkIndex = (currentChunkIndex + 1) % stageMapPatternIds.Count;

        // 2. MapTable 데이터 로드
        MapPatternData tableData = GameDataManager.Instance.GetMapPatternData(targetTableId);
        if (tableData == null) return;

        // 3. 해당 데이터가 요구하는 맵(바닥) 프리팹을 찾아 로드
        GameObject loadedMapPrefab = Resources.Load<GameObject>("Prefabs/MapChunks/" + tableData.MapPrefabId);
        if (loadedMapPrefab != null)
        {
            // 4. 오브젝트 풀을 통해 맵 스폰
            Vector3 spawnPosition = new Vector3(0f, 0f, nextSpawnZ);
            GameObject chunkObj = ObjectPoolingManager.Instance.SpawnFromPool(loadedMapPrefab, spawnPosition, Quaternion.identity);

            if (chunkObj != null)
            {
                MapChunk chunk = chunkObj.GetComponent<MapChunk>();
                if (chunk != null)
                {
                    // 5. 생성된 맵에 MapTable 데이터 주입 (내부에서 장애물이 랜덤 생성됨)
                    chunk.SetupChunkData(tableData, nextSpawnZ);

                    // 6. 다음 맵이 생성될 Z좌표 갱신 (프리팹별로 길이가 달라도 자동 대응)
                    nextSpawnZ += chunk.ChunkLength;

                    // 7. 수거(Recycle)를 위해 프리팹 원본과 청크 객체를 함께 큐에 저장
                    activeChunks.Enqueue(new ActiveChunkInfo(loadedMapPrefab, chunk));
                }
            }
        }
        else
        {
            Debug.LogError("[MapGenerator] 맵 프리팹을 찾을 수 없습니다: " + tableData.MapPrefabId);
        }
    }

    public void RecycleOldestChunk()
    {
        if (activeChunks.Count > 0)
        {
            ActiveChunkInfo oldChunkInfo = activeChunks.Dequeue();
            // 정확한 프리팹 키값을 넘겨주어 ObjectPoolingManager가 올바른 큐에 회수하도록 처리
            ObjectPoolingManager.Instance.ReturnToPool(oldChunkInfo.PrefabKey, oldChunkInfo.Chunk.gameObject);
        }
    }
}