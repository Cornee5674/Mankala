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
            for (int i = 0; i < board.ListLength; i++)
            {
                if (!(board.GetAtIndex(i) is Pocket)) //Ignore homePockets
                    continue;
                if (!board.GetAtIndex(i).IsOwner(playerAtTurn)) //Ignore pockets of the opposing player
                    continue;
                if (board.GetAtIndex(i).AmountofStones != 0) // If any pocket is not empty the game isnt finished yet
                    return false;
            }
            return true;
        }
        public abstract Board MakeBoard(Board b,Player p1, Player p2);
        protected GeneralPocket GetOpposing(GeneralPocket originalPocket, Board board)
        {
            //Returns the pocket which is on the other side of the board
            int index = originalPocket.Index;
            int x = amountOfPockets - index;
            int pocketIndexOpposing = amountOfPockets + 2 + x;
            //Deals with the looping around the board
            if (pocketIndexOpposing >= board.ListLength)
                pocketIndexOpposing -= board.ListLength;
            return board.GetAtIndex(pocketIndexOpposing);
        }
        protected void AddToHomePocket(Player p, int stones, Board board)
        {
            int index;
            //Adds a specific amount of stones to the homepocket of the specified player
            if (board.HomepocketP1.IsOwner(p))
                index = board.HomepocketP1.Index;
            else
                index = board.HomepocketP2.Index;
            board.GetAtIndex(index).AddStones(stones);
        }
        public abstract bool IsForcedTurn(Move move, Board board);
    }

    public class ImpossibleStateException : Exception
    {
        public ImpossibleStateException(string message) : base(message) { }
    }
}
