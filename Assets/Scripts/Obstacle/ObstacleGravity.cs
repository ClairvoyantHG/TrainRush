using UnityEngine;

// 이동형 장애물 컴포넌트
public class ObstacleGravity : MonoBehaviour, IGravityAffected
{
    [SerializeField] private float fallSpeed = 30f;             // 낙하 속도
    [SerializeField] private float rotationSpeed = 1000f;       // 회전 속도
    [SerializeField] private bool rotateToGravity = true;       // 낙하 시 회전 여부
    //[SerializeField] private float activationDistance = 40f;  // 중력 적용 범위

    private GridPosition currentGridPosition;   // 현재 장애물 위치
    private bool isFalling = false;             // 낙하 여부
    private bool isGravityActivated = false;    // 중력 활성화 여부

    private Vector3 targetPosition;     // 목표 이동 위치
    private Quaternion targetRotation;  // 목표 회전

    private Transform playerTransform;  // 중력 거리 기준점

    // 초기화
    public void Initialize(GridPosition startPosition)
    {
        currentGridPosition = startPosition;
        isFalling = false;
        isGravityActivated = false;

        if (PlayerController.Instance != null)
        {
            playerTransform = PlayerController.Instance.transform;
        }

        targetRotation = transform.rotation;

        if (GravityManager.Instance != null)
        {
            GravityManager.Instance.BindEventOnGravity(OnGravityChanged);
        }
    }

    private void OnDisable()
    {
        if (GravityManager.Instance != null)
        {
            GravityManager.Instance.UnbindEventOnGravity(OnGravityChanged);
        }
    }

    // 중력 변환 시 호출
    public void OnGravityChanged(GravityDirection newGravity)
    {
        if (!isGravityActivated) return;

        TriggerFall(newGravity);
    }

    private void Update()
    {
        // 중력 활성화 이전 거리 계산
        if (!isGravityActivated && playerTransform != null && GravityManager.Instance != null)
        {
            if (transform.position.z - playerTransform.position.z <= GravityManager.Instance.ActivationDistance)
            {
                isGravityActivated = true;

                TriggerFall(GravityManager.Instance.CurrentGravity);
            }
        }

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

    // 낙하 시작
    private void TriggerFall(GravityDirection newGravity)
    {
        if (GridManager.Instance != null)
        {
            // 중력 변환에 따른 목표 값 계산
            currentGridPosition = GridManager.Instance.GetFallenPosition(currentGridPosition, newGravity);
            targetPosition = GridManager.Instance.GetWorldPosition(currentGridPosition);

            if (rotateToGravity)
            {
                targetRotation = GridManager.Instance.GetWorldRotation(newGravity);
            }
            else
            {
                targetRotation = transform.rotation;
            }

            // z위치 고정
            targetPosition.z = transform.position.z;

            isFalling = true;
        }
    }

    public GridPosition GetCurrentGridPosition()
    {
        return currentGridPosition;
    }
}