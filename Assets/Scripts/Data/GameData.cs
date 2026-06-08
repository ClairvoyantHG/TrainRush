using System;
using System.Collections.Generic;

[Serializable]
public class GameDataBase
{
    public string Id;
}

[Serializable]
public class MapPatternData : GameDataBase
{
    public string MapPrefabId;
    public List<string> ObstaclePrefabList;
    public List<string> ItemPrefabList;
    public List<string> SpawnPointsList;
}

[Serializable]
public class StageData : GameDataBase
{
    public List<string> MapPatternIdList;
}