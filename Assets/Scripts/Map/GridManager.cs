using UnityEngine;

// 좌표 변환 매니저
public class GridManager : SingletonBase<GridManager>
{
    public int gridHalfSize = 1;    // 중심에서 부터 한쪽 방향으로의 칸의 개수, n*2+1이 가로 또는 세로의 칸의 개수
    public float cellSize = 1f;     // 칸의 크기

    // 오브젝트 위치 데이터를 실제 유니티 월드 좌표로 변환
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        float worldX = gridPosition.X * cellSize;
        float worldY = gridPosition.Y * cellSize;
        return new Vector3(worldX, worldY, gridPosition.ZPosition);
    }

    // 현재 중력 방향의 물리적 회전값 반환
    public Quaternion GetWorldRotation(GravityDirection surface)
    {
        switch (surface)
        {
            case GravityDirection.Down: 
                return Quaternion.Euler(0f, 0f, 0f);
            
            case GravityDirection.Up:
                return Quaternion.Euler(0f, 0f, 180f);
            
            case GravityDirection.Left:
                return Quaternion.Euler(0f, 0f, -90f);
            
            case GravityDirection.Right: 
                return Quaternion.Euler(0f, 0f, 90f);
            
            default: 
                return Quaternion.identity;
        }
    }

    // 중력 변환 시 바닥 계산
    public GridPosition GetFallenPosition(GridPosition current, GravityDirection newGravity)
    {
        int targetX = current.X;
        int targetY = current.Y;

        switch (newGravity)
        {
            case GravityDirection.Down: targetY = -gridHalfSize; break;
            case GravityDirection.Up: targetY = gridHalfSize; break;
            case GravityDirection.Left: targetX = -gridHalfSize; break;
            case GravityDirection.Right: targetX = gridHalfSize; break;
        }

        return new GridPosition(targetX, targetY, newGravity, current.ZPosition);
    }
}