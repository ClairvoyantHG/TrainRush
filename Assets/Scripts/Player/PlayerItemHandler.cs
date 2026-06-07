using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float zHitRadius = 0.5f; // 아이템 획득 판정 범위
    [SerializeField] private float bonusSpeedAmount = 2.0f; // 추가될 이동속도 수치

    [Header("References")]
    [SerializeField] private PlayerCollision playerCollision;
    [SerializeField] private PlayerMovement playerMovement;

    private void Start()
    {
        // 1. 매니저들이 모두 깨어난 직후, 장착된 업그레이드 아이템 효과를 적용합니다.
        if (InventoryManager.Instance != null)
        {
            // 체력 업그레이드 적용
            if (InventoryManager.Instance.IsHpUpgradeEquipped && playerCollision != null)
            {
                playerCollision.AddBonusHp(1);
            }

            // 이동속도 업그레이드 적용
            if (InventoryManager.Instance.IsSpeedUpgradeEquipped && playerMovement != null)
            {
                playerMovement.AddBonusSpeed(bonusSpeedAmount);
            }
        }
    }

    private void Update()
    {
        // 게임 진행 중이 아니거나 이동 스크립트가 없으면 아이템 획득 검사 생략
        if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameState.Playing) return;
        if (playerMovement == null) return;

        // 2. 매 프레임 아이템 획득 여부를 직접 계산하여 판정
        ItemPickable hitItem = ItemManager.Instance.GetCollidedItem(
            playerMovement.GetCurrentGridPosition(),
            transform.position.z,
            zHitRadius
        );

        if (hitItem != null)
        {
            // 인벤토리에 수집품 추가 후 맵에서 비활성화
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.AddItem(1);
            }
            hitItem.OnPickedUp();
        }
    }
}