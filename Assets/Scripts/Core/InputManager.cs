using UnityEngine;

// 게임 내 모든 사용자 입력을 중앙에서 관리하는 싱글턴 매니저
public class InputManager : SingletonBase<InputManager>, IInputProvider
{
    // 점프 입력
    public bool GetJumpInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    // 공격 입력
    public bool GetAttackInput()
    {
        return Input.GetKeyDown(KeyCode.Z);
    }

    // 중력 전환 입력
    public bool GetGravityInput(GravityDirection direction)
    {
        switch (direction)
        {
            case GravityDirection.Up: 
                return Input.GetKeyDown(KeyCode.UpArrow);

            case GravityDirection.Down: 
                return Input.GetKeyDown(KeyCode.DownArrow);

            case GravityDirection.Left: 
                return Input.GetKeyDown(KeyCode.LeftArrow);

            case GravityDirection.Right: 
                return Input.GetKeyDown(KeyCode.RightArrow);

            default: 
                return false;
        }
    }
}