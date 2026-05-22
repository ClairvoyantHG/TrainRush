using UnityEngine;

// 중력 방향에 따라 카메라의 XY 오프셋과 회전을 동시에 조절하는 컴포넌트
public class CameraGravityMovement : MonoBehaviour
{
    [Header("Smooth Speeds")]
    public float smoothSpeedXY = 5f;
    public float smoothSpeedRotation = 5f;

    [Header("Gravity XY Offsets")]
    public Vector2 downOffset = new Vector2(0f, 2f);
    public Vector2 upOffset = new Vector2(0f, -2f);
    public Vector2 leftOffset = new Vector2(2f, 0f);
    public Vector2 rightOffset = new Vector2(-2f, 0f);

    [Header("Gravity Rotations")]
    public Vector3 downRotation = new Vector3(15f, 0f, 0f);
    public Vector3 upRotation = new Vector3(-15f, 0f, 0f);
    public Vector3 leftRotation = new Vector3(0f, -15f, 0f);
    public Vector3 rightRotation = new Vector3(0f, 15f, 0f);

    private Vector2 currentTargetXY;
    private Quaternion currentTargetRotation;

    private void Start()
    {
        if (GravityManager.Instance != null)
        {
            UpdateTargetValues(GravityManager.Instance.CurrentGravity);
            GravityManager.Instance.BindEventOnGravity(OnGravityChanged);
        }
        else
        {
            currentTargetXY = downOffset;
            currentTargetRotation = Quaternion.Euler(downRotation);
        }
    }

    private void OnDestroy()
    {
        if (GravityManager.Instance != null)
        {
            GravityManager.Instance.UnbindEventOnGravity(OnGravityChanged);
        }
    }

    private void OnGravityChanged(GravityDirection newGravity)
    {
        UpdateTargetValues(newGravity);
    }

    // 중력 방향에 맞춰 목표 이동 위치와 목표 회전값을 동시에 갱신
    private void UpdateTargetValues(GravityDirection gravity)
    {
        switch (gravity)
        {
            case GravityDirection.Down:
                currentTargetXY = downOffset;
                currentTargetRotation = Quaternion.Euler(downRotation);
                break;
            case GravityDirection.Up:
                currentTargetXY = upOffset;
                currentTargetRotation = Quaternion.Euler(upRotation);
                break;
            case GravityDirection.Left:
                currentTargetXY = leftOffset;
                currentTargetRotation = Quaternion.Euler(leftRotation);
                break;
            case GravityDirection.Right:
                currentTargetXY = rightOffset;
                currentTargetRotation = Quaternion.Euler(rightRotation);
                break;
        }
    }

    private void LateUpdate()
    {
        // XY 위치 보간
        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Lerp(currentPos.x, currentTargetXY.x, smoothSpeedXY * Time.deltaTime);
        currentPos.y = Mathf.Lerp(currentPos.y, currentTargetXY.y, smoothSpeedXY * Time.deltaTime);
        transform.position = currentPos;

        // 회전 구면 선형 보간 
        transform.rotation = Quaternion.Slerp(transform.rotation, currentTargetRotation, smoothSpeedRotation * Time.deltaTime);
    }
}