using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float checkSize = 0.02f;
    [SerializeField] bool isplayer1 = true;
    InputSystem_Actions controls;
    InputAction jumpAction;
    InputAction moveAction;
    Rigidbody2D body;
    BoxCollider2D hitbox;
    bool isGrounded;
    bool jumpJustPressed;
    bool jumpJustReleased;
    bool jumpready;
    bool dashready;
    bool dashdownready;

    [SerializeField] float jumpForce = 15f;
    [SerializeField] float gravityForce = 25f;
    [SerializeField] float horizontalVelocity = 10f;

    checkcollisionwall playerdir;

    void Awake()
    {
         playerdir = GetComponent<checkcollisionwall>();

        controls = new InputSystem_Actions();
        // can be skipped, since OnEnable() is also called right after Awake()
        // but calling it twice won't do any harm
        // controls.Enable();
        if (isplayer1)
        {
            jumpAction = controls.Player.Jump;
            moveAction = controls.Player.Move;
        }
        else
        {
            jumpAction = controls.Player2.Jump;
            moveAction = controls.Player2.Move;    
        }
        body = GetComponent<Rigidbody2D>();
        // disable natural gravity, since we will control it manually
        body.gravityScale = 0f;
        hitbox = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        jumpJustPressed = jumpJustPressed || jumpAction.WasPressedThisFrame();
        jumpJustReleased = jumpJustReleased || jumpAction.WasReleasedThisFrame();
    }

    void FixedUpdate()
    {
        UpdateJumpStates_ColliderVariant();

        HandleVerticalMovement();
        HandleHorizontalMovement();
    }

    void UpdateJumpStates_ColliderVariant()
    {
        if (!Mathf.Approximately(body.linearVelocityY, 0f))
        {
            isGrounded = false;
            return;
        }

        Vector2 hitboxPosition = transform.position;
        hitboxPosition += hitbox.offset;
        Vector2 hitboxSize = hitbox.size;
        hitboxSize.x -= 0.01f;

        // 0.01f needed to avoid border collission with player hitbox
        hitboxPosition.y -= hitboxSize.y * (0.5f + checkSize / 2) + 0.01f;
        hitboxSize.y = hitboxSize.y * checkSize;
        isGrounded = Physics2D.OverlapBox(hitboxPosition, hitboxSize, 0f) != null;

        hitboxPosition.y += checkSize / 2;
        hitboxPosition.x -= hitboxSize.x / 2;
        Debug.DrawRay(hitboxPosition, Vector2.down, Color.red);
        hitboxPosition.x += hitboxSize.x;
        Debug.DrawRay(hitboxPosition, Vector2.down, Color.red);
    }

    void HandleVerticalMovement()
    {
        if (isGrounded)
        {
            jumpready = true;
            dashready = true;
            dashdownready = true;
            body.linearVelocityY = 0f;
        }
        else
        {
            body.linearVelocityY -= gravityForce * Time.fixedDeltaTime;
            float direction = moveAction.ReadValue<Vector2>().y;
            if (direction < 0)
            {
                if (body.linearVelocityY > gravityForce*-0.5f)
                {
                    body.linearVelocityY = gravityForce*-0.5f;
                }
                float direction2 = moveAction.ReadValue<Vector2>().x;
                if (direction2 > 0 && !jumpready && dashdownready)
                {
                    body.linearVelocityX += jumpForce * 1f;
                    dashdownready = false;
                }
                else if (direction2 < 0 && !jumpready && dashdownready)
                {
                    body.linearVelocityX -= jumpForce * 1f;
                    dashdownready = false;
                }
                body.linearVelocityY -= gravityForce * Time.fixedDeltaTime;   
            }
        }
        if (jumpJustPressed && !jumpready)
        {
            float direction = moveAction.ReadValue<Vector2>().x;
            if (playerdir.walltouching == "left" && direction > 0)
            {
                body.linearVelocityX += jumpForce * 1.5f;
                body.linearVelocityY = jumpForce * 1.1f;
            }
            else if (playerdir.walltouching == "right" && direction < 0)
            {
                body.linearVelocityX -= jumpForce * 1.5f;
                body.linearVelocityY = jumpForce * 1.1f;
            }
            else if (playerdir.walltouching == "both")
            {
                body.linearVelocityY = jumpForce * 1.3f;
            }
            else
            {
                if (dashready)
                {
                    if (direction > 0)
                    {
                        body.linearVelocityX += jumpForce * 1.5f;
                        body.linearVelocityY = jumpForce * 0.5f;
                        dashready = false;
                    }
                    else if (direction < 0)
                    {
                        body.linearVelocityX -= jumpForce * 1.5f;
                        body.linearVelocityY = jumpForce * 0.5f;
                        dashready = false;
                    }   
                }
            }
        }
        if (jumpJustPressed && jumpready)
        {
            body.linearVelocityY = jumpForce;
            jumpready = false;  
        }
        jumpJustPressed = false;
        jumpJustReleased = false;
    }

    void HandleHorizontalMovement()
    {
    float direction = moveAction.ReadValue<Vector2>().x;
        if (direction > 0)
        {
            direction = 1;
        }
        if (direction < 0)
        {
            direction = -1;
        }

    float targetVelocity = direction * horizontalVelocity;

    body.linearVelocityX = Mathf.MoveTowards(
        body.linearVelocityX,
        targetVelocity,
        0.96f
    );
    }

    void HandleHorizontalMovement2()
    {
        float direction = moveAction.ReadValue<Vector2>().x;
        body.linearVelocityX = direction * horizontalVelocity;
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void OnDestroy()
    {
        controls.Dispose();
    }
}