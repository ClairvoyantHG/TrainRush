using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float zHitRadius = 0.5f;
    [SerializeField] private float bonusSpeedAmount = 2.0f;

    [Header("References")]
    [SerializeField] private PlayerCollision playerCollision;
    [SerializeField] private PlayerMovement playerMovement;

    private void Start()
    {
        if (InventoryManager.Instance != null)
        {
            if (InventoryManager.Instance.IsHpUpgradeEquipped && playerCollision != null)
            {
                playerCollision.AddBonusHp(1);
            }

            if (InventoryManager.Instance.IsSpeedUpgradeEquipped && playerMovement != null)
            {
                playerMovement.AddBonusSpeed(bonusSpeedAmount);
            }

            InventoryManager.Instance.ConsumeEquippedUpgrades();
        }
    }

    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameState.Playing) return;
        if (playerMovement == null) return;

        ItemPickable hitItem = ItemManager.Instance.GetCollidedItem(
            playerMovement.GetCurrentGridPosition(), transform.position.z, zHitRadius);

        if (hitItem != null)
        {
            if (InventoryManager.Instance != null)
            {
                if (hitItem.CurrentItemType == ItemType.HpMaterial)
                {
                    InventoryManager.Instance.AddHpMaterial(1);
                }
                else if (hitItem.CurrentItemType == ItemType.SpeedMaterial)
                {
                    InventoryManager.Instance.AddSpeedMaterial(1);
                }
            }
            hitItem.OnPickedUp();
        }
    }
}