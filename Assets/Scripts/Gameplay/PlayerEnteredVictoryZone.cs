using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using Platformer.UI;
using UnityEngine;
using TMPro;

namespace Platformer.Gameplay
{
    public class PlayerEnteredVictoryZone : Simulation.Event<PlayerEnteredVictoryZone>
    {
        public VictoryZone victoryZone;
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            model.player.animator.SetTrigger("victory");
            model.player.controlEnabled = false;
            
            // Stop timer when victory achieved
            var gameStats = Object.FindObjectOfType<GameStats>();
            if (gameStats != null)
            {
                gameStats.StopTimer();
            }

            // Show victory message
            var victoryText = GameObject.Find("VictoryMessage")?.GetComponent<TextMeshProUGUI>();
            if (victoryText != null)
            {
                victoryText.text = "Victory!\nPress R to play again";
            }
        }
    }
}