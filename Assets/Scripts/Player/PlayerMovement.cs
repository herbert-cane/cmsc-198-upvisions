using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
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
    public AudioSource sfxSource; // Reference to the AudioSource
    public AudioSource musicSource; // Reference to the Music AudioSource

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
        sfxSource = GameObject.FindWithTag("SFX").GetComponent<AudioSource>();
        musicSource = GameObject.FindWithTag("Music").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (PauseController.isGamePaused || isInteracting)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetFloat("MoveMagnitude", 0f);
            return;
        } // Skip all input if game is paused

        ProcessInputs(); // Always read WASD

        // Run function: Holding "R" boosts movement speed
        if (Input.GetKey(KeyCode.R))
        {
            moveSpeed = 4f; // 2x the normal speed (boosting to 4 when "R" is held)
            anim.SetFloat("moveSpeed", 4f); // Adjust animation speed for running
            sfxSource.pitch = 1.5f; // Adjust the walk sound pitch for faster movement
        }
        else
        {
            moveSpeed = 2f; // Reset back to normal speed
            anim.SetFloat("moveSpeed", 1f); // Set animation speed to normal
            sfxSource.pitch = 1f; // Reset pitch to normal speed
        }

        // JumpDash (Space + Shift)
        if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.LeftShift) && !isJumpDashing)
        {
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
            if (input.sqrMagnitude > 0.01f && !sfxSource.isPlaying) // Play walk sound when moving
            {
                PlaySFX(walkSound); // Play walking sound
            }
            sfxSource.pitch = 1 + (input.magnitude * 0.5f); // Adjust pitch based on speed
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

            yield return null;
        }

        visual.localPosition = Vector3.zero;
        anim.SetBool("IsJumping", false);
        isJumping = false;
    }

    IEnumerator DoDash()
    {
        isDashing = true;
        anim.SetBool("IsDashing", true);

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

            rb.linearVelocity = dashDir * dashSpeed;

            float jumpOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            visual.localPosition = new Vector3(0, jumpOffset, 0);

            yield return null;
        }
        rb.linearVelocity = Vector2.zero;
        visual.localPosition = Vector3.zero;

        // Reset all flags
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
        if (sfxSource && clip)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}