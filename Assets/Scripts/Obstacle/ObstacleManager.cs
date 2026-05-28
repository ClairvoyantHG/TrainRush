using System.Collections.Generic;
using UnityEngine;

// 모든 장애물 관리 매니저
public class ObstacleManager : SingletonBase<ObstacleManager>
{
    private List<ObstacleBase> activeObstacles = new List<ObstacleBase>();

    // 장애물 등록
    public void Register(ObstacleBase obstacle)
    {
        if (!activeObstacles.Contains(obstacle)) activeObstacles.Add(obstacle);
    }

    // 장애물 해제
    public void Unregister(ObstacleBase obstacle)
    {
        if (activeObstacles.Contains(obstacle)) activeObstacles.Remove(obstacle);
    }

    // 특정 좌표의 장애물 확인
    public ObstacleBase GetObstacleAt(int x, int y, float zPosition)
    {
        foreach (ObstacleBase obs in activeObstacles)
        {
            GridPosition obsPos = obs.GetCurrentGridPosition();

            if (obsPos.X == x && obsPos.Y == y)
            {
                float halfLength = obs.zLength * 0.5f;
                float minZ = obsPos.ZPosition - halfLength;
                float maxZ = obsPos.ZPosition + halfLength;

                // 조건에 맞는 장애물 반환
                if (zPosition >= minZ && zPosition <= maxZ)
                {
                    return obs; 
                }
            }
        }
        return null;
    }
    
    // 플레이어 피격 판정
    public ObstacleBase GetCollidedObstacle(GridPosition playerGrid, float playerZ, float zHitRadius)
    {
        foreach (ObstacleBase obs in activeObstacles)
        {
            GridPosition obsPos = obs.GetCurrentGridPosition();

            if (obsPos.X == playerGrid.X && obsPos.Y == playerGrid.Y)
            {
                float halfLength = obs.zLength * 0.5f;
                float minZ = obsPos.ZPosition - halfLength - zHitRadius;
                float maxZ = obsPos.ZPosition + halfLength + zHitRadius;

                if (playerZ >= minZ && playerZ <= maxZ)
                {
                    return obs;
                }
            }
        }
        return null;
    }

    // 중력에 따른 이동 계산
    public GridPosition CalculateFloorPosition(GridPosition currentPos, int maxGridHalfSize)
    {
        int stepX = 0;
        int stepY = 0;

        switch (currentPos.CurrentGravity)
        {
            case GravityDirection.Down: 
                stepY = -1;    
                break;

            case GravityDirection.Up: 
                stepY = 1; 
                break;

            case GravityDirection.Left: 
                stepX = -1; 
                break;

            case GravityDirection.Right:
                stepX = 1; 
                break;
        }

        int testX = currentPos.X;
        int testY = currentPos.Y;

        // 바닥 검사
        while (true)
        {
            int nextX = testX + stepX;
            int nextY = testY + stepY;

            // 칸을 벗어나면 검사 종료
            if (nextX < -maxGridHalfSize || nextX > maxGridHalfSize ||
                nextY < -maxGridHalfSize || nextY > maxGridHalfSize)
            {
                break;
            }

            // 발판이 있다면 검사 종료
            ObstacleBase obstacle = GetObstacleAt(nextX, nextY, currentPos.ZPosition);
            if (obstacle != null)
            {
                ObstaclePlatform platform = obstacle.GetComponent<ObstaclePlatform>();
                if (platform != null && platform.CanStep(currentPos.CurrentGravity))
                {
                    break;
                }
            }

            testX = nextX;
            testY = nextY;
        }

        return new GridPosition(testX, testY, currentPos.CurrentGravity, currentPos.ZPosition);
    }
}