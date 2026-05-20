using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerGravity))]
[RequireComponent(typeof(PlayerJump))]

// 플레이어의 컴포넌트 관리용 컴포넌트
public class PlayerController : SingletonBase<PlayerController>
{
    private PlayerMovement playerMovement;
    private PlayerGravity playerGravity;
    private PlayerJump playerJump;
    private IInputProvider inputProvider;

    protected override void Awake()
    {
        base.Awake(); // 싱글턴 초기화

        playerMovement = GetComponent<PlayerMovement>();
        playerGravity = GetComponent<PlayerGravity>();
        playerJump = GetComponent<PlayerJump>();
    }
    private void Start()
    {
        inputProvider = PlayerInputManager.Instance;

        if (inputProvider == null)
        {
            this.enabled = false;
            return;
        }
    }
    private void Update()
    { 
        bool isJumpPressed = inputProvider.GetJumpInput();
        float currentSpeed = playerMovement.GetForwardSpeed();
        GridPosition currentPos = playerMovement.GetCurrentGridPosition();

        playerJump.CalculateJump(isJumpPressed, currentSpeed, currentPos);
    }
}