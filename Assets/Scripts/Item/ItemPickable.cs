using System;
using UnityEngine;

public class ItemPickable : MonoBehaviour, IPoolable
{
    public float zLength = 1f;

    [SerializeField] private ItemType currentItemType;
    private GridPosition currentGrid;
    private float currentZ;
    private bool canPick;
    public bool CanPick {  get { return canPick; } }

    public ItemType CurrentItemType { get { return currentItemType; } }
    public GridPosition CurrentGrid { get { return currentGrid; } }
    public float CurrentZ { get { return currentZ; } }

    public void Initialize(GridPosition startPos, float zPos)
    {
        currentGrid = startPos;
        currentZ = zPos;

        if (GridManager.Instance != null)
        {
            transform.position = GridManager.Instance.GetWorldPosition(currentGrid);
        }

        if (ItemManager.Instance != null) ItemManager.Instance.RegisterItem(this);
    }


    public void OnSpawn()
    {
        canPick = true;
    }

    public void OnDespawn()
    {
        if (ItemManager.Instance != null) ItemManager.Instance.UnregisterItem(this);
    }

    public void OnPickedUp()
    {
        canPick = false;
        gameObject.SetActive(false);
    }
}