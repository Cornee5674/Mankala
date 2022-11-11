using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankala
{
    class BoardFactory
    {
        public Board CreateBoard(int pocketsPerPlayer, bool hasHomePockets, int stonesPerPocket, Player p1, Player p2)
        {
            //Calculate the total amount of pockets on the board
            int lengthArray;
            lengthArray = 2 * pocketsPerPlayer + 2;
            
            GeneralPocket[] pocketList = new GeneralPocket[lengthArray];

            //Creation of p2 pockets
            pocketList[0] = new HomePocket(0,p2,hasHomePockets);
            for (int i = 1; i < pocketsPerPlayer + 1; i++)
            {
                pocketList[i] = new Pocket(i,stonesPerPocket,p2);
            }

            //Creation of p1 pockets
            //Note that pocketsPerPlayer + 1 will always be the homepocket of the other player
            pocketList[pocketsPerPlayer + 1] = new HomePocket(pocketsPerPlayer+1,p1,hasHomePockets);
            for (int i = pocketsPerPlayer + 2; i < pocketList.Length; i++)
            {
                pocketList[i] = new Pocket(i,stonesPerPocket,p1);
            }
            
            return new Board(pocketList);
        }
    }

}
