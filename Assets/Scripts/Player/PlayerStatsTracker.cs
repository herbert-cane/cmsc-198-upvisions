using UnityEngine;

[RequireComponent(typeof(PlayerController2D))]
[RequireComponent(typeof(Player))] // this is the MonoBehaviour that contains PlayerStats
public class PlayerStatsTracker : MonoBehaviour
{
    [Header("Energy & Stress Config")]
    public float maxEnergy = 100f;
    public float maxStress = 100f;
    public float drainOnJump = 10f;
    public float drainOnDash = 10f;
    public float drainOnJumpDash = 15f;
    public float regenPerSecond = 5f;
    public float regenDelay = 0.75f;
    public float stressReductionOnTalk = 20f;

    private PlayerController2D controller;
    private Player player;
    private float lastSpendTime;

    void Awake()
    {
        controller = GetComponent<PlayerController2D>();
        player = GetComponent<Player>();
        
        if (player == null)
        {
            Debug.LogError("Player component not found.");
            return;
        }
        
        if (player.playerStats == null)
        {
            player.playerStats = new PlayerStats();  // Initialize PlayerStats if null
        }

        player.playerStats.energy = Mathf.Clamp(player.playerStats.energy, 0f, maxEnergy);
        player.playerStats.stress = Mathf.Clamp(player.playerStats.stress, 0f, maxStress);
        lastSpendTime = -999f;
    }

    void Update()
    {
        bool busy = controller.isJumping || controller.isDashing || controller.isJumpDashing;
        if (!busy && Time.time - lastSpendTime >= regenDelay)
        {
            AddEnergy(regenPerSecond * Time.deltaTime);
        }

        if (player.playerStats.energy <= 20f)
        {
            IncreaseStress(1f * Time.deltaTime);
        }
    }

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

    public void ReduceStress(float amount)
    {
        if (amount <= 0f) return;
        player.playerStats.stress = Mathf.Max(0f, player.playerStats.stress - amount);
        
    }

    public void IncreaseStress(float amount)
    {
        if (amount <= 0f) return;
        player.playerStats.stress = Mathf.Min(maxStress, player.playerStats.stress + amount);
    }

    public void TalkToCounselor()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is null.");
            return;
        }

        if (player.playerStats == null)
        {
            Debug.LogError("PlayerStats reference is null.");
            return;
        }

        ReduceStress(stressReductionOnTalk);
    }


    public float GetEnergy() => player.playerStats.energy;
    public float GetStress() => player.playerStats.stress;
}