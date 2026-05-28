using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float invincibilityDuration = 2f;
    [SerializeField] private float flickerInterval = 0.1f;

    [SerializeField] private float zHitRadius = 0.5f;

    [Header("References")]
    [SerializeField] private GameObject characterModel;
    [SerializeField] private Animator animator;

    [SerializeField] private PlayerMovement playerMovement;

    private int maxHp = 2;
    private int currentHp;
    private bool isInvincible = false;
    private Renderer[] renderers;

    private void Awake()
    {
        currentHp = maxHp;
        if (characterModel != null)
        {
            renderers = characterModel.GetComponentsInChildren<Renderer>();
        }
    }

    private void Update()
    {
        if (isInvincible || playerMovement == null) return;

        ObstacleBase hitObstacle = ObstacleManager.Instance.GetCollidedObstacle(
            playerMovement.GetCurrentGridPosition(),
            transform.position.z,
            zHitRadius
        );

        if (hitObstacle != null)
        {
            TakeDamage(hitObstacle.DamageType);
        }
    }

    private void TakeDamage(ObstacleDamageType type)
    {
        if (type == ObstacleDamageType.Fatal) currentHp = 0;
        else currentHp--;

        if (currentHp <= 0) TriggerGameOver();
        else TriggerWarningState();
    }

    private void TriggerWarningState()
    {
        if (animator != null) animator.SetTrigger("Hit");
        InGameUIManager.Instance.ShowChaserWarning();
        StartCoroutine(InvincibilityRoutine());
    }

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

    private void SetRenderersVisibility(bool isVisible)
    {
        if (renderers == null) return;
        for (int i = 0; i < renderers.Length; i++) renderers[i].enabled = isVisible;
    }

    private void TriggerGameOver()
    {
        Time.timeScale = 0f;
        Debug.Log("[PlayerCollision] 게임 오버!");
    }
}