using System.Collections;
using UnityEngine;

// 플레이어 충돌 컴포넌트
public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private float invincibilityDuration = 2f;
    [SerializeField] private float flickerInterval = 0.1f;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject characterModel;

    private int maxHp = 2;
    private int currentHp;
    private bool isInvincible = false;
    private SpriteRenderer[] renderers;
    private float playerRadius = 0.5f;

    private void Awake()
    {
        currentHp = maxHp;
        if (characterModel != null)
        {
            renderers = characterModel.GetComponentsInChildren<SpriteRenderer>();
        }
    }

    private void Update()
    {
        if (isInvincible || playerMovement == null) return;

        float currentZ = transform.position.z;

        // 장애물 충돌 확인
        ObstacleBase hitObstacle = ObstacleManager.Instance.GetCollidedObstacle(playerMovement.GetCurrentGridPosition(), currentZ, playerRadius);

        if (hitObstacle != null)
        {
            TakeDamage(hitObstacle.DamageType);
            return;
        }

    }

    // 체력 보너스
    public void AddBonusHp(int bonus)
    {
        maxHp += bonus;
        currentHp += bonus;
        Debug.Log("[PlayerCollision] 보너스 체력 적용! 현재 최대 체력: " + maxHp);
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
        if (renderers == null)
        {
            Debug.Log("플레이어 랜더러 확인");
            return;
        }

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