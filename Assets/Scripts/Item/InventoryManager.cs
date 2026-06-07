using UnityEngine;

public class InventoryManager : SingletonBase<InventoryManager>
{
    public int CurrentItemCount { get; private set; } = 0;

    public bool IsHpUpgradeEquipped { get; private set; } = false;

    public void AddItem(int amount)
    {
        CurrentItemCount += amount;
        Debug.Log("[InventoryManager] 아이템 획득 현재 수량: " + CurrentItemCount);
    }

    public bool UseItem(int amount)
    {
        if (CurrentItemCount >= amount)
        {
            CurrentItemCount -= amount;
            return true; 
        }
        return false; 
    }

    public void SetHpUpgrade(bool isEquipped)
    {
        IsHpUpgradeEquipped = isEquipped;
        Debug.Log("[InventoryManager] 체력 업그레이드 장착 상태: " + IsHpUpgradeEquipped);
    }
}