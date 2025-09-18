using UnityEngine;

[RequireComponent(typeof(PlayerController2D))]
[RequireComponent(typeof(Player))] // this is the MonoBehaviour that contains PlayerStats
public class PlayerStatsTracker : MonoBehaviour
{
    [Header("Energy Config")]
    public float maxEnergy = 100f;
    public float drainOnJump = 10f;
    public float drainOnDash = 10f;
    public float drainOnJumpDash = 15f;
    public float regenPerSecond = 5f;
    public float regenDelay = 0.75f; // Delay before energy starts regenerating

    private PlayerController2D controller;
    private Player player;
    private float lastSpendTime;

    public float Energy => player?.playerStats?.energy ?? 0f;

    void Awake()
    {
        controller = GetComponent<PlayerController2D>();
        player = GetComponent<Player>();

        if (player.playerStats == null) player.playerStats = new PlayerStats();
        player.playerStats.energy = Mathf.Clamp(player.playerStats.energy <= 0 ? maxEnergy : player.playerStats.energy, 0f, maxEnergy);
        lastSpendTime = -999f;
    }

    void Update()
    {
        bool busy = controller.isJumping || controller.isDashing || controller.isJumpDashing;
        if (!busy && Time.time - lastSpendTime >= regenDelay)
        {
            AddEnergy(regenPerSecond * Time.deltaTime);
        }
    }

    // Atomically checks & spends energy. Returns true if spend succeeded.
    public bool TrySpend(float amount)
    {
        if (amount <= 0f) return true;
        if (player.playerStats.energy < amount) return false;

        player.playerStats.energy = Mathf.Max(0f, player.playerStats.energy - amount);
        lastSpendTime = Time.time;
        return true;
    }

    public void AddEnergy(float amount)
    {
        if (amount <= 0f) return;
        player.playerStats.energy = Mathf.Min(maxEnergy, player.playerStats.energy + amount);
    }

    public float GetEnergy() => player?.playerStats?.energy ?? 0f;
}
