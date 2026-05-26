using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : SingletonBase<MapGenerator>
{
    [Header("Map Settings")]
    [SerializeField] private GameObject mapChunkPrefab;

    // 🔥 인스펙터에서는 "Stage_001" 같은 스테이지 ID 하나만 입력받습니다.
    [SerializeField] private string currentStageId = "Stage_001";
    [SerializeField] private int initialChunkCount = 5;

    // 외부 Json에서 받아와 저장할 리스트 변수 (더 이상 [SerializeField]가 아님)
    private List<string> stageChunkIds = new List<string>();

    private float nextSpawnZ = 0f;
    private Queue<MapChunk> activeChunks = new Queue<MapChunk>();
    private int currentChunkIndex = 0;

    private void Start()
    {
        if (mapChunkPrefab == null)
        {
            Debug.LogError("[MapGenerator] 맵 청크 프리팹이 비어있습니다.");
            return;
        }

        // 1. 🔥 GameDataManager에서 현재 스테이지의 데이터를 Json에서 끌어옵니다.
        StageData stageData = GameDataManager.Instance.GetStageData(currentStageId);

        if (stageData != null && stageData.MapChunkIdList != null && stageData.MapChunkIdList.Count > 0)
        {
            // 성공적으로 로드되었다면 리스트 덮어씌우기
            stageChunkIds = stageData.MapChunkIdList;
        }
        else
        {
            Debug.LogError("[MapGenerator] 스테이지 데이터를 불러올 수 없거나 맵 청크 리스트가 비어있습니다: " + currentStageId);
            return;
        }

        // 2. 초기 맵 청크 생성 실행
        for (int i = 0; i < initialChunkCount; i++)
        {
            SpawnNextChunk();
        }
    }

    public void SpawnNextChunk()
    {
        string targetChunkId = stageChunkIds[currentChunkIndex];
        currentChunkIndex = (currentChunkIndex + 1) % stageChunkIds.Count;

        Vector3 spawnPosition = new Vector3(0f, 0f, nextSpawnZ);
        GameObject chunkObj = ObjectPoolingManager.Instance.SpawnFromPool(mapChunkPrefab, spawnPosition, Quaternion.identity);

        if (chunkObj != null)
        {
            MapChunk chunk = chunkObj.GetComponent<MapChunk>();
            if (chunk != null)
            {
                chunk.SetupChunkData(targetChunkId, nextSpawnZ);
                nextSpawnZ += chunk.ChunkLength;
                activeChunks.Enqueue(chunk);
            }
        }
    }

    public void RecycleOldestChunk()
    {
        if (activeChunks.Count > 0)
        {
            MapChunk oldChunk = activeChunks.Dequeue();
            ObjectPoolingManager.Instance.ReturnToPool(mapChunkPrefab, oldChunk.gameObject);
        }
    }
}