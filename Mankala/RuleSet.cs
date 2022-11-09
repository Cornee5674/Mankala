using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mankala
{
    internal abstract class RuleSet
    {
        protected int amountOfPockets;
        protected int stonesPerPocket;
        protected bool hasHomePockets;
        public bool hasForcedMoves;
        public RuleSet()
        {
        }

        public abstract bool NeedToSwitchPlayer(Move move, Board board);
        public virtual bool GameIsFinished(Board board, Player playerAtTurn)
        {
            //In most versions the game ends when a player has no more pockets with stones in them
            GeneralPocket[] temp = board.pocketList;
            for (int i = 0; i < temp.Length; i++)
            {
                if (!(temp[i] is Pocket)) //Ignore homePockets
                    continue;
                if (temp[i].GetOwner() != playerAtTurn) //Ignore pockets of the opposing player
                    continue;
                if (temp[i].GetAmountOfStones() != 0) // If any pocket is not empty the game isnt finished yet
                    return false;
            }
            return true;
        }
        public abstract Board MakeBoard(Board b,Player p1, Player p2);
        protected GeneralPocket GetOpposing(GeneralPocket originalPocket, Board board)
        {
            //Returns the pocket which is on the other side of the board
            int index = originalPocket.GetIndex();
            int x = amountOfPockets - index;
            int pocketIndexOpposing = amountOfPockets + 2 + x;
            //Deals with the looping around the board
            if (pocketIndexOpposing >= board.pocketList.Length)
                pocketIndexOpposing -= board.pocketList.Length;
            return board.pocketList[pocketIndexOpposing];
        }
        protected void AddToHomePocket(Player p, int stones, Board board)
        {
            //Adds a specific amount of stones to the homepocket of the specified player
            if (board.pocketList[0].GetOwner() == p)
            {
                board.pocketList[0].AddStones(stones);
                return;
            }
            board.pocketList[amountOfPockets + 1].AddStones(stones);

        }
        public abstract bool IsForcedTurn(Move move, Board board);
    }

    public class ImpossibleStateException : Exception
    {
        public ImpossibleStateException(string message) : base(message) { }
    }
}
