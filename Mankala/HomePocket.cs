using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankala
{
    internal class HomePocket : GeneralPocket
    {
        readonly bool isActive;
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
            //Method returns true if this pocket was updated

            //If the home pocket doesn't partake in the game -> always skip
            if (!isActive)
                return false;
            //If the home pocket doesn't belong to the player then don't throw in a stone
            if (player != owner)
                return false;
            
            amountOfStones++;
            return true;
        }
        public override int EmptyPocket()
        {
            //With the current rules it is never possible to take stones from a home pocket
            //Maybe in the future a new ruleset will come that will allow this
            throw new IllegalMoveException("Can't take stones from a home pocket");
        }
    }

    public class IllegalMoveException : Exception
    {
        public IllegalMoveException(string message) : base(message) { }
    }
}
