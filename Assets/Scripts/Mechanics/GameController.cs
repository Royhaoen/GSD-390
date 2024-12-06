using Platformer.Core;
using Platformer.Model;
using Platformer.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }
        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        void OnEnable()
        {
            Instance = this;
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this) 
            {
                // Debug teleport
                if (Input.GetKeyDown(KeyCode.T))
                {
                    var victoryZone = FindObjectOfType<VictoryZone>();
                    if (victoryZone != null && model.player != null)
                    {
                        Vector3 teleportPosition = victoryZone.transform.position;
                        teleportPosition.x -= 2f;
                        model.player.Teleport(teleportPosition);
                    }
                }

                // Restart when dead OR when victory and R pressed
                if (Input.GetKeyDown(KeyCode.R) && 
                    ((model.player != null && !model.player.health.IsAlive) || 
                     (model.player != null && !model.player.controlEnabled)))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                
                Simulation.Tick();
            }
        }
    }
}