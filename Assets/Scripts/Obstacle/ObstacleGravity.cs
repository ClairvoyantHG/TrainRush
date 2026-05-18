using UnityEngine;

// 이동형 장애물 컴포넌트
public class ObstacleGravity : MonoBehaviour, IGravityAffected
{
    [SerializeField] private float fallSpeed = 30f;         // 낙하 속도
    [SerializeField] private float rotationSpeed = 1000f;   // 회전 속도

    private GridPosition currentGridPosition;   // 현재 장애물 위치
    private bool isFalling = false;             // 낙하 여부

    private Vector3 targetPosition;     // 목표 이동 위치
    private Quaternion targetRotation;  // 목표 회전

    // 초기화
    public void Initialize(GridPosition startPosition)
    {
        currentGridPosition = startPosition;
        isFalling = false;

        // 초기 회전값 설정
        if (GridManager.Instance != null)
        {
            targetRotation = GridManager.Instance.GetWorldRotation(currentGridPosition.CurrentGravity);
            transform.rotation = targetRotation;
        }

        // 중력 매니저에 옵저버로 등록
        if (GravityManager.Instance != null)
        {
            //GravityManager.Instance.Register(this);

            GravityManager.Instance.BindEventOnGravity(OnGravityChanged);
        }
    }

    // 중력 변환 시 호출
    public void OnGravityChanged(GravityDirection newGravity)
    {
        if (GridManager.Instance != null)
        {
            // 중력 변환에 따른 목표 값 계산
            currentGridPosition = GridManager.Instance.GetFallenPosition(currentGridPosition, newGravity);
            targetRotation = GridManager.Instance.GetWorldRotation(newGravity);
            targetPosition = GridManager.Instance.GetWorldPosition(currentGridPosition);

            // z위치 고정
            targetPosition.z = transform.position.z;

            isFalling = true;
        }
    }

    private void Update()
    {
        // 낙하 상태인 경우 목표 위치와 회전으로 부드럽게 이동
        if (isFalling)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, fallSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 낙하 종료
            if (transform.position == targetPosition && transform.rotation == targetRotation)
            {
                isFalling = false;
            }
        }
    }

    public GridPosition GetCurrentGridPosition()
    {
        return currentGridPosition;
    }
}