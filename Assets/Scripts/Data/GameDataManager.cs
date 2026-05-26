using System;
using System.Collections.Generic;
using UnityEngine;

// 게임 데이터 관리 매니저
public class GameDataManager : SingletonBase<GameDataManager>
{
    [Serializable]
    private class SerializationWrapper<T>
    {
        public List<T> items; 
    }

    public Dictionary<string, MapChunkData> MapChunkDataList { get; private set; } = new Dictionary<string, MapChunkData>();
    

    protected override void Awake()
    {
        base.Awake();
        LoadAllData();
    }

    public void LoadAllData()
    {
        MapChunkDataList = LoadData<MapChunkData>("MapTable");
    }

    private Dictionary<string, T> LoadData<T>(string tableName) where T : GameDataBase
    {
        string resourcePath = "JsonOutput/" + tableName;
        TextAsset textAsset = Resources.Load<TextAsset>(resourcePath);

        Dictionary<string, T> dataDictionary = new Dictionary<string, T>();

        if (textAsset == null)
        {
            Debug.LogError("[Error] 리소스를 찾을 수 없습니다: Resources/" + resourcePath);
            return dataDictionary;
        }

        try
        {
            string jsonString = textAsset.text;
            string wrappedJson = "{\"items\":" + jsonString + "}";
            SerializationWrapper<T> wrapper = JsonUtility.FromJson<SerializationWrapper<T>>(wrappedJson);

            if (wrapper != null && wrapper.items != null)
            {
                foreach (T item in wrapper.items)
                {
                    if (!dataDictionary.ContainsKey(item.Id))
                    {
                        dataDictionary.Add(item.Id, item);
                    }
                }
                
                Debug.Log(typeof(T).Name + " 데이터를 " + wrapper.items.Count + "개 로드했습니다.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("[" + typeof(T).Name + " JSON 로드 오류] " + ex.Message);
        }

        return dataDictionary;
    }


    public MapChunkData GetMapChunkData(string id)
    {
        if (MapChunkDataList == null || string.IsNullOrEmpty(id))
        {
            return null;
        }

        MapChunkData item;
        if (MapChunkDataList.TryGetValue(id, out item))
        {
            return item;
        }

        return null;
    }
}