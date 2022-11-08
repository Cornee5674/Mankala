using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankala
{
    internal class Board
    {
        public GeneralPocket[] pocketList;

        public Board(GeneralPocket[] list)
        {
            pocketList = list;
        }

        public Move DoTurn(Player player, int pocketIndex)
        {
            int stonesInHand = pocketList[pocketIndex].EmptyPocket();
            
            while(stonesInHand != 0)
            {
                pocketIndex--; //Rotate counter-Clockwise so decrease pocketindex

                if (pocketIndex < 0) //Loop back to the end of the pockets if needed
                    pocketIndex = pocketList.Length - 1;

                //Only if we put a stone in this pocket we remove a stone from our hand
                if (pocketList[pocketIndex].UpdatePocket(player))
                    stonesInHand--;               
            }
            return new Move(player, pocketList[pocketIndex]);
        }
    }
}
