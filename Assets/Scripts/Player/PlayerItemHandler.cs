using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float zHitRadius = 0.5f;
    [SerializeField] private float bonusSpeedAmount = 5.0f;

    [Header("References")]
    [SerializeField] private PlayerCollision playerCollision;
    [SerializeField] private PlayerMovement playerMovement;

    private bool isItemApplied = false;

    private void Awake()
    {
        if (playerCollision == null) playerCollision = GetComponent<PlayerCollision>();
        if (playerMovement == null) playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.CurrentState == GameState.Playing)
        {
            if (!isItemApplied)
            {
                ApplyEquippedItems();
                isItemApplied = true;
            }

            if (playerMovement == null) return;

            ItemPickable hitItem = ItemManager.Instance.GetCollidedItem(playerMovement.GetCurrentGridPosition(), transform.position.z, zHitRadius);

            if (hitItem != null && hitItem.CanPick)
            {
                if (InventoryManager.Instance != null)
                {
                    InventoryManager.Instance.AddItem(hitItem.CurrentItemType, 1);
                }

                hitItem.OnPickedUp();
            }
        }
        else
        {
            isItemApplied = false;
        }
    }

    private void ApplyEquippedItems()
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
}