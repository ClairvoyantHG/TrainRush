using UnityEngine;

public class ItemPickable : MonoBehaviour
{
    public float zLength = 1f;

    public ItemType CurrentItemType { get; private set; }

    private GridPosition currentGrid;
    private float currentZ;

    public GridPosition CurrentGrid { get { return currentGrid; } }
    public float CurrentZ { get { return currentZ; } }

    public void Initialize(ItemType type, GridPosition startPos, float zPos)
    {
        CurrentItemType = type;
        currentGrid = startPos;
        currentZ = zPos;

        if (GridManager.Instance != null)
        {
            transform.position = GridManager.Instance.GetWorldPosition(currentGrid);
        }
    }

    private void OnEnable()
    {
        if (ItemManager.Instance != null) ItemManager.Instance.RegisterItem(this);
    }

    private void OnDisable()
    {
        if (ItemManager.Instance != null) ItemManager.Instance.UnregisterItem(this);
    }

    public void OnPickedUp()
    {
        gameObject.SetActive(false);
    }
}