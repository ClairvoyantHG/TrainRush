using System;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : SingletonBase<GameDataManager>
{
    [Serializable]
    private class SerializationWrapper<T>
    {
        public List<T> items;
    }

    public Dictionary<string, MapPatternData> MapPatternDataList { get; private set; } = new Dictionary<string, MapPatternData>();
    public Dictionary<string, StageData> StageDataList { get; private set; } = new Dictionary<string, StageData>();

    protected override void Awake()
    {
        base.Awake();
        LoadAllData();
    }

    public void LoadAllData()
    {
        MapPatternDataList = LoadData<MapPatternData>("MapPatternData");
        StageDataList = LoadData<StageData>("StageData");
    }

    private Dictionary<string, T> LoadData<T>(string tableName) where T : GameDataBase
    {
        string resourcePath = "JsonOutput/" + tableName;
        TextAsset textAsset = Resources.Load<TextAsset>(resourcePath);
        Dictionary<string, T> dataDictionary = new Dictionary<string, T>();

        if (textAsset == null) return dataDictionary;

        try
        {
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
        return dataDictionary;
    }

    public MapPatternData GetMapPatternData(string id)
    {
        if (MapPatternDataList == null || string.IsNullOrEmpty(id)) return null;
        MapPatternData item;
        if (MapPatternDataList.TryGetValue(id, out item)) return item;
        return null;
    }

    public StageData GetStageData(string id)
    {
        if (StageDataList == null || string.IsNullOrEmpty(id)) return null;
        StageData item;
        if (StageDataList.TryGetValue(id, out item)) return item;
        return null;
    }
}