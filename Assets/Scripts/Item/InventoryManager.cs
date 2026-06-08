using UnityEngine;

public class InventoryManager : SingletonBase<InventoryManager>
{
    private static int currentHpMaterial = 2;
    private static int currentSpeedMaterial = 2;
    private static bool isHpUpgradeEquipped = false;
    private static bool isSpeedUpgradeEquipped = false;

    public int CurrentHpMaterial { get { return currentHpMaterial; } }
    public int CurrentSpeedMaterial { get { return currentSpeedMaterial; } }
    public bool IsHpUpgradeEquipped { get { return isHpUpgradeEquipped; } }
    public bool IsSpeedUpgradeEquipped { get { return isSpeedUpgradeEquipped; } }

    public void AddItem(ItemType itemType, int amount)
    {
        if(itemType == ItemType.HpMaterial)
        {
            currentHpMaterial += amount;
            Debug.Log("[InventoryManager]" + itemType.ToString() + " 재화 획득! 총량: " + currentHpMaterial);
        }
        else if(itemType == ItemType.SpeedMaterial)
        {
            currentSpeedMaterial += amount;
            Debug.Log("[InventoryManager]" + itemType.ToString() + " 재화 획득! 총량: " + currentSpeedMaterial);
        }
    }

    public bool UseHpMaterial(int amount)
    {
        if (currentHpMaterial >= amount)
        {
            currentHpMaterial -= amount;
            return true;
        }
        return false;
    }

    public bool UseSpeedMaterial(int amount)
    {
        if (currentSpeedMaterial >= amount)
        {
            currentSpeedMaterial -= amount;
            return true;
        }
        return false;
    }

    public void SetHpUpgrade(bool isEquipped)
    {
        isHpUpgradeEquipped = isEquipped;
    }

    public void SetSpeedUpgrade(bool isEquipped)
    {
        isSpeedUpgradeEquipped = isEquipped;
    }

    public void ConsumeEquippedUpgrades()
    {
        isHpUpgradeEquipped = false;
        isSpeedUpgradeEquipped = false;
        Debug.Log("[InventoryManager] 장착된 일회용 업그레이드가 이번 플레이를 위해 소모되었습니다.");
    }
}