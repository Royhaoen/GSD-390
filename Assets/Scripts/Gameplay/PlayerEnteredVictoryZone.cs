using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
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
            // Find the victory text and show message
            var victoryText = GameObject.Find("VictoryMessage")?.GetComponent<TextMeshProUGUI>();
            if (victoryText != null)
            {
                victoryText.text = "Victory!\nPress R to play again";
            }
            else
            {
                Debug.LogError("No VictoryMessage TextMeshPro object found!");
            }

            model.player.animator.SetTrigger("victory");
            model.player.controlEnabled = false;
        }
    }
}