using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerJump))]

// 플레이어 관리용
public class PlayerController : SingletonBase<PlayerController>
{
    private PlayerMovement playerMovement;
    private PlayerJump playerJump;
    private IInputProvider inputProvider;

    protected override void Awake()
    {
        base.Awake(); // 싱글턴 초기화

        playerMovement = GetComponent<PlayerMovement>();
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