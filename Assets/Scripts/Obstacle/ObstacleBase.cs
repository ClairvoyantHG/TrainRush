using UnityEngine;

// 장애물 데미지 타입
public enum ObstacleDamageType
{
    Normal,     // 1회 피격 시 경고 상태로 전환
    Fatal       // 충돌 즉시 게임 오버
}

// 장애물의 기본 데이터와 생명주기 관리 컴포넌트
public class ObstacleBase : MonoBehaviour, IPoolable
{
    [SerializeField] private ObstacleDamageType damageType = ObstacleDamageType.Normal;     // 데미지 타입

    public float zLength = 1f;                  // 장애물의 길이
    public bool alignRotationToSurface = true;  // 회전 필요 여부

    private GridPosition currentGridPosition;
    private ObstacleGravity obstacleGravity;
    public ObstacleDamageType DamageType { get { return damageType; } }

    private void Awake()
    {
        // 이동형 장애물인지 확인
        obstacleGravity = GetComponent<ObstacleGravity>();
    }

    // MapChunk에 의해 소환될 때 상태 설정
    public void Initialize(GridPosition startPosition, float zPosition)
    {
        startPosition.ZPosition = zPosition;
        currentGridPosition = startPosition;

        // 실제 유니티 월드 위치 및 회전 적용
        if (GridManager.Instance != null)
        {
            transform.position = GridManager.Instance.GetWorldPosition(currentGridPosition);

            if (alignRotationToSurface)
            {
                transform.rotation = GridManager.Instance.GetWorldRotation(currentGridPosition.CurrentGravity);
            }
        }

        // 중력 컴포넌트 초기화
        if (obstacleGravity != null) obstacleGravity.Initialize(currentGridPosition);

        // 장애물 매니저에 자신을 등록
        if (ObstacleManager.Instance != null) ObstacleManager.Instance.Register(this);
    }

    public void OnSpawn() { }

    // 맵이 지나가서 오브젝트 풀로 돌아갈 때 등록 해제
    public void OnDespawn()
    {
        if (obstacleGravity != null && GravityManager.Instance != null)
            GravityManager.Instance.UnbindEventOnGravity(obstacleGravity.OnGravityChanged);

        if (ObstacleManager.Instance != null) ObstacleManager.Instance.Unregister(this);
    }

    // 외부에 현재 위치 반환
    public GridPosition GetCurrentGridPosition()
    {
        if (obstacleGravity != null) return obstacleGravity.GetCurrentGridPosition();
        return currentGridPosition;
    }
}