using UnityEngine;

// 플레이어 점프 컴포넌트
public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpZDistance = 5f;    // 점프 지속 거리

    private bool isJumping = false;             // 점프 중 확인
    private float remainingJumpDistance = 0f;   // 남은 점프 거리

    // 점프 입력 처리 및 남은 거리 계산
    public void CalculateJump(bool jumpInput, float currentForwardSpeed, GridPosition currentPos)
    {
        // 바닥에 있을 때 점프 시도
        if (!isJumping && jumpInput)
        {
            // 머리 위 계산
            Vector2Int potentialOffset = Vector2Int.zero;
            switch (currentPos.CurrentGravity)
            {
                case GravityDirection.Down: 
                    potentialOffset = new Vector2Int(0, 1); 
                    break;

                case GravityDirection.Up: 
                    potentialOffset = new Vector2Int(0, -1); 
                    break;

                case GravityDirection.Left: 
                    potentialOffset = new Vector2Int(1, 0);
                    break;

                case GravityDirection.Right: 
                    potentialOffset = new Vector2Int(-1, 0);
                    break;

                default:
                    break;
            }



            int targetX = currentPos.X + potentialOffset.x;
            int targetY = currentPos.Y + potentialOffset.y;

            int maxHalfSize = GridManager.Instance.gridHalfSize;

            // 칸을 벗어나는지 확인
            if (Mathf.Abs(targetX) <= maxHalfSize && Mathf.Abs(targetY) <= maxHalfSize)
            {
                bool canJump = true;

                // 목표 칸에 발판 존재 확인
                if (ObstacleManager.Instance != null)
                {
                    ObstacleBase obstacleAbove = ObstacleManager.Instance.GetObstacleAt(targetX, targetY, currentPos.ZPosition);
                    if (obstacleAbove != null)
                    {
                        canJump = false;
                    }
                }

                // 점프 실행
                if (canJump)
                {
                    isJumping = true;
                    remainingJumpDistance = jumpZDistance;
                }
            }
        }

        // 공중에 있을 때 전진 거리에 따라 체공 시간 차감
        if (isJumping)
        {
            remainingJumpDistance -= currentForwardSpeed * Time.deltaTime;

            // 체공 거리가 끝났을 때 착지 처리
            if (remainingJumpDistance <= 0f)
            {
                PlayerMovement pm = GetComponent<PlayerMovement>();
                if (pm != null) pm.LandFromJump();

                CancelJump();
            }
        }
    }

    // 점프 방향에 따른 이동 위치
    public Vector2Int GetJumpGridOffset(GravityDirection currentGravity)
    {
        if (!isJumping) return Vector2Int.zero;

        switch (currentGravity)
        {
            case GravityDirection.Down: 
                return new Vector2Int(0, 1);

            case GravityDirection.Up:
                return new Vector2Int(0, -1);

            case GravityDirection.Left: 
                return new Vector2Int(1, 0);

            case GravityDirection.Right: 
                return new Vector2Int(-1, 0);

            default: 
                return Vector2Int.zero;
        }
    }

    // 중력 전환이나 착지 시 점프 상태 초기화
    public void CancelJump()
    {
        isJumping = false;
        remainingJumpDistance = 0f;
    }

    public bool IsJumping()
    {
        return isJumping;
    }
}