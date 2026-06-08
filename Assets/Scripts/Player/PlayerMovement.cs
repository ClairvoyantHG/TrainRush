using UnityEngine;

// 플레이어 이동 컴포넌트
public class PlayerMovement : MonoBehaviour
{
    private float forwardSpeed = 5f;        // 이동 속도
    private float transitionSpeed = 30f;    // 칸 전환 속도
    private float rotationSpeed = 1000f;    // 회전 속도

    private GridPosition baseGridPosition;      // 플레이어의 좌표
    private PlayerJump playerJump;             

    private void Start()
    {
        playerJump = GetComponent<PlayerJump>();

        // 기본 위치 설정 
        baseGridPosition = new GridPosition(0, -1, GravityDirection.Down, 0f);

        if (GridManager.Instance != null)
        {
            transform.position = GridManager.Instance.GetWorldPosition(baseGridPosition);
            transform.rotation = GridManager.Instance.GetWorldRotation(baseGridPosition.CurrentGravity);
        }
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameState.Playing) return;

        // z축 자동 이동
        baseGridPosition.ZPosition += forwardSpeed * Time.deltaTime;

        // 낙하 체크
        if (ObstacleManager.Instance != null && playerJump != null && !playerJump.IsJumping())
        {
            baseGridPosition = ObstacleManager.Instance.CalculateFloorPosition(baseGridPosition, GridManager.Instance.gridHalfSize);
        }

        // 플레이어의 위치 반영
        ApplySmoothTransform();
    }

    // 부드러운 이동
    private void ApplySmoothTransform()
    {
        // 목표 좌표 설정
        GridPosition targetGrid = GetCurrentGridPosition();

        Vector3 targetPosition = GridManager.Instance.GetWorldPosition(targetGrid);
        Quaternion targetRotation = GridManager.Instance.GetWorldRotation(targetGrid.CurrentGravity);

        // 일정한 속도로 부드럽게 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, transitionSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // 점프 후 착지
    public void LandFromJump()
    {
        if (playerJump != null && GridManager.Instance != null)
        {
            // 점프한 오프셋을 바닥 좌표에 합산
            Vector2Int jumpOffset = playerJump.GetJumpGridOffset(baseGridPosition.CurrentGravity);
            baseGridPosition.X += jumpOffset.x;
            baseGridPosition.Y += jumpOffset.y;

            // 공중 칸에서부터 즉시 추락
            if (ObstacleManager.Instance != null)
            {
                baseGridPosition = ObstacleManager.Instance.CalculateFloorPosition(baseGridPosition, GridManager.Instance.gridHalfSize);
            }
        }
    }

    // 보너스 속도
    public void AddBonusSpeed(float bonusSpeed)
    {
        forwardSpeed += bonusSpeed; 

        Debug.Log("[PlayerMovement] 보너스 이동속도 적용! 속도가 " + bonusSpeed + "만큼 빨라졌습니다.");
    }

    // 외부에 플레이어의 위치를 반환
    public GridPosition GetCurrentGridPosition()
    {
        GridPosition actualPos = baseGridPosition;

        // 점프 중이라면 공중으로 이동한 칸을 반환
        if (playerJump != null)
        {
            Vector2Int jumpOffset = playerJump.GetJumpGridOffset(baseGridPosition.CurrentGravity);
            actualPos.X += jumpOffset.x;
            actualPos.Y += jumpOffset.y;
        }
        return actualPos;
    }

    public void SetCurrentGridPosition(GridPosition newPosition)
    {
        baseGridPosition = newPosition;
    }

    public float GetForwardSpeed()
    {
        return forwardSpeed;
    }
}