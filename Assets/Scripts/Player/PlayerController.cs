using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerGravity))]
[RequireComponent(typeof(PlayerJump))]

// 플레이어의 컴포넌트 관리용 컴포넌트
public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerGravity playerGravity;
    private PlayerJump playerJump;
    private IInputProvider inputProvider;


    private void Start()
    {
        // 필수 컴포넌트 캐싱
        playerMovement = GetComponent<PlayerMovement>();
        playerGravity = GetComponent<PlayerGravity>();
        playerJump = GetComponent<PlayerJump>();

        // 입력 매니저 참조
        inputProvider = InputManager.Instance;

    }

    private void Update()
    { 
        bool isJumpPressed = inputProvider.GetJumpInput();
        float currentSpeed = playerMovement.GetForwardSpeed();
        GridPosition currentPos = playerMovement.GetCurrentGridPosition();

        playerJump.CalculateJump(isJumpPressed, currentSpeed, currentPos);
    }
}