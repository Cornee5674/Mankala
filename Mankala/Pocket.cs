using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankala
{
    internal class Pocket : GeneralPocket
    {
        //If no amount of stones are specified, use the first one otherwise the 2nd one
        public Pocket(int index, Player player)
        {
            this.amountOfStones = 0;
            this.index = index;
            this.owner = player;
        }
        public Pocket(int index, int amountOfStones, Player player)
        {
            this.amountOfStones = amountOfStones;
            this.index = index;
            this.owner = player;
        }
        public override bool UpdatePocket(Player player)
        {
            //Normal pockets will always be affected by a move
            amountOfStones++;
            return true;
        }
    }
}
