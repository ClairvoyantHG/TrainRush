using UnityEngine;

// 카메라 추적 컴포넌트
public class CameraFollowZ : MonoBehaviour
{
    public Transform target;        // 추적 대상
    public float zOffset = -10f;    // 대상과의 거리

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 currentPosition = transform.position;

        currentPosition.z = target.position.z + zOffset;

        transform.position = currentPosition;
    }
}