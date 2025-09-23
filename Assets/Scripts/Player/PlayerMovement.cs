using System.Collections;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    [Header("Visual Layers")]
    public Transform visual;
    public Transform jumpShadow;
    private SpriteRenderer jumpShadowRenderer;

    [Header("Jump Settings")]
    public float jumpHeight = 0.5f;
    public float jumpDuration = 0.6f;

    [Header("Dash Settings")]
    public float dashSpeed = 5f;
    public float dashDuration = 0.2f;
    private Vector2 input;
    private Vector2 lastMoveDirection = Vector2.down;
    private Animator anim;
    private SpriteRenderer sr;
    public bool isJumping = false;
    public bool isDashing = false;
    public bool isJumpDashing = false;
    public bool isInteracting = false;

    // Sound Effects
    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip walkSound;
    public AudioClip jumpDashSound;
    public AudioSource audioSource; // Reference to the AudioSource
    private Coroutine currentAction;

    // Reference to the PlayerStatsTracker script
    private PlayerStatsTracker statsTracker;

    void Start()
    {
        anim = visual.GetComponent<Animator>();
        sr = visual.GetComponent<SpriteRenderer>();
        jumpShadowRenderer = jumpShadow.GetComponent<SpriteRenderer>();

        lastMoveDirection = Vector2.down;

        // Get the PlayerStatsTracker component to track energy
        statsTracker = GetComponent<PlayerStatsTracker>();
        audioSource = FindFirstObjectByType<AudioSource>();
    }

    void Update()
    {
        if (PauseController.isGamePaused || isInteracting)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetFloat("MoveMagnitude", 0f);
            return;
        } // Skip all input if game is paused

        ProcessInputs(); //  Always read WASD

        // JumpDash (Space + Shift)
        if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.LeftShift) && !isJumpDashing)
        {
            // Spend energy before starting coroutine
            if (statsTracker != null && statsTracker.TrySpend(statsTracker.drainOnJumpDash))
            {
                if (currentAction != null) StopCoroutine(currentAction);
                currentAction = StartCoroutine(DoJumpDash());
                PlaySFX(jumpDashSound);
            }
            else
            {
                Debug.Log("Not enough energy to JumpDash!");
            }
        }
        // Jump
        else if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isDashing && !isJumpDashing)
        {
            // Spend energy before starting coroutine
            if (statsTracker != null && statsTracker.TrySpend(statsTracker.drainOnJump))
            {
                if (currentAction != null) StopCoroutine(currentAction);
                currentAction = StartCoroutine(DoJump());
                PlaySFX(jumpSound);
            }
            else
            {
                Debug.Log("Not enough energy to Jump!");
            }
        }
        // Dash
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isJumping && !isJumpDashing)
            {
                // Spend energy before starting coroutine
                if (statsTracker != null && statsTracker.TrySpend(statsTracker.drainOnJumpDash))
                {
                    if (currentAction != null) StopCoroutine(currentAction);
                    currentAction = StartCoroutine(DoJumpDash());
                    PlaySFX(jumpDashSound);
                }
                else
                {
                    Debug.Log("Not enough energy to JumpDash!");
                }
            }
            else if (!isDashing && !isJumpDashing)
            {
                // Spend energy before starting coroutine
                if (statsTracker != null && statsTracker.TrySpend(statsTracker.drainOnDash))
                {
                    if (currentAction != null) StopCoroutine(currentAction);
                    currentAction = StartCoroutine(DoDash());
                    PlaySFX(dashSound);
                }
                else
                {
                    Debug.Log("Not enough energy to Dash!");
                }
            }
        }

        Animate();
    }

    private void FixedUpdate()
    {
        if (PauseController.isGamePaused || isInteracting)
        {
            return;
        } // Skip all movement if game is paused
        if (!isJumping && !isDashing && !isJumpDashing)
        {
            rb.linearVelocity = input * moveSpeed; // normal movement
            if (input.sqrMagnitude > 0.01f && !audioSource.isPlaying) // Play walk sound when moving
            {
                PlaySFX(walkSound); // Play walking sound
            }
            audioSource.pitch = 1 + (input.magnitude * 0.5f); // Adjust pitch based on speed
        }
        else if (isJumping && !isJumpDashing)
        {
            rb.linearVelocity = input * moveSpeed * 0.75f; // slight air control
        }
        // else → JumpDash handles velocity itself
    }

    void ProcessInputs()
    {
        if (PauseController.isGamePaused)
        {
            input = Vector2.zero;
            return;
        } // Skip all input if game is paused

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        input = new Vector2(moveX, moveY);
        input = Vector2.ClampMagnitude(input, 1f);

        if (input.sqrMagnitude > 0.01f)
            lastMoveDirection = input.normalized;
    }

    IEnumerator DoJump()
    {
        isJumping = true;
        anim.SetBool("IsJumping", true);

        float elapsed = 0f;
        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / jumpDuration;

            rb.position += input * moveSpeed * Time.deltaTime;

            float jumpOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            visual.localPosition = new Vector3(0, jumpOffset, 0);

            float squash = Mathf.Lerp(1f, 0.6f, Mathf.Sin(t * Mathf.PI));
            float alpha = Mathf.Lerp(1f, 0.4f, Mathf.Sin(t * Mathf.PI));
            jumpShadow.localScale = new Vector3(squash, squash, 1f);
            var c = jumpShadowRenderer.color; c.a = alpha; jumpShadowRenderer.color = c;

            yield return null;
        }

        visual.localPosition = Vector3.zero;
        jumpShadow.localScale = Vector3.one;
        var finalC = jumpShadowRenderer.color; finalC.a = 1f; jumpShadowRenderer.color = finalC;

        anim.SetBool("IsJumping", false);
        isJumping = false;
    }

    IEnumerator DoDash()
    {
        isDashing = true;
        anim.SetBool("IsDashing", true);

        // Use current input if moving, otherwise last direction
        Vector2 dashDir = (input.sqrMagnitude > 0.01f) ? input.normalized : lastMoveDirection;

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            elapsed += Time.deltaTime;

            rb.linearVelocity = dashDir * dashSpeed;

            yield return null;
        }
        rb.linearVelocity = Vector2.zero;

        anim.SetBool("IsDashing", false);
        isDashing = false;
    }

    IEnumerator DoJumpDash()
    {
        if (isJumping) isJumping = false;
        if (isDashing) isDashing = false;

        isJumping = false;
        isDashing = false;
        isJumpDashing = true;

        anim.SetBool("IsJumping", true);
        anim.SetBool("IsDashing", true);

        Vector2 dashDir = (input.sqrMagnitude > 0.01f) ? input.normalized : lastMoveDirection;

        float elapsed = 0f;
        float duration = Mathf.Max(jumpDuration, dashDuration); // combine both lengths

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Dash movement
            rb.linearVelocity = dashDir * dashSpeed;

            // Jump vertical arc
            float jumpOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            visual.localPosition = new Vector3(0, jumpOffset, 0);

            float squash = Mathf.Lerp(1f, 0.6f, Mathf.Sin(t * Mathf.PI));
            float alpha = Mathf.Lerp(1f, 0.4f, Mathf.Sin(t * Mathf.PI));
            jumpShadow.localScale = new Vector3(squash, squash, 1f);
            var c = jumpShadowRenderer.color; c.a = alpha; jumpShadowRenderer.color = c;

            yield return null;
        }
        rb.linearVelocity = Vector2.zero;
        visual.localPosition = Vector3.zero;
        jumpShadow.localScale = Vector3.one;
        var finalC = jumpShadowRenderer.color;
        finalC.a = 1f;
        jumpShadowRenderer.color = finalC;

        // ✅ Reset all flags
        isJumping = false;
        isDashing = false;
        isJumpDashing = false;

        anim.SetBool("IsJumping", false);
        anim.SetBool("IsDashing", false);

        ProcessInputs(); // refresh control
    }

    void Animate()
    {
        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveY", input.y);
        anim.SetFloat("MoveMagnitude", input.magnitude);

        if (input.sqrMagnitude > 0.01f)
            lastMoveDirection = input.normalized;

        anim.SetFloat("LastMoveX", lastMoveDirection.x);
        anim.SetFloat("LastMoveY", lastMoveDirection.y);
    }
    
        // Method to play sound effects
    void PlaySFX(AudioClip clip)
    {
        if (audioSource && clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}