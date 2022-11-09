using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankala
{
    internal class HomePocket : GeneralPocket
    {
        bool isActive;
        public HomePocket(int index, Player player, bool isActive)
        {
            this.amountOfStones = 0;
            this.index = index;
            this.owner = player;
            this.isActive = isActive;

        }
        public HomePocket(int index, int amountOfStones, Player player, bool isActive)
        {
            this.amountOfStones = amountOfStones;
            this.index = index;
            this.owner = player;
            this.isActive = isActive;
        }
        public override bool UpdatePocket(Player player)
        {
            //If the home pocket doesn't partake in the game -> always skip
            if (!isActive)
                return false;
            if (player != owner)
                return false;
            
            amountOfStones++;
            return true;
        }
        public override int EmptyPocket()
        {
            throw new IllegalMoveException("Can't take stones from a home pocket");
        }
    }

    public class IllegalMoveException : Exception
    {
        public IllegalMoveException(string message) : base(message) { }
    }
}
