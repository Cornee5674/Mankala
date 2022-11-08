using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankala
{
    class BoardFactory
    {
        public virtual Board createBoard(int pocketsPerPlayer, bool hasHomePockets, int stonesPerPocket, Player p1, Player p2)
        {
            int lengthArray;
            if (hasHomePockets)
                lengthArray = 2 * pocketsPerPlayer + 2;
            else
                lengthArray = 2 * pocketsPerPlayer;

            GeneralPocket[] pocketList = new GeneralPocket[lengthArray];

            pocketList[0] = new HomePocket(0,p1);
            for (int i = 1; i < pocketsPerPlayer + 1; i++)
            {
                pocketList[i] = new Pocket(i,stonesPerPocket,p1);
            }
            pocketList[pocketsPerPlayer + 1] = new HomePocket(pocketsPerPlayer+1,p2);
            for (int i = pocketsPerPlayer + 2; i < pocketList.Length; i++)
            {
                pocketList[i] = new Pocket(i,stonesPerPocket,p2);
            }
            return new Board(pocketList);
        }
    }

    class MankalaFactory : BoardFactory
    {
        public override Board createBoard(int pocketsPerPlayer, bool hasHomePockets, int stonesPerPocket, Player p1, Player p2)
        {
            return base.createBoard(pocketsPerPlayer, hasHomePockets, stonesPerPocket,p1,p2);
        }
    }
}
