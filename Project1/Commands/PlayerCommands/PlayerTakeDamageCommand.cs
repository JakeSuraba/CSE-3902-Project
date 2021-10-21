using Microsoft.Xna.Framework;
using Project1.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Commands
{
    class PlayerTakeDamageCommand : ICommand
    {
        Player player;
        int amount;

        public PlayerTakeDamageCommand(Player player, int amount)
        {
            this.player = player;
            this.amount = amount;
        }

        public void Execute()
        {
            foreach (Player player in GameObjectManager.Instance.GetObjectsOfType<Player>())
            {
                player.TakeDamage(amount);
            }
        }
    }
}
