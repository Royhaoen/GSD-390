using UnityEngine;
using Platformer.Mechanics;

public class PlayerStatsTracker : MonoBehaviour
{
    private PlayerController playerController;
    private bool wasGrounded;

    // Static class to track jumps
    public static class GameplayStats
    {
        public static int TotalJumps { get; private set; }
        public static void RecordJump() => TotalJumps++;
        public static void Reset() => TotalJumps = 0;
    }

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        wasGrounded = playerController.jumpState == PlayerController.JumpState.Grounded;
        // Reset stats when the component starts
        GameplayStats.Reset();
    }

    void Update()
    {
        if (playerController == null) return;

        bool isGrounded = playerController.jumpState == PlayerController.JumpState.Grounded;
        
        // Record jump when player leaves ground
        if (!isGrounded && wasGrounded)
        {
            GameplayStats.RecordJump();
        }

        wasGrounded = isGrounded;
    }
}