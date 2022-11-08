using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankala
{
    internal class HomePocket : GeneralPocket
    {
        public HomePocket(int index, Player player)
        {
            this.amountOfStones = 0;
            this.index = index;
            this.owner = player;

        }
        public HomePocket(int index, int amountOfStones, Player player)
        {
            this.amountOfStones = amountOfStones;
            this.index = index;
            this.owner = player;
        }
        public override bool UpdatePocket(Player player)
        {
            //owner never gets assigned so this is always true
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
