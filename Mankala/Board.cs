using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("MankalaUnitTests")]

namespace Mankala
{
    internal class Board
    {
        private readonly GeneralPocket[] _pocketList;
        public Board(GeneralPocket[] list)
        {
            _pocketList = list;
        }
        public void printPockets()
        {
            for (int i = 0; i < _pocketList.Length; i++)
            {
                Console.WriteLine(_pocketList[i]);
            }
        }
        public Move DoTurn(Player player, int pocketIndex)
        {
            int stonesInHand = _pocketList[pocketIndex].EmptyPocket();
            
            while(stonesInHand != 0)
            {
                pocketIndex--; //Rotate counter-Clockwise so decrease pocketindex

                if (pocketIndex < 0) //Loop back to the end of the pockets if needed
                    pocketIndex = _pocketList.Length - 1;

                //Only if we put a stone in this pocket we remove a stone from our hand
                if (_pocketList[pocketIndex].UpdatePocket(player))
                    stonesInHand--;               
            }
            return new Move(player, _pocketList[pocketIndex]);
        }
        public GeneralPocket GetAtIndex(int index)
        {
            return _pocketList[index];
        }
        public GeneralPocket GetOpposing(GeneralPocket originalPocket)
        {
            //Returns the pocket which is on the other side of the board
            int amountOfPockets = (ListLength - 2) / 2;
            int index = originalPocket.Index;
            int x = amountOfPockets - index;
            int pocketIndexOpposing = amountOfPockets + 2 + x;
            //Deals with the looping around the board
            if (pocketIndexOpposing >= ListLength)
                pocketIndexOpposing -= ListLength;
            return GetAtIndex(pocketIndexOpposing);
        }
        public void AddToHomePocket(Player p, int stones)
        {
            int index;
            //Adds a specific amount of stones to the homepocket of the specified player
            if (HomepocketP1.IsOwner(p))
                index = HomepocketP1.Index;
            else
                index = HomepocketP2.Index;
            GetAtIndex(index).AddStones(stones);
        }
        public int ListLength
        {
            get { return _pocketList.Length; }
        }
        public GeneralPocket HomepocketP1
        {
            get { return _pocketList[ListLength / 2]; }
            set { _pocketList[ListLength / 2] = value; }
        }
        public GeneralPocket HomepocketP2
        {
            get { return _pocketList[0]; }
            set { _pocketList[0] = value; }
        }
    }
}
