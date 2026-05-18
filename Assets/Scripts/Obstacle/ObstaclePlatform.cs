using System.Collections.Generic;
using UnityEngine;

// 장애물의 발판 여부
public enum PlatformSteppableType
{
    AnyDirection,       // 방향 상관없이
    SpecificDirection,  // 지정된 방향
    NotSteppable        // 밟을 수 없음
}

// 장애물 발판 기능 부여 컴포넌트
public class ObstaclePlatform : MonoBehaviour
{
    [SerializeField] private PlatformSteppableType steppableType = PlatformSteppableType.NotSteppable;    // 발판 여부
    [SerializeField] private List<GravityDirection> allowedDirections = new List<GravityDirection>();     // SpecificDirection일 경우 발판 방향

    // 중력 상태에 따라 장애물의 밟힘 판정
    public bool CanStep(GravityDirection playerGravity)
    {
        if (steppableType == PlatformSteppableType.NotSteppable)
        {
            return false;
        }

        if (steppableType == PlatformSteppableType.AnyDirection)
        {
            return true;
        }

        return allowedDirections.Contains(playerGravity);
    }
}