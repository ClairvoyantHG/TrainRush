using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    HpMaterial,    
    SpeedMaterial  
}

public class ItemManager : SingletonBase<ItemManager>
{
    private List<ItemPickable> activeItems = new List<ItemPickable>();

    public void RegisterItem(ItemPickable item)
    {
        if (!activeItems.Contains(item)) activeItems.Add(item);
    }

    public void UnregisterItem(ItemPickable item)
    {
        if (activeItems.Contains(item)) activeItems.Remove(item);
    }

    public ItemPickable GetCollidedItem(GridPosition playerGrid, float playerZ, float playerRadius)
    {
        for (int i = 0; i < activeItems.Count; i++)
        {
            ItemPickable item = activeItems[i];

            if (item.CurrentGrid.X == playerGrid.X && item.CurrentGrid.Y == playerGrid.Y)
            {
                float halfLength = item.zLength * 0.5f;
                float minZ = item.CurrentZ - halfLength - playerRadius;
                float maxZ = item.CurrentZ + halfLength - 0.1f;

                if (playerZ >= minZ && playerZ <= maxZ)
                {
                    return item;
                }
            }
        }
        return null;
    }
}