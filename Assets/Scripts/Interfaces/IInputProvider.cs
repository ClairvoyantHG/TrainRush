// 게임 내의 입력 인터페이스
public interface IInputProvider
{
    bool GetJumpInput();
    bool GetAttackInput();
    bool GetGravityInput(GravityDirection direction);
}