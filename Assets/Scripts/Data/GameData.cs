using System.Collections.Generic;

[System.Serializable]
public class GameDataBase
{
    public string Id;
}

[System.Serializable]
public class ObstacleSpawnDataJson
{
    public string prefabId;     
    public int gridX;          
    public int gridY;          
    public float localZOffset; 
}

[System.Serializable]
public class MapChunkData : GameDataBase
{
    public float chunkLength;
    public List<ObstacleSpawnDataJson> spawnPoints = new List<ObstacleSpawnDataJson>();
}