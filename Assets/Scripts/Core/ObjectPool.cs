using System.Collections.Generic;
using UnityEngine;

// 오브젝트 풀링 매니저
public class ObjectPool : SingletonBase<ObjectPool>
{
    // 프리팹 Key, 해당 프리팹의 대기 객체들 Value
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

    // 풀에서 객체를 꺼내 배치
    public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        // 처음 생성되는 프리팹이라면 큐 생성
        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary.Add(prefab, new Queue<GameObject>());
        }

        GameObject objectToSpawn = null;

        // 대기 중인 객체가 있으면 꺼내고 없으면 새로 생성
        if (poolDictionary[prefab].Count > 0)
        {
            objectToSpawn = poolDictionary[prefab].Dequeue();
        }
        else
        {
            objectToSpawn = Instantiate(prefab);
        }

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // 객체 상태 초기화
        IPoolable poolable = objectToSpawn.GetComponent<IPoolable>();
        if (poolable != null)
        {
            poolable.OnSpawn();
        }

        return objectToSpawn;
    }

    // 사용이 끝난 객체를 풀의 대기열로 반환
    public void ReturnToPool(GameObject prefabKey, GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);

        // 객체 상태 초기화
        IPoolable poolable = objectToReturn.GetComponent<IPoolable>();
        if (poolable != null)
        {
            poolable.OnDespawn();
        }

        poolDictionary[prefabKey].Enqueue(objectToReturn);
    }
}