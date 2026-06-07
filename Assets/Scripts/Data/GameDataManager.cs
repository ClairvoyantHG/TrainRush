using System;
using System.Collections.Generic;
using UnityEngine;

// 게임 내의 데이터를 로드하여 관리하는 매니저
public class GameDataManager : SingletonBase<GameDataManager>
{

    // 리스트 파싱 우회를 위한 래퍼 클래스
    [Serializable]
    private class SerializationWrapper<T>
    {
        public List<T> items;
    }

    // 모든 데이터를 타입별로 관리하는 딕셔너리
    private Dictionary<Type, object> allDataDictionaries = new Dictionary<Type, object>();

    protected override void Awake()
    {
        base.Awake();
        LoadAllData();
    }

    // 모든 데이터 로드
    public void LoadAllData()
    {
        LoadAndCacheData<MapPatternData>("MapPatternData");
        LoadAndCacheData<StageData>("StageData");
    }

    // 로드한 데이터를 딕셔너리로 변환한 뒤 마스터 딕셔너리에 저장
    private void LoadAndCacheData<T>(string tableName) where T : GameDataBase
    {
        string resourcePath = "JsonOutput/" + tableName;
        TextAsset textAsset = Resources.Load<TextAsset>(resourcePath);
        Dictionary<string, T> dataDictionary = new Dictionary<string, T>();

        if (textAsset != null)
        {
            try
            {
                // JsonUtility가 최상위 배열을 읽을 수 있도록 중괄호 추가
                string jsonString = textAsset.text;
                string wrappedJson = "{\"items\":" + jsonString + "}";
                SerializationWrapper<T> wrapper = JsonUtility.FromJson<SerializationWrapper<T>>(wrappedJson);

                if (wrapper != null && wrapper.items != null)
                {
                    foreach (T item in wrapper.items)
                    {
                        if (!dataDictionary.ContainsKey(item.Id)) dataDictionary.Add(item.Id, item);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("[" + typeof(T).Name + " 로드 오류] " + ex.Message);
            }
        }

        // 변환이 완료된 데이터 딕셔너리를 마스터 딕셔너리에 등록
        if (!allDataDictionaries.ContainsKey(typeof(T)))
        {
            allDataDictionaries.Add(typeof(T), dataDictionary);
        }
    }

    // 데이터 타입과 ID로 데이터 반환
    public T GetData<T>(string id) where T : GameDataBase
    {
        if (string.IsNullOrEmpty(id)) return null;

        object dictObj;

        if (allDataDictionaries.TryGetValue(typeof(T), out dictObj))
        {
            Dictionary<string, T> dict = dictObj as Dictionary<string, T>;

            if (dict != null)
            {
                T item;

                if (dict.TryGetValue(id, out item))
                {
                    return item;
                }
            }
        }

        return null;
    }
}