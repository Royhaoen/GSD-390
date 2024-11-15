using UnityEngine;
using System;
using Platformer.Mechanics;

public class PlayerStatsTracker : MonoBehaviour
{
    // Reference to the existing PlayerController
    private PlayerController playerController;

    // PROPERTY DEMONSTRATION
    // Property to track player's jump efficiency
    private float _jumpEfficiency = 0f;
    public float JumpEfficiency
    {
        get => _jumpEfficiency;
        private set
        {
            if (_jumpEfficiency != value)
            {
                _jumpEfficiency = value;
                OnJumpEfficiencyChanged?.Invoke(_jumpEfficiency);
            }
        }
    }

    // STATIC DEMONSTRATION
    // Static class to track gameplay statistics across sessions
    public static class GameplayStats
    {
        public static int TotalJumps { get; private set; }
        public static float LongestGroundedTime { get; private set; }
        public static float LongestAirtimeTime { get; private set; }

        public static void RecordJump() => TotalJumps++;
        public static void UpdateGroundedTime(float time) 
        {
            if (time > LongestGroundedTime)
            {
                LongestGroundedTime = time;
                Debug.Log($"New record for longest time grounded: {LongestGroundedTime:F2} seconds!");
            }
        }
        public static void UpdateAirtime(float time)
        {
            if (time > LongestAirtimeTime)
            {
                LongestAirtimeTime = time;
                Debug.Log($"New record for longest airtime: {LongestAirtimeTime:F2} seconds!");
            }
        }
    }

    // EVENT DEMONSTRATION
    // Event that fires when jump efficiency changes
    public event Action<float> OnJumpEfficiencyChanged;

    private float groundedStartTime;
    private float jumpStartTime;
    private bool wasGrounded;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerStatsTracker requires a PlayerController component!");
            enabled = false;
            return;
        }

        // Subscribe to our efficiency changed event
        OnJumpEfficiencyChanged += (efficiency) =>
        {
            if (efficiency > 0.8f)
                Debug.Log($"Great jump efficiency: {efficiency:P0}!");
        };

        groundedStartTime = Time.time;
        wasGrounded = playerController.jumpState == PlayerController.JumpState.Grounded;
    }

    void Update()
    {
        if (playerController == null) return;

        // Track grounded and airtime stats
        bool isGrounded = playerController.jumpState == PlayerController.JumpState.Grounded;
        
        // Detect when player lands
        if (isGrounded && !wasGrounded)
        {
            float airtime = Time.time - jumpStartTime;
            GameplayStats.UpdateAirtime(airtime);
            groundedStartTime = Time.time;
        }
        // Detect when player leaves ground
        else if (!isGrounded && wasGrounded)
        {
            float groundedTime = Time.time - groundedStartTime;
            GameplayStats.UpdateGroundedTime(groundedTime);
            jumpStartTime = Time.time;
            GameplayStats.RecordJump();
        }

        wasGrounded = isGrounded;

        // Calculate jump efficiency (based on horizontal movement during jump)
        if (playerController.jumpState == PlayerController.JumpState.InFlight)
        {
            float horizontalSpeed = Mathf.Abs(playerController.velocity.x);
            JumpEfficiency = horizontalSpeed / playerController.maxSpeed;
        }

        // Log periodic stats
        if (Time.frameCount % 300 == 0) // Every 5 seconds at 60fps
        {
            Debug.Log($"Player Stats:\n" +
                     $"Total Jumps: {GameplayStats.TotalJumps}\n" +
                     $"Longest Airtime: {GameplayStats.LongestAirtimeTime:F2}s\n" +
                     $"Longest Grounded Time: {GameplayStats.LongestGroundedTime:F2}s\n" +
                     $"Current Jump Efficiency: {JumpEfficiency:P0}");
        }
    }
}