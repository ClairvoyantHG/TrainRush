using System.Collections;
using UnityEngine;

// 플레이어 충돌 컴포넌트
public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private float invincibilityDuration = 2f;
    [SerializeField] private float flickerInterval = 0.1f;

    [SerializeField] private GameObject characterModel;
    [SerializeField] private PlayerMovement playerMovement;

    private int maxHp = 2;
    private int currentHp;
    private bool isInvincible = false;
    private Renderer[] renderers;
    private float playerRadius = 0.5f;

    private void Awake()
    {
        currentHp = maxHp;
        if (characterModel != null)
        {
            renderers = characterModel.GetComponentsInChildren<Renderer>();
        }
    }

    private void Start()
    {
        // 체력 추가 아이템 적용
        if (InventoryManager.Instance != null && InventoryManager.Instance.IsHpUpgradeEquipped)
        {
            maxHp += 1; 
            Debug.Log("[PlayerCollision] 아이템 효과 적용! 최대 체력이 " + maxHp + "이 되었습니다.");
        }

        currentHp = maxHp;
    }

    private void Update()
    {
        if (isInvincible || playerMovement == null) return;

        float currentZ = transform.position.z;

        // 장애물 충돌 검사
        ObstacleBase hitObstacle = ObstacleManager.Instance.GetCollidedObstacle(playerMovement.GetCurrentGridPosition(), currentZ, playerRadius);

        if (hitObstacle != null)
        {
            TakeDamage(hitObstacle.DamageType);
            return;
        }

        // 아이템 획득 검사
        ItemPickable hitItem = ItemManager.Instance.GetCollidedItem(playerMovement.GetCurrentGridPosition(), currentZ, playerRadius);

        if (hitItem != null)
        {
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.AddItem(1);
            }

            // 아이템 객체 비활성화
            hitItem.OnPickedUp();
        }
    }

    // 피격
    private void TakeDamage(ObstacleDamageType type)
    {
        // 즉사 체크
        if (type == ObstacleDamageType.Fatal) currentHp = 0;
        else currentHp--;

        if (currentHp <= 0) 
        {
            TriggerGameOver();
        }
        else 
        {
            TriggerWarningState(currentHp);
        }
    }

    // 충돌 경고
    private void TriggerWarningState(int currentHp)
    {
        StartCoroutine(InvincibilityRoutine());

        UIManager.Instance.OpenUI(UIRootType.Main, UIType.ChaserWarningUI);

        ChaserWarningUI warningUI = UIManager.Instance.GetUI<ChaserWarningUI>(UIType.ChaserWarningUI);
        if (warningUI != null)
        {
            warningUI.SetWarningHp(currentHp);
        }
    }

    // 피격 후 무적시간
    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        float elapsedTime = 0f;
        bool isVisible = true;

        WaitForSeconds waitFlicker = YieldInstructionCache.WaitForSeconds(flickerInterval);

        while (elapsedTime < invincibilityDuration)
        {
            isVisible = !isVisible;
            SetRenderersVisibility(isVisible);

            yield return waitFlicker;

            elapsedTime += flickerInterval;
        }

        SetRenderersVisibility(true);
        isInvincible = false;
    }

    // 렌더러 정돈
    private void SetRenderersVisibility(bool isVisible)
    {
        if (renderers == null) return;

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = isVisible;
        }
    }

    // 게임 오버
    private void TriggerGameOver()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }
}