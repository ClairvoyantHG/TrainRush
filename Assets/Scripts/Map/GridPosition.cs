// 오브젝트 위치 데이터 구조체
public struct GridPosition
{
    // 가상 배열의 좌우 위치, 중심이 0
    public int X;

    // 가상 배열의 상하 위치, 중심이 0
    public int Y;

    // 오브젝트에 적용되는 중력의 방향
    public GravityDirection CurrentGravity;

    // 실제 z축 좌표
    public float ZPosition;

    public GridPosition(int x, int y, GravityDirection gravity, float zPosition)
    {
        this.X = x;
        this.Y = y;
        this.CurrentGravity = gravity;
        this.ZPosition = zPosition;
    }
}