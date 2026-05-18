using System.Collections.Generic;
using UnityEngine;

// 맵 생성 관리 매니저
public class MapGenerator : SingletonBase<MapGenerator>
{
    [SerializeField] private List<GameObject> chunkPrefabs = new List<GameObject>();  // 생성할 맵 프리팹

    [SerializeField] private Transform playerTransform;         // 플레이어
    [SerializeField] private int chunksOnScreen = 5;            // 화면에 유지되는 맵의 개수
    [SerializeField] private float despawnDistance = 30f;       // 맵을 제거할 거리
    [SerializeField] private float spawnZ = 0f;                 // 다음 맵이 이어 붙여질 Z 좌표

    // 현재 맵에 배치된 청크의 정보
    private struct ActiveChunk
    {
        public GameObject Instance;
        public GameObject PrefabKey;
    }

    private List<ActiveChunk> activeChunks = new List<ActiveChunk>();   // 맵을 관리할 리스트

    private void Start()
    {
        if (chunkPrefabs.Count == 0 || playerTransform == null)
        {
            Debug.LogError("MapGenerator 누락 확인");
            return;
        }

        // 게임 시작 시 맵 배치
        for (int i = 0; i < chunksOnScreen; i++)
        {
            SpawnNextChunk();
        }
    }

    private void Update()
    {
        if (activeChunks.Count == 0) return;

        // 가장 오래된 맵의 Z 위치 확인
        float firstChunkZ = activeChunks[0].Instance.transform.position.z;

        // 맵 제거 거리 확인
        if (playerTransform.position.z - firstChunkZ > despawnDistance)
        {
            // 맵 제거 후 새 맵 생성
            DespawnFirstChunk(); 
            SpawnNextChunk();    
        }
    }

    // 전방에 새 맵 생성
    private void SpawnNextChunk()
    {
        // 랜덤한 맵 프리팹 선택
        int randomIndex = Random.Range(0, chunkPrefabs.Count);
        GameObject selectedPrefab = chunkPrefabs[randomIndex];

        // 오브젝트 풀을 통해 맵 생성
        GameObject newChunk = ObjectPool.Instance.SpawnFromPool(selectedPrefab, new Vector3(0f, 0f, spawnZ), Quaternion.identity);

        ActiveChunk newActiveChunk = new ActiveChunk();
        newActiveChunk.Instance = newChunk;
        newActiveChunk.PrefabKey = selectedPrefab;

        // 관리 리스트에 추가
        activeChunks.Add(newActiveChunk);

        // 생성된 맵의 길이만큼 다음 스폰 위치를 전진
        MapChunk chunkComponent = newChunk.GetComponent<MapChunk>();
        if (chunkComponent != null)
        {
            spawnZ += chunkComponent.GetChunkLength();
        }
    }

    // 가장 뒤에 있는 맵을 제거
    private void DespawnFirstChunk()
    {
        // 리스트의 첫 번째 맵을 꺼냄
        ActiveChunk chunkToRemove = activeChunks[0];
        activeChunks.RemoveAt(0);

        // 오브젝트 풀로 반환
        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.ReturnToPool(chunkToRemove.PrefabKey, chunkToRemove.Instance);
        }
    }
}