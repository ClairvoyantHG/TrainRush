using System;
using UnityEngine;

// 플레이어의 중력 처리 컴포넌트
public class PlayerGravity : MonoBehaviour, IGravityAffected
{
    private PlayerMovement playerMovement;
    private PlayerJump playerJump;


    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerJump = GetComponent<PlayerJump>();
    }

    private void Start()
    {
        GravityManager.Instance.BindEventOnGravity(OnGravityChanged);
    }

    // 중력 변환 시 호출
    public void OnGravityChanged(GravityDirection newGravity)
    {
        if (playerMovement != null && GridManager.Instance != null && ObstacleManager.Instance != null)
        {
            // 플레이어의 현재 위치
            GridPosition currentActualPos = playerMovement.GetCurrentGridPosition();

            // 점프 중이면 점프 취소
            if (playerJump != null) playerJump.CancelJump();

            // 현재 위치에서 중력 방향만 변경한 임시 위치 생성
            GridPosition tempPos = currentActualPos;
            tempPos.CurrentGravity = newGravity;

            // 중력에 따라 이동할 바닥 계산 후 적용
            GridPosition newFloorPos = ObstacleManager.Instance.CalculateFloorPosition(tempPos, GridManager.Instance.gridHalfSize);
            playerMovement.SetCurrentGridPosition(newFloorPos);
        }
    }
}